using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Clear
{
    public static class Router
    {        
        public static object Run(Type type, string[] args)
        {
            var methodFinder = new SingleMethodFinder(type);

            var command = args.Length > 0 ? args[0] : null;

            if (command != null && methodFinder.TryFindMethodToExecute(command, out var methodToExecute))
            {
                var methodParameters = methodToExecute.GetParameters();

                var commandArguments = args.Skip(1).ToArray();

                var (success, parametersToSupply) = TryGetParameterValues(methodParameters, commandArguments);

                if(success && parametersToSupply.Length == methodParameters.Length)
                {
                    var returnVal = methodToExecute.Invoke(null, parametersToSupply);
                    return returnVal;
                }
            }

            var helpFormatter = new HelpFormatter();
            var helpText = helpFormatter.GetCommands(type);

            Console.Write(helpText);

            return null;
        }

        public static object Run(string @namespace, string[] args){

            var command = args.Length > 0 ? args[0] : null;

            var types = Assembly.GetEntryAssembly().GetTypes().Where(t => t.Namespace.StartsWith(@namespace, StringComparison.InvariantCultureIgnoreCase));

            if(command == null)
            {
                ShowCommands(@namespace, types);
                return null;
            }

            if(TryGetClassToExecute(@namespace, command, types, out var classToExecute))
            {
                return Run(classToExecute, args.Skip(1).ToArray());
            }

            return Run(@namespace + "." + command, args.Skip(1).ToArray());
        }

        private static void ShowCommands(string @namespace, IEnumerable<Type> types)
        {
            var typesWithoutNamespace = from t in types
                                        let ns = Regex.Replace(t.FullName, Regex.Escape(@namespace + "."), "", RegexOptions.IgnoreCase)
                                        select ns;

            var commands = from c in typesWithoutNamespace
                           let segments = c.Split('.')
                           select segments[0];

            var uniqueCommands = commands.Distinct();

            foreach (var command in uniqueCommands)
            {
                Console.WriteLine(command);
            }
        }

        private static void ShowClasses(IEnumerable<Type> types)
        {                           
            foreach(var type in types)
            {
                Console.WriteLine(type.Name);
            }
        }

        private static bool TryGetClassToExecute(string @namespace, string @class, IEnumerable<Type> classes, out Type classToExecute)
        {
            classToExecute = classes.FirstOrDefault(c => 
                c.Namespace.Equals(@namespace, StringComparison.InvariantCultureIgnoreCase) && 
                c.Name.Equals(@class, StringComparison.InvariantCultureIgnoreCase)
            );

            return classToExecute != null;
        }

        private static (bool, object[]) TryGetParameterValues(ParameterInfo[] parameterTypes, string[] args)
        {
            var parameterValues = new List<(bool?, object)>(parameterTypes.Length);
            parameterValues.AddRange(Enumerable.Repeat<(bool?, object)>((default, null), parameterTypes.Length));
            
            var parameterConverter = new ParameterTypeConverter();
            for (int i = 0; i < args.Length && parameterValues.FindIndex(pv => pv.Item1 == null) >= 0; i++)
            {
                // get the parameter value
                var arg = args[i];

                // check if it's a named argument
                var namedArgument = parameterTypes.FirstOrDefault(p => string.Equals($"--{p.Name}", arg, StringComparison.InvariantCultureIgnoreCase));

                // if a named argument, add the next value as the parameter value
                if (namedArgument != null)
                {
                    var paramIndex = Array.IndexOf(parameterTypes, namedArgument);
                    if(args.Length >= i + 2)
                    {
                        // only convert and assign the value if it hasn't already been assigned, e.g. by an unnamed argument
                        if (parameterValues[paramIndex].Item1 == null)
                        {
                            parameterValues[paramIndex] = parameterConverter.TryChangeValue(args[i + 1], namedArgument.ParameterType);
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
                    parameterValues[emptyIndex] = parameterConverter.TryChangeValue(arg, unnamedArgument.ParameterType);
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
