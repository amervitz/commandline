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

            try
            {
                return ChangeNonNullableType(value, underlyingType ?? type);
            }
            catch
            {
                return (false, null);
            }
        }

        private (bool, object) ChangeNonNullableType(string value, Type type)
        {
            // change types that implement IConvertible
            if (type.GetInterface(nameof(IConvertible)) != null)
            {
                return (true, Convert.ChangeType(value, type, CultureInfo.InvariantCulture));
            }

            // change types that implement a constructor with a single string parameter
            var constructor = type.GetConstructor(new[] { typeof(string) });
            if (constructor != null)
            {
                return (true, constructor.Invoke(new[] { value }));
            }

            // changing the type is not supported
            return (false, null);
        }
    }
}
