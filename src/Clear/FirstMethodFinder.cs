using System;
using System.Collections.Generic;
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

            var (success, parametersToSupply) = TryGetParameterValues(methodParameters, args);

            return (success ? methodToExecute : null, parametersToSupply);
        }

        private (bool, object[]) TryGetParameterValues(ParameterInfo[] parameterTypes, string[] args)
        {
            var parameterValues = new List<(bool?, object)>(parameterTypes.Length);
            parameterValues.AddRange(Enumerable.Repeat<(bool?, object)>((default, null), parameterTypes.Length));

            var argsQueue = new Queue<string>(args);

            var parameterConverter = new ParameterTypeConverter();
            for (int i = 0; i < args.Length && parameterValues.FindIndex(pv => pv.Item1 == null) >= 0; i++)
            {
                // get the parameter value
                var currentArg = argsQueue.Dequeue();

                // check if it's a named argument
                var namedArgument = parameterTypes.FirstOrDefault(p => string.Equals($"--{p.Name}", currentArg, StringComparison.InvariantCultureIgnoreCase));

                // if a named argument, add the next value as the parameter value
                if (namedArgument != null)
                {
                    var paramIndex = Array.IndexOf(parameterTypes, namedArgument);
                    if (args.Length >= i + 2)
                    {
                        // only convert and assign the value if it hasn't already been assigned, e.g. by an unnamed argument
                        if (parameterValues[paramIndex].Item1 == null)
                        {
                            currentArg = argsQueue.Dequeue();
                            parameterValues[paramIndex] = parameterConverter.TryChangeValue(currentArg, namedArgument.ParameterType);
                        }

                        i += 1;
                    }
                    else // the last parameter and it has a default value
                    {
                        if (namedArgument.HasDefaultValue)
                        {
                            parameterValues[i] = (true, namedArgument.DefaultValue);
                        }
                    }
                }
                // try to use the current argument as the next parameter value 
                else
                {
                    // find the first null position
                    var emptyIndex = parameterValues.FindIndex(pv => pv.Item1 == null);

                    // get the named argument for the position
                    var unnamedArgument = parameterTypes[emptyIndex];
                    parameterValues[emptyIndex] = parameterConverter.TryChangeValue(currentArg, unnamedArgument.ParameterType);
                }
            }

            // if there are still arguments left, more were passed in than the method allows, not successful match
            if(argsQueue.Count > 0)
            {
                return (false, parameterValues.Select(i => i.Item2).ToArray());
            }

            // assign any default values to unassigned parameters
            for (int i = 0; i < parameterTypes.Length; i++)
            {
                if (parameterValues[i].Item1 == null && parameterTypes[i].HasDefaultValue)
                {
                    parameterValues[i] = (true, parameterTypes[i].DefaultValue);
                }
            }

            var allParsedSuccessfully = parameterValues.All(p => p.Item1 == true);

            return (allParsedSuccessfully, parameterValues.Select(i => i.Item2).ToArray());
        }

    }
}
