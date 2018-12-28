using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Clear.Arguments
{
    public class ArgumentParser
    {
        public ArgumentCollection Parse(string[] args)
        {
            var argsQueue = new Queue<string>(args);
            var parsedArguments = new Dictionary<string, bool>();
            var parameters = new ArgumentCollection();

            while(argsQueue.Count > 0)
            {
                // get the next argument
                var currentArg = argsQueue.Dequeue();

                // test if it's a named parameter
                if (IsNamedArgument(currentArg))
                {
                    var parameter = new NamedArgument
                    {
                        Name = currentArg.Remove(0, 2),
                        OriginalName = currentArg,
                    };

                    // check if the next argument is a value
                    if (argsQueue.Count > 0 && !IsNamedArgument(argsQueue.Peek()))
                    {
                        var nextArg = argsQueue.Dequeue();
                        var unescaped = Unescape(nextArg);

                        parameter.Value = unescaped;
                    }

                    // only keep the parameter if it hasn't already been seen
                    if (!parsedArguments.ContainsKey(parameter.Name))
                    {
                        parameters.Add(parameter);
                        parsedArguments.Add(parameter.Name, true);
                    }
                }
                else
                {
                    var parameter = new AnonymousArgument
                    {
                        Value = Unescape(currentArg)
                    };

                    parameters.Add(parameter);
                }
            }

            return parameters;
        }

        public static bool IsNamedArgument(string arg)
        {
            return arg.StartsWith("--") && arg.Length > 2 && arg[2] != '-';
        }

        public static string Unescape(string arg)
        {
            if (arg == null)
            {
                return null;
            }

            var startChars = arg.ToCharArray();
            var numFound = 0;
            foreach (var schar in startChars)
            {
                if (schar == '-')
                {
                    numFound++;
                } else
                {
                    break;
                }
            }

            var substituationsToPerform = numFound / 4;
            if (substituationsToPerform > 0)
            {
                return arg.Remove(0, substituationsToPerform * 2);
            }
            else
            {
                return arg;
            }
        }

        public static string Escape(string arg)
        {
            if (arg == null)
            {
                return null;
            }

            var startChars = arg.ToCharArray();
            var numFound = 0;
            foreach (var schar in startChars)
            {
                if (schar == '-')
                {
                    numFound++;
                }
                else
                {
                    break;
                }
            }

            var substituationsToPerform = numFound / 2;
            if (substituationsToPerform > 0)
            {
                return new string('-', substituationsToPerform * 2) + arg;
            }
            else
            {
                return arg;
            }
        }
    }
}
