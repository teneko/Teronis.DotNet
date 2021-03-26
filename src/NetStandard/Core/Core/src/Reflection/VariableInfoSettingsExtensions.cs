// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Teronis.Reflection;

namespace Teronis.Extensions
{
    public static class VariableInfoSettingsExtensions
    {
        internal static VariableInfoDescriptor DefaultIfNull(this VariableInfoDescriptor? descriptor, bool seal)
        {
            descriptor ??= new VariableInfoDescriptor();

            if (seal) {
                descriptor.Seal();
            }

            return descriptor;
        }
    }
}
