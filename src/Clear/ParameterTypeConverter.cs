using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Clear
{
    public class ParameterTypeConverter
    {
        public (bool, object) TryChangeValue(string value, Type type)
        {
            var underlyingType = Nullable.GetUnderlyingType(type);

            // handle null on a nullable type
            if (underlyingType != null && string.Equals(value, "null", StringComparison.InvariantCultureIgnoreCase))
            {
                return (true, null);
            }

            return TryChangeNonNullableType(value, underlyingType ?? type);
        }

        private (bool, object) TryChangeNonNullableType(string value, Type type)
        {
            if (type == typeof(Guid))
            {
                return (Guid.TryParse(value, out Guid result), result);
            }
            else
            {
                try
                {
                    return (true, Convert.ChangeType(value, type));
                }
                catch
                {
                    return (false, null);
                }
            }
        }
    }
}
