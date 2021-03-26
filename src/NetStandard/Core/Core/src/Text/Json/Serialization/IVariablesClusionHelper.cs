// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Teronis.Text.Json.Serialization
{
    public interface IVariablesClusionHelper
    {
        void ConsiderVariable(Type declaringType, params string[] propertyName);
    }
}
