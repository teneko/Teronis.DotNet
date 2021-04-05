// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Reflection;

namespace Teronis.Reflection
{
    public sealed class VariableMemberDescriptor
    {
        /// <summary>
        /// A combination of <see cref="BindingFlags.Instance"/> and <see cref="BindingFlags.Public"/>.
        /// </summary>
        public readonly static BindingFlags DefaultFlags = BindingFlags.Instance | BindingFlags.Public;

        /// <summary>
        /// Has the value of <see cref="DefaultFlags"/> by default.
        /// </summary>
        public BindingFlags Flags {
            get => flags;

            set {
                throwExceptionIfSealed();
                flags = value;
            }
        }
        public bool ExcludeIfReadable {
            get => excludeIfReadable;

            set {
                throwExceptionIfSealed();
                excludeIfReadable = value;
            }
        }

        public bool ExcludeIfWritable {
            get => excludeIfWritable;

            set {
                throwExceptionIfSealed();
                excludeIfWritable = value;
            }
        }

        public bool IncludeIfReadable {
            get => includeIfReadable;

            set {
                throwExceptionIfSealed();
                includeIfReadable = value;
            }
        }

        public bool IncludeIfWritable {
            get => includeIfWritable;

            set {
                throwExceptionIfSealed();
                includeIfWritable = value;
            }
        }

        public IEnumerable<Type>? ExcludeByAttributeTypes {
            get => excludeByAttributeTypes;

            set {
                throwExceptionIfSealed();
                excludeByAttributeTypes = value;
            }
        }

        public bool ExcludeByAttributeTypesInherit {
            get => excludeByAttributeTypesInherit;

            set {
                throwExceptionIfSealed();
                excludeByAttributeTypesInherit = value;
            }
        }

        public IEnumerable<Type>? IncludeByAttributeTypes {
            get => includeByAttributeTypes;

            set {
                throwExceptionIfSealed();
                includeByAttributeTypes = value;
            }
        }

        public bool IncludeByAttributeTypesInherit {
            get => includeByAttributeTypesInherit;

            set {
                throwExceptionIfSealed();
                includeByAttributeTypesInherit = value;
            }
        }

        public bool IsSealed { get; private set; }

        private BindingFlags flags;
        private bool excludeIfReadable;
        private bool excludeIfWritable;
        private bool includeIfReadable;
        private bool includeIfWritable;
        private IEnumerable<Type>? excludeByAttributeTypes;
        private bool excludeByAttributeTypesInherit;
        private IEnumerable<Type>? includeByAttributeTypes;
        private bool includeByAttributeTypesInherit;

        public VariableMemberDescriptor()
        {
            Flags = DefaultFlags;
            ExcludeByAttributeTypesInherit = true; // Library.DefaultCustomAttributesInherit
            IncludeByAttributeTypesInherit = true; // Library.DefaultCustomAttributesInherit
        }

        /// <summary>
        /// A shallow copy of 
        /// </summary>
        /// <returns></returns>
        public VariableMemberDescriptor ShallowCopy()
        {
            var properties = GetType().GetProperties(DefaultFlags);
            var settings = new VariableMemberDescriptor();

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
            if (IsSealed) {
                throw new Exception("This instance has been sealed is not intented to be changed any further.");
            }
        }

        public void Seal() => IsSealed = true;
    }
}
