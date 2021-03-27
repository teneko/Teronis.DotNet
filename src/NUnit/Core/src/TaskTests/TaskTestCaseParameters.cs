// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using NUnit.Framework.Interfaces;
using NUnit.Framework.Internal;

namespace Teronis.NUnit.TaskTests
{
    /// <summary>
    /// This is the derived class of <see cref="TestCaseParameters"/>.
    /// </summary>
    public sealed class TaskTestCaseParameters : TestCaseParameters
    {
        internal TaskTestCaseParameters(ITestCaseData data) 
            : base(data) { }
    }
}
