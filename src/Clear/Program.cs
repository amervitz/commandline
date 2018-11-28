using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

namespace Clear
{
    class Program
    {
        static void Main(string[] args)
        {
            var output = Run(typeof(TopLevel), args);
        }
        
        private static object Run(Type type, string[] args)
        {
            var methods = type.GetMethods(BindingFlags.Static | BindingFlags.Public);

            if(args.Length > 0 && TryGetMethodToExecute(args[0], methods, out var methodToExecute))
            {
                Console.WriteLine($"Matched on {methodToExecute.Name}");
                var ps = methodToExecute.GetParameters();

                object[] parametersToSupply = GetParameterValues(ps, args.Skip(1).ToArray());

                var returnVal = methodToExecute.Invoke(null, parametersToSupply);
                return (methodToExecute, returnVal);
            }
            else
            {
                Console.WriteLine("Methods:");
                foreach (var m in methods)
                {
                    Console.Write("\t");
                    Console.Write(m.Name + "(");
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

                    var da = m.GetCustomAttribute<DisplayAttribute>();
                    if (da != null)
                    {
                        Console.WriteLine($"\t\t{da.GetShortName()} - {da.GetName()} - {da.GetDescription()} - {da.GetPrompt()}");
                    }
                }
            }

            return null;
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

            return paramsToUse.ToArray();
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
