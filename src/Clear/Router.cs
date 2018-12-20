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
            var methodFinder = new OverloadMethodFinder(type);

            var command = args.Length > 0 ? args[0] : null;

            if (command != null)
            {
                var (method, parameters) = methodFinder.TryFindMethodToInvoke(command, args.Skip(1).ToArray());
                if (method != null)
                {
                    var returnValue = method.Invoke(null, parameters);
                    return returnValue;
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
    }
}
