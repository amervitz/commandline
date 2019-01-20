using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace amervitz.commandline
{
    public class Method
    {
        public readonly string Name;
        public readonly MethodInfo MethodInfo;

        public Method(MethodInfo methodInfo)
        {
            MethodInfo = methodInfo;
            Name = methodInfo.Name;

            var displayAttribute = methodInfo.GetCustomAttribute<DisplayAttribute>();
            if (displayAttribute != null)
            {
                if (displayAttribute.Name != null)
                {
                    Name = displayAttribute.Name;
                }
            }
        }
    }
}