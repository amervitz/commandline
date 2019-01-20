using System.ComponentModel.DataAnnotations;

namespace amervitz.commandline.Tests.Commands
{
    public static class Annotations
    {
        public static string ParameterNameOverride([Display(Name = "p1")]string param1, [Display(Name = "p2")]string param2)
        {
            return $"{param1}{param2}";
        }

        public static string ParameterShortNameOverride([Display(ShortName = "p")]string param1, [Display(ShortName = "q")]string param2)
        {
            return $"{param1}{param2}";
        }

        public static string ParameterShortAndLongNameOverride([Display(ShortName = "p", Name = "par1")]string param1, [Display(ShortName = "q", Name = "par2")]string param2)
        {
            return $"{param1}{param2}";
        }

        [Display(Name = "OverriddenMethodName")]
        public static string MethodNameOverridden(string param1)
        {
            return $"{param1}";
        }
    }
}
