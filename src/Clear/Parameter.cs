using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace Clear
{
    public class Parameter
    {
        public readonly string ShortName;

        public readonly string Name;

        public readonly ParameterInfo ParameterInfo;

        public Parameter(ParameterInfo parameterInfo)
        {
            ParameterInfo = parameterInfo;

            var displayAttribute = parameterInfo.GetCustomAttribute<DisplayAttribute>();
            Name = parameterInfo.Name;

            if (displayAttribute != null)
            {
                if (displayAttribute.Name != null)
                {
                    Name = displayAttribute.Name;
                }

                if (displayAttribute.ShortName != null)
                {
                    ShortName = displayAttribute.ShortName;
                }
            }
        }
    }
}