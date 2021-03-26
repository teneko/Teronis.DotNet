// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Reflection;

namespace Teronis.Microsoft.JSInterop.Dynamic.Reflection
{
    public abstract class ParameterBase<AttributeType> : IParameterInfoReader
        where AttributeType : Attribute
    {
        public ParameterInfo ParameterInfo { get; }
        public AttributeType Attribute { get; }

        protected ParameterBase(ParameterInfo parameterInfo, AttributeType attribute)
        {
            ParameterInfo = parameterInfo ?? throw new ArgumentNullException(nameof(parameterInfo));
            Attribute = attribute ?? throw new ArgumentNullException(nameof(attribute));
        }

        protected internal virtual void ReadParameterInfo() { }

        void IParameterInfoReader.ReadParameterInfo() =>
            ReadParameterInfo();
    }
}
