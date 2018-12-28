using System;
using System.Collections.Generic;
using System.Text;

namespace Clear.Arguments
{
    public class ArgumentCollection : List<IArgument>
    {
        public string[] ToStringArray()
        {
            var returnVal = new List<string>();

            foreach (var arg in this)
            {
                switch(arg)
                {
                    case AnonymousArgument anon:
                        returnVal.Add(ArgumentParser.Escape(anon.Value));
                        break;
                    case NamedArgument named:
                        returnVal.Add($"--{named.Name}");

                        if (named.Value != null)
                        {
                            returnVal.Add(ArgumentParser.Escape(named.Value));
                        }

                        break;
                }
            }

            return returnVal.ToArray();
        }

        public override string ToString()
        {
            var returnVal = new StringBuilder();

            foreach (var arg in this)
            {
                switch (arg)
                {
                    case AnonymousArgument anon:
                        returnVal.Append($"{ArgumentParser.Escape(anon.Value)} ");
                        break;
                    case NamedArgument named:
                        returnVal.Append($"--{named.Name} ");

                        if (named.Value != null)
                        {
                            returnVal.Append($"{ArgumentParser.Escape(named.Value)} ");
                        }
                        break;
                }
            }

            returnVal.Remove(returnVal.Length - 1, 1);
            return returnVal.ToString();
        }
    }
}
