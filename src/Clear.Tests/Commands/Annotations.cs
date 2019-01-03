using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clear.Tests.Commands
{
    public static class Annotations
    {
        public static string ParameterNameOverride([Display(Name = "p1")]int param1, [Display(Name = "p2")]int param2)
        {
            return $"{param1}{param2}";
        }

        public static string ParameterShortNameOverride([Display(ShortName = "p")]int param1, [Display(ShortName = "q")]int param2)
        {
            return $"{param1}{param2}";
        }

        public static string ParameterShortAndLongNameOverride([Display(ShortName = "p", Name = "par1")]int param1, [Display(ShortName = "q", Name = "par2")]int param2)
        {
            return $"{param1}{param2}";
        }
    }
}
