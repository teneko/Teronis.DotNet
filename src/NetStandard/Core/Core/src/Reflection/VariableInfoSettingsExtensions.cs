// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Teronis.Reflection;

namespace Teronis.Extensions
{
    public static class VariableInfoSettingsExtensions
    {
        internal static VariableMemberDescriptor DefaultIfNull(this VariableMemberDescriptor? descriptor, bool seal)
        {
            descriptor ??= new VariableMemberDescriptor();

            if (seal) {
                descriptor.Seal();
            }

            return descriptor;
        }
    }
}
