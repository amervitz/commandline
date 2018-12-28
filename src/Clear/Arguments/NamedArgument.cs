using System;
using System.Collections.Generic;
using System.Text;

namespace Clear.Arguments
{
    public class NamedArgument : IArgument
    {
        public string Name { get; set; }

        public string Value { get; set; }

        public string OriginalName { get; set; }
    }
}
