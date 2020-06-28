using System.Configuration;
using System.Diagnostics.CodeAnalysis;

namespace Teronis.Extensions
{
    public static class SettingsPropertyValueExtensions
    {
        /// <summary>
        /// Create a deep copy of <paramref name="value"/>.
        /// </summary>
        /// <param name="value">
        /// Should be a deserialized value (ex. <see cref="SettingsPropertyValue.PropertyValue"/>) 
        /// or a serialized value (ex. <see cref="SettingsProperty.DefaultValue"/>).
        /// </param>
        /// <param name="isDeserialized">Indicates whether <paramref name="value"/> is deserialized or serialized.</param>
        public static object? CopyValue(this SettingsPropertyValue propertyValue, object? value, bool isDeserialized)
        {
            var cachedPropertyValue = propertyValue.PropertyValue;
            propertyValue.PropertyValue = value;

            if (isDeserialized)
                // We have to reassign, otherwise PropertyValue/SerializedValue of SettingsPropertyValue may be defaulted
                propertyValue.SerializedValue = propertyValue.SerializedValue;

            propertyValue.Deserialized = false;
            var valueCopy = propertyValue.PropertyValue;
            propertyValue.PropertyValue = cachedPropertyValue;
            return valueCopy;
        }

        [return: MaybeNull]
        public static PropertyType CopyValue<PropertyType>(this SettingsPropertyValue propertyValue, object value, bool isDeserialized)
            => (PropertyType)CopyValue(propertyValue, value, isDeserialized);

        public static object? CopyPropertyValue(this SettingsPropertyValue settingsPropertyValue) {
            var propertyValue = settingsPropertyValue.PropertyValue;
            return CopyValue(settingsPropertyValue, propertyValue, false);
        }

        [return: MaybeNull]
        public static PropertyType CopyPropertyValue<PropertyType>(this SettingsPropertyValue propertyValue)
            => (PropertyType)CopyPropertyValue(propertyValue);

        public static object? CopyDefaultValue(this SettingsPropertyValue settingsPropertyValue)
        {
            var defaultValue = settingsPropertyValue.Property.DefaultValue;
            return CopyValue(settingsPropertyValue, defaultValue, false);
        }

        [return: MaybeNull]
        public static PropertyType CopyDefaultValue<PropertyType>(this SettingsPropertyValue propertyValue)
            => (PropertyType)CopyDefaultValue(propertyValue);
    }
}
