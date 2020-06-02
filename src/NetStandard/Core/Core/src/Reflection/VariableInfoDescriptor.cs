using System;
using System.Collections.Generic;
using System.Reflection;
using System.ComponentModel;

namespace Teronis.Reflection
{
    public sealed class VariableInfoDescriptor : INotifyPropertyChanging
    {
#pragma warning disable 0067
        public event PropertyChangingEventHandler PropertyChanging;
#pragma warning restore 0067

        /// <summary>
        /// A combination of <see cref="BindingFlags.Instance"/> and <see cref="BindingFlags.Public"/>.
        /// </summary>
        public readonly static BindingFlags DefaultFlags = BindingFlags.Instance | BindingFlags.Public;

        /// <summary>
        /// Has the value of <see cref="DefaultFlags"/> by default.
        /// </summary>
        public BindingFlags Flags { get; set; }
        public bool ExcludeIfReadable { get; set; }
        public bool ExcludeIfWritable { get; set; }
        public bool IncludeIfReadable { get; set; }
        public bool IncludeIfWritable { get; set; }
        public IEnumerable<Type> ExcludeByAttributeTypes { get; set; }
        public bool ExcludeByAttributeTypesInherit { get; set; }
        public IEnumerable<Type> IncludeByAttributeTypes { get; set; }
        public bool IncludeByAttributeTypesInherit { get; set; }
        public bool IsSealed { get; private set; }

        public VariableInfoDescriptor()
        {
            Flags = DefaultFlags;
            ExcludeByAttributeTypesInherit = true; // Library.DefaultCustomAttributesInherit
            IncludeByAttributeTypesInherit = true; // Library.DefaultCustomAttributesInherit
        }

        /// <summary>
        /// A shallow copy of 
        /// </summary>
        /// <returns></returns>
        public VariableInfoDescriptor ShallowCopy()
        {
            var properties = GetType().GetProperties(DefaultFlags);
            var settings = new VariableInfoDescriptor();

            foreach (var property in properties) {
                // We Want to exclude IsSealed consequently
                if (property.GetSetMethod() != null) {
                    var value = property.GetValue(this);
                    property.SetValue(settings, value);
                }
            }

            return settings;
        }

        private void throwExceptionIfSealed()
        {
            if (IsSealed)
                throw new Exception("This instance is already sealed");
        }

        private void OnPropertyChanging(string propertyName)
        {
            throwExceptionIfSealed();
            PropertyChanging?.Invoke(this, new PropertyChangingEventArgs(propertyName));
        }

        public void Seal() => IsSealed = true;
    }
}
