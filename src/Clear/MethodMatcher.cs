using System;
using System.Linq;
using System.Reflection;

namespace Clear
{
    public class MethodMatcher
    {
        private readonly Type _type;

        public MethodMatcher(Type type)
        {
            _type = type;
        }

        public (MethodInfo, object[]) TryFindMethodToInvoke(string command, string[] args)
        {
            var methods = _type.GetMethods(BindingFlags.Static | BindingFlags.Public);

            var methodsToExecute = methods.Where(m =>
                string.Equals(new Method(m).Name, command, StringComparison.InvariantCultureIgnoreCase)).ToList();

            if (methodsToExecute?.Count == 0)
            {
                return (null, null);
            }

            foreach (var method in methodsToExecute)
            {
                var methodParameters = method.GetParameters();

                var parameterMatcher = new ParameterMatcher();

                var (success, parametersToSupply) = parameterMatcher.TryGetParameterValues(methodParameters, args);

                if (success)
                {
                    return (method, parametersToSupply);
                }
            }

            return (null, null);
        }
    }
}
