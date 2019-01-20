using System;
using System.Collections.Generic;
using System.Text;

namespace amervitz.commandline.Arguments
{
    public enum NamedArgumentFormat
    {
        Short,
        Long
    }

    public class NamedArgument : IArgument
    {
        public readonly string Name;

        public readonly NamedArgumentFormat Format;

        public string Value { get; set; }

        public string OriginalName { get; set; }

        public NamedArgument(string name)
        {
            var isLongName = name.StartsWith("--") && name.Length > 2 && name[2] != '-';
            var isShortName = name.StartsWith("-") && name.Length == 2 && name[1] != '-';

            if (isLongName)
            {
                Name = name.Substring(2);
                Format = NamedArgumentFormat.Long;
            }
            else if (isShortName)
            {
                Name = name.Substring(1);
                Format = NamedArgumentFormat.Short;
            }
            else
            {
                Name = name;
                Format = NamedArgumentFormat.Long;
            }
        }
    }
}
