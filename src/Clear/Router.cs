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
            var methods = type.GetMethods(BindingFlags.Static | BindingFlags.Public);

            if(args.Length > 0 && TryGetMethodToExecute(args[0], methods, out var methodToExecute))
            {
                var ps = methodToExecute.GetParameters();

                object[] parametersToSupply = GetParameterValues(ps, args.Skip(1).ToArray());

                if(parametersToSupply.Length == ps.Length)
                {
                    var returnVal = methodToExecute.Invoke(null, parametersToSupply);
                    return (methodToExecute, returnVal);
                }
            }

            ShowMethods(methods);

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

        private static void ShowMethods(MethodInfo[] methods)
        {
            foreach (var m in methods)
            {
                Console.Write(m.Name + " (");
                var ps = m.GetParameters();

                for (int i = 0; i < ps.Length; i++)
                {
                    Console.Write(ps[i].ParameterType.Name);
                    Console.Write(" ");
                    Console.Write(ps[i].Name);

                    if (i < (ps.Length - 1))
                    {
                        Console.Write(", ");
                    }
                }
                Console.WriteLine(")");

                //var da = m.GetCustomAttribute<DisplayAttribute>();
                //if (da != null)
                //{
                //    Console.WriteLine($"\t\t{da.GetShortName()} - {da.GetName()} - {da.GetDescription()} - {da.GetPrompt()}");
                //}
            }
        }

        private static object[] GetParameterValues(ParameterInfo[] ps, string[] args)
        {
            var paramsToUse = new object[ps.Length];

            if (args.Length == 0)
            {
                // loop through the arguments and assign default values
                for (int i = 0; i < ps.Length; i++)
                {
                    if (ps[i].HasDefaultValue)
                    {
                        paramsToUse[i] = ps[i].DefaultValue;
                    }
                }
            }

            for (int i = 0; i < args.Length; i++)
            {
                // get the parameter value
                var param = args[i];

                // check if it's a named argument
                var namedArgument = ps.FirstOrDefault(p => string.Equals($"--{p.Name}", param, StringComparison.InvariantCultureIgnoreCase));

                // if a named argument, add the next value as the parameter value
                if (namedArgument != null)
                {
                    var paramIndex = Array.IndexOf(ps, namedArgument);
                    if(args.Length >= i + 2)
                    {
                        paramsToUse[paramIndex] = GetObjectParameter(namedArgument, args[i+1]);
                        i += 1;
                    }
                    else // the last parameter and it has a default value
                    {
                        if (namedArgument.HasDefaultValue)
                        {
                            paramsToUse[i] = namedArgument.DefaultValue;
                        }
                    }
                }
                // try to use the current argument as the next parameter value 
                else
                {
                    // find the first null position
                    var emptyIndex = Array.IndexOf(paramsToUse, null);

                    // get the named argument for the position
                    var unnamedArgument = ps[emptyIndex];
                    paramsToUse[emptyIndex] = GetObjectParameter(unnamedArgument, param);
                }
            }

            return paramsToUse;
        }

        private static object GetObjectParameter(ParameterInfo parm, string value)
        {
            var ut = Nullable.GetUnderlyingType(parm.ParameterType);
            if (ut != null)
            {
                if (string.Equals(value, "null", StringComparison.InvariantCultureIgnoreCase))
                {
                    return null;
                }
                else
                {
                    return Convert.ChangeType(value, ut);
                }
            }
            else
            {
                return Convert.ChangeType(value, parm.ParameterType);
            }
        }

        private static bool TryGetMethodToExecute(string command, MethodInfo[] methods, out MethodInfo methodToExecute)
        {
            methodToExecute = methods.Where(m => string.Equals(m.Name, command, StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();
            return methodToExecute != null;
        }
    }
}
