using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace amervitz.commandline.Arguments
{
    public class ArgumentParser
    {
        public ArgumentCollection Parse(string[] args, bool removeDuplicates = true)
        {
            var argsQueue = new Queue<string>(args);
            var parsedArguments = new Dictionary<string, bool>();
            var arguments = new ArgumentCollection();

            while(argsQueue.Count > 0)
            {
                // get the next argument
                var currentArg = argsQueue.Dequeue();

                // test if it's a named parameter
                if (IsNamedArgument(currentArg))
                {
                    var argument = new NamedArgument(currentArg);

                    // check if the next argument is a value
                    if (argsQueue.Count > 0 && !IsNamedArgument(argsQueue.Peek()))
                    {
                        var nextArg = argsQueue.Dequeue();
                        var unescaped = Unescape(nextArg);

                        argument.Value = unescaped;
                    }

                    if (!removeDuplicates)
                    {
                        arguments.Add(argument);
                    }
                    else if(!parsedArguments.ContainsKey(argument.Name))
                    {
                        arguments.Add(argument);
                        parsedArguments.Add(argument.Name, true);
                    }
                }
                else
                {
                    var parameter = new AnonymousArgument
                    {
                        Value = Unescape(currentArg)
                    };

                    arguments.Add(parameter);
                }
            }

            return arguments;
        }

        public static bool IsNamedArgument(string arg)
        {
            var isLongName = arg.StartsWith("--") && arg.Length > 3 && arg[2] != '-';

            if (isLongName)
            {
                return true;
            }

            var isShortName = arg.StartsWith("-") && arg.Length == 2 && arg[1] != '-';
            if (isShortName)
            {
                return true;
            }

            return false;
        }

        public static string Unescape(string arg)
        {
            if (arg == null)
            {
                return null;
            }

            var argArray = arg.ToCharArray();
            var numFound = 0;
            foreach (var schar in argArray)
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

            var remainingChars = arg.Substring(numFound);

            // unescape what would normally be a shortname (single dash and single character)
            if (numFound == 2 && remainingChars.Length == 1)
            {
                return $"-{remainingChars}";
            }

            // unescape what would normally be a longname (two dashes and atleast 1 character)
            if (numFound == 4 && remainingChars.Length > 1)
            {
                return $"--{remainingChars}";
            }

            // no escaping necessary
            return arg;

        }

        public static string Escape(string arg)
        {
            if (arg == null)
            {
                return null;
            }

            var argArray = arg.ToCharArray();
            var numFound = 0;
            foreach (var schar in argArray)
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

            var remainingChars = arg.Substring(numFound);

            // escape what would normally be a shortname (single dash and single character)
            if(numFound == 1 && remainingChars.Length == 1)
            {
                return $"-{arg}";
            }

            // escape what would normally be a longname (two dashes and atleast 1 character)
            if(numFound == 2 && remainingChars.Length > 1)
            {
                return $"--{arg}";
            }

            // no escaping necessary
            return arg;
        }
    }
}
