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

        public static object Run(string @namespace, string[] args)
        {
            var command = args.Length > 0 ? args[0] : null;

            var types = Assembly.GetEntryAssembly().GetTypes().Where(t => t.Namespace.StartsWith(@namespace, StringComparison.InvariantCultureIgnoreCase));

            if (command == null)
            {
                var helpFormatter = new HelpFormatter();
                var helpText = helpFormatter.GetCommands(@namespace, types);

                Console.Write(helpText);

                return null;
            }

            if (TryGetClassToExecute(@namespace, command, types, out var classToExecute))
            {
                return Run(classToExecute, args.Skip(1).ToArray());
            }

            return Run(@namespace + "." + command, args.Skip(1).ToArray());
        }

        private static bool TryGetClassToExecute(string @namespace, string @class, IEnumerable<Type> classes, out Type classToExecute)
        {
            classToExecute = classes.FirstOrDefault(c =>
                c.Namespace.Equals(@namespace, StringComparison.InvariantCultureIgnoreCase) &&
                c.Name.Equals(@class, StringComparison.InvariantCultureIgnoreCase)
            );

            return classToExecute != null;
        }

        public static TResult Run<TResult>(Func<TResult> method, string[] args)
        {
            var methodInfo = method.GetMethodInfo();
            return RunMethod<TResult>(methodInfo, args);
        }

        public static void Run(Action method, string[] args)
        {
            var methodInfo = method.GetMethodInfo();
            RunMethod<object>(methodInfo, args);
        }

        public static TResult Run<T, TResult>(Func<T, TResult> method, string[] args)
        {
            var methodInfo = method.GetMethodInfo();
            return RunMethod<TResult>(methodInfo, args);
        }

        public static void Run<T>(Action<T> method, string[] args)
        {
            var methodInfo = method.GetMethodInfo();
            RunMethod<object>(methodInfo, args);
        }

        public static TResult Run<T1, T2, TResult>(Func<T1, T2, TResult> method, string[] args)
        {
            var methodInfo = method.GetMethodInfo();
            return RunMethod<TResult>(methodInfo, args);
        }

        public static void Run<T1, T2>(Action<T1, T2> method, string[] args)
        {
            var methodInfo = method.GetMethodInfo();
            RunMethod<object>(methodInfo, args);
        }

        public static TResult Run<T1, T2, T3, TResult>(Func<T1, T2, T3, TResult> method, string[] args)
        {
            var methodInfo = method.GetMethodInfo();
            return RunMethod<TResult>(methodInfo, args);
        }

        public static void Run<T1, T2, T3>(Action<T1, T2, T3> method, string[] args)
        {
            var methodInfo = method.GetMethodInfo();
            RunMethod<object>(methodInfo, args);
        }

        public static TResult Run<T1, T2, T3, T4, TResult>(Func<T1, T2, T3, T4, TResult> method, string[] args)
        {
            var methodInfo = method.GetMethodInfo();
            return RunMethod<TResult>(methodInfo, args);
        }

        public static void Run<T1, T2, T3, T4>(Action<T1, T2, T3, T4> method, string[] args)
        {
            var methodInfo = method.GetMethodInfo();
            RunMethod<object>(methodInfo, args);
        }

        public static TResult Run<T1, T2, T3, T4, T5, TResult>(Func<T1, T2, T3, T4, T5, TResult> method, string[] args)
        {
            var methodInfo = method.GetMethodInfo();
            return RunMethod<TResult>(methodInfo, args);
        }

        public static void Run<T1, T2, T3, T4, T5>(Action<T1, T2, T3, T4, T5> method, string[] args)
        {
            var methodInfo = method.GetMethodInfo();
            RunMethod<object>(methodInfo, args);
        }

        private static TResult RunMethod<TResult>(MethodInfo methodInfo, string[] args)
        {
            var methodParameters = methodInfo.GetParameters();
            var parameterMatcher = new ParameterMatcher();

            var (success, parameters) = parameterMatcher.TryGetParameterValues(methodParameters, args);

            if (success)
            {
                return (TResult)methodInfo.Invoke(null, parameters);
            }
            else
            {
                var helpFormatter = new HelpFormatter();
                var helpText = helpFormatter.GetArguments(methodInfo);
                Console.WriteLine(helpText);
            }

            return default;
        }
    }
}
