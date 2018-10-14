using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace MovieHunt.Extensions
{
    public static class TypeExtensions
    {
        public static bool IsClosedTypeOf(this Type type, Type openGeneric)
        {
            return TypesAssignableFrom(type).Any(t => t.GetTypeInfo().IsGenericType &&
                                                      !type.GetTypeInfo().ContainsGenericParameters && t.GetGenericTypeDefinition() == openGeneric);
        }

        private static IEnumerable<Type> TypesAssignableFrom(Type candidateType)
        {
            return candidateType.GetTypeInfo().ImplementedInterfaces.Concat(
                Traverse.Across(candidateType, t => t.GetTypeInfo().BaseType));
        }
    }
}
