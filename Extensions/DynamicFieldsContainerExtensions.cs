using SitefinityWebApp.Services.Extensions;
using System;
using Telerik.Sitefinity.Model;

namespace SitefinityApiWrapper.Extensions
{
    public static class DynamicFieldsContainerExtensions
    {
        public static string GetStringValueOrDefault(this IDynamicFieldsContainer content, string fieldName, string defaultValue)
        {
            return content.DoesFieldExist(fieldName) ? content.GetValue<string>(fieldName).NullToString() : defaultValue;
        }
        public static string GetLstringValueOrDefault(this IDynamicFieldsContainer content, string fieldName, string defaultValue)
        {
            return content.DoesFieldExist(fieldName) ? content.GetValue<Lstring>(fieldName).NullToString() : defaultValue;
        }
        public static void SetStringValueSafe(this IDynamicFieldsContainer dataItem, string fieldName, string value)
        {
            if (dataItem.DoesFieldExist(fieldName)) dataItem.SetValue(fieldName, value);
        }

        public static bool GetBoolValueOrDefault(this IDynamicFieldsContainer content, string fieldName, bool defaultValue = false)
        {
            return content.DoesFieldExist(fieldName) ? content.GetValue<bool>(fieldName) : defaultValue;
        }

        public static decimal GetDecimalValueOrDefault(this IDynamicFieldsContainer content, string fieldName, decimal defaultValue)
        {
            return content.DoesFieldExist(fieldName) ? content.GetValue<decimal?>(fieldName).GetValueOrDefault(defaultValue) : defaultValue;
        }

        public static int GetIntValueOrDefault(this IDynamicFieldsContainer content, string fieldName, int defaultValue)
        {
            return content.DoesFieldExist(fieldName) ? (int)content.GetValue<decimal?>(fieldName).GetValueOrDefault(defaultValue) : defaultValue;
        }

        public static DateTime? GetDateTimeNullableValueOrDefault(this IDynamicFieldsContainer content, string fieldName, DateTime? defaultValue)
        {
            return content.DoesFieldExist(fieldName) ? content.GetValue<DateTime?>(fieldName) : defaultValue;
        }

        public static DateTime GetDateTimeValueOrDefault(this IDynamicFieldsContainer content, string fieldName, DateTime defaultValue)
        {
            DateTime? fieldValue = null;
            if (content.DoesFieldExist(fieldName))
                fieldValue = content.GetValue<DateTime?>(fieldName);

            if (fieldValue.IsNull() || !fieldValue.HasValue)
                fieldValue = defaultValue;

            return fieldValue.Value;
        }
    }
}
