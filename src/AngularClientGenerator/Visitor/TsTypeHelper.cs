using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using AngularClientGenerator.DescriptionParts;

namespace AngularClientGenerator.Visitor
{
    public static class TsTypeHelper
    {
        public static bool IsEnumerable(this TypeDescriptionPart typeDescriptionPart)
        {
            return typeDescriptionPart.Type.GetInterfaces().Any(ti => ti == typeof(IEnumerable));
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

        private static readonly Type[] IgnoredTypesOnDefinition = { typeof(int), typeof(double), typeof(float), typeof(decimal), typeof(string), typeof(bool), typeof(void), typeof(IHttpActionResult) };

        public static bool IsIgnoredType(this TypeDescriptionPart typeDescriptionPart)
        {
            return IgnoredTypesOnDefinition.Contains(typeDescriptionPart.Type);
        }

        private static readonly Type[] NumberTypes = { typeof(int), typeof(double), typeof(float), typeof(decimal) };
        public static bool IsNumberType(this TypeDescriptionPart typeDescriptionPart)
        {
            return NumberTypes.Contains(typeDescriptionPart.Type);
        }
    }
}
