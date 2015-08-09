using System;
using System.Reflection;

using Genetics.Attributes;

namespace Genetics.Mappings
{
    public class MethodMapping
    {
        public MethodMapping(Type type, MethodInfo method, SpliceEventAttribute attr)
        {
            Type = type;
            Method = method;
            Attribute = attr;

            MapMethod();
        }

        public virtual Type Type { get; protected set; }

        public virtual MethodInfo Method { get; protected set; }

        public virtual SpliceEventAttribute Attribute { get; protected set; }

        private void MapMethod()
        {
            // do something
        }

        public static MethodMatch CompareMethods(MethodInfo eventMethod, MethodInfo targetMethod)
        {
            // check for valid returns
            if (!CompatibleReturnTypes(eventMethod.ReturnType, targetMethod.ReturnType))
            {
                throw new ArgumentException("The return types must be the same, or more specific.");
            }

            var targetParameters = targetMethod.GetParameters();

            // TODO: we want to add support for simple methods
            //// check for ()
            //if (targetParameters.Length == 0)
            //{
            //    return MethodMatch.NoParameters;
            //}
            //
            //// check for (object sender)
            //if (targetParameters.Length == 1 && 
            //    targetParameters[0].ParameterType.IsAssignableFrom(typeof(object)))
            //{
            //    return MethodMatch.SenderParameter;
            //}

            var eventParameters = eventMethod.GetParameters();

            // check for similar (assignable, ...)
            if (eventParameters.Length != targetParameters.Length)
            {
                throw new ArgumentException("The number of parameters must be the same.");
            }
            for (int i = 0; i < eventParameters.Length; i++)
            {
                var eventParameter = eventParameters[i];
                var targetParameter = targetParameters[i];
                if (!CompatibleParameterTypes(eventParameter.ParameterType, targetParameter.ParameterType))
                {
                    throw new ArgumentException("The parameters must be the same, or less specific than the event parameters.");
                }
            }

            return MethodMatch.SimilarParameters;
        }

        public static bool CompatibleParameterTypes(Type eventParam, Type targetParam)
        {
            // same type, or assignable class type, or assignable enum type
            return (eventParam == targetParam) ||
                   (!targetParam.IsValueType && targetParam.IsAssignableFrom(eventParam)) ||
                   (eventParam.IsEnum && Enum.GetUnderlyingType(eventParam) == targetParam);
        }

        public static bool CompatibleReturnTypes(Type eventReturn, Type targetReturn)
        {
            // same type, or assignable class type
            return (eventReturn == targetReturn) ||
                   (!targetReturn.IsValueType && eventReturn.IsAssignableFrom(targetReturn));
        }

        public enum MethodMatch
        {
            //NoParameters,
            //SenderParameter,
            SimilarParameters
        }
    }
}
