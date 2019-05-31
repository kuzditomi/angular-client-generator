using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using AngularClientGenerator.DescriptionParts;

namespace AngularClientGenerator.Extensions
{
    public static class TypeDescriptionPartExtensions
    {
        public static bool IsEnumerable(this TypeDescriptionPart typeDescriptionPart)
        {
            return typeDescriptionPart.Type.GetInterfaces().Any(ti => ti == typeof(IEnumerable));
        }

        public static bool IsGeneric(this TypeDescriptionPart typeDescriptionPart)
        {
            return typeDescriptionPart.Type.IsGenericType;
        }

        public static bool IsNullable(this TypeDescriptionPart typeDescriptionPart)
        {
            return typeDescriptionPart.Type.IsGenericType &&
                   typeDescriptionPart.Type.GetGenericTypeDefinition() == typeof(Nullable<>);
        }
        
        public static bool IsTask(this TypeDescriptionPart typeDescriptionPart)
        {
            return typeDescriptionPart.Type == typeof(Task) || typeDescriptionPart.Type.BaseType == typeof(Task);
        }

        private static readonly Type[] IgnoredTypesOnDefinition = { typeof(int), typeof(double), typeof(float), typeof(decimal), typeof(string), typeof(bool), typeof(DateTime), typeof(void), typeof(HttpResponseMessage), typeof(Guid) };

        public static bool IsIgnoredType(this TypeDescriptionPart typeDescriptionPart)
        {
            return IgnoredTypesOnDefinition.Contains(typeDescriptionPart.Type) || typeDescriptionPart.Type.Name == "IHttpActionResult";
        }

        public static bool IsDictionary(this TypeDescriptionPart typeDescriptionPart)
        {
            return typeDescriptionPart.Type.IsGenericType
                && (typeof(IDictionary).IsAssignableFrom(typeDescriptionPart.Type) || typeDescriptionPart.Type.GetGenericTypeDefinition() == typeof(IDictionary<,>));
        }

        private static readonly Type[] NumberTypes = { typeof(int), typeof(double), typeof(float), typeof(decimal) };
        public static bool IsNumberType(this TypeDescriptionPart typeDescriptionPart)
        {
            return NumberTypes.Contains(typeDescriptionPart.Type);
        }

        private static readonly Type[] simpleTypes = { typeof(int), typeof(double), typeof(float), typeof(decimal), typeof(string), typeof(bool), typeof(DateTime), typeof(void), typeof(Guid) };

        /// <summary>
        /// Something is basicly 'complex' if it is and object in javascript 
        /// </summary>
        /// <param name="typeDescriptionPart"></param>
        /// <returns></returns>
        public static bool IsComplex(this TypeDescriptionPart typeDescriptionPart)
        {
            if (simpleTypes.Contains(typeDescriptionPart.Type))
                return false;

            if (typeDescriptionPart.Type.IsEnum)
                return false;

            if (typeDescriptionPart.IsEnumerable())
            {
                var isArray = typeDescriptionPart.Type.IsArray;
                if (isArray)
                {
                    var typeofArray = typeDescriptionPart.Type.GetElementType();
                    return new TypeDescriptionPart(typeofArray).IsComplex();
                }
                else
                {
                    var genericType = typeDescriptionPart.Type.GetGenericArguments()[0];
                    return new TypeDescriptionPart(genericType).IsComplex();
                }
            }

            if (typeDescriptionPart.IsNullable())
            {
                var genericType = typeDescriptionPart.Type.GetGenericArguments()[0];
                return new TypeDescriptionPart(genericType).IsComplex();
            }

            return true;
        }
    }
}
