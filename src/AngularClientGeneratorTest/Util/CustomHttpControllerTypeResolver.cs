using System;
using System.Collections.Generic;
using System.Web.Http.Dispatcher;

namespace AngularClientGeneratorTest.Util
{
    public class CustomHttpControllerTypeResolver : DefaultHttpControllerTypeResolver
    {
        private static List<Type> controllerTypesToDiscover;
        private static List<Type> ControllerTypesToDiscover => controllerTypesToDiscover ?? (controllerTypesToDiscover = new List<Type>());

        public static void ClearTypesToDiscover()
        {
            ControllerTypesToDiscover.Clear();
        }

        public static void RegisterTypeToDiscover(Type t)
        {
            ControllerTypesToDiscover.Add(t);
        }

        public CustomHttpControllerTypeResolver() : base(IsHttpEndpoint)
        { }

        internal static bool IsHttpEndpoint(Type t)
        {
            if (t == null) throw new ArgumentNullException("t");

            return ControllerTypesToDiscover.Contains(t);
        }
    }
}
