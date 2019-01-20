using System;
using System.Collections.Generic;
using System.Text;

namespace amervitz.commandline.Arguments
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

                        if (named.Format == NamedArgumentFormat.Long)
                        {
                            returnVal.Add($"--{named.Name}");
                        }
                        else
                        {
                            returnVal.Add($"-{named.Name}");
                        }

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

                        if (named.Format == NamedArgumentFormat.Long)
                        {
                            returnVal.Append($"--{named.Name} ");
                        }
                        else
                        {
                            returnVal.Append($"-{named.Name} ");
                        }

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
