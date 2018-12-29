using Clear.Arguments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Clear
{
    public class ParameterMatcher
    {
        private readonly ArgumentParser argumentParser = new ArgumentParser();
        private readonly ParameterTypeConverter parameterTypeConverter = new ParameterTypeConverter();

        public (bool, object[]) TryGetParameterValues(ParameterInfo[] parameterTypes, string[] args)
        {
            var parameterValues = new List<(bool?, object)>(parameterTypes.Length);
            parameterValues.AddRange(Enumerable.Repeat<(bool?, object)>((default, null), parameterTypes.Length));

            var argsCollection = argumentParser.Parse(args);

            foreach (var arg in argsCollection)
            {
                if (arg is NamedArgument namedArgument)
                {
                    var parameter = parameterTypes.FirstOrDefault(p => string.Equals(p.Name, namedArgument.Name, StringComparison.InvariantCultureIgnoreCase));

                    if (parameter == null)
                    {
                        return (false, parameterValues.Select(param => param.Item2).ToArray());
                    }

                    var paramIndex = Array.IndexOf(parameterTypes, parameter);

                    // only assign the parameter value if it hasn't already been assigned by another argument
                    if (parameterValues[paramIndex].Item1 != null)
                    {
                        return (false, parameterValues.Select(param => param.Item2).ToArray());
                    }

                    parameterValues[paramIndex] = parameterTypeConverter.TryChangeValue(namedArgument.Value, parameter.ParameterType);
                }
                else if (arg is AnonymousArgument anonymousArgument)
                {
                    // find the first parameter without a value
                    var emptyIndex = parameterValues.FindIndex(pv => pv.Item1 == null);

                    // only assign the parameter value if an unassigned parameter is found
                    if (emptyIndex == -1)
                    {
                        return (false, parameterValues.Select(param => param.Item2).ToArray());
                    }

                    // get the parameter
                    var anonymousParameter = parameterTypes[emptyIndex];
                    parameterValues[emptyIndex] = parameterTypeConverter.TryChangeValue(anonymousArgument.Value, anonymousParameter.ParameterType);
                }
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
