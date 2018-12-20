using System;
using System.Globalization;

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

            try
            {
                return (true, Convert.ChangeType(value, type, CultureInfo.InvariantCulture));
            }
            catch
            {
                return (false, null);
            }
        }
    }
}
