using System;
using System.Linq;
using System.Reflection;

namespace Clear
{
    public class FirstMethodFinder
    {
        private readonly Type _type;

        public FirstMethodFinder(Type type)
        {
            _type = type;
        }

        public (MethodInfo, object[]) TryFindMethodToInvoke(string command, string[] args)
        {
            var methods = _type.GetMethods(BindingFlags.Static | BindingFlags.Public);

            var methodToExecute = methods.Where(m => string.Equals(m.Name, command, StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();

            if (methodToExecute == null)
            {
                return (null, null);
            }

            var methodParameters = methodToExecute.GetParameters();

            var parameterMatcher = new ParameterMatcher();
            var (success, parametersToSupply) = parameterMatcher.TryGetParameterValues(methodParameters, args);

            return (success ? methodToExecute : null, parametersToSupply);
        }
    }
}
