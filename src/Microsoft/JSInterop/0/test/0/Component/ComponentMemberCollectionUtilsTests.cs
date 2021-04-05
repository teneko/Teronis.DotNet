// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Xunit;

namespace Teronis.Microsoft.JSInterop.Component
{
    public class ComponentMemberCollectionUtilsTests
    {
        [Theory]
        [InlineData(typeof(DerivedFieldHavingClass))]
        public void Has_single_member(Type componentType)
        {
            var members = ComponentMemberCollectionUtils.GetComponentMembers(componentType).ToList();
            Assert.Single(members);
        }

        private class DerivedFieldHavingClass : PrivateFieldHavingClass
        { }

        public class PrivateFieldHavingClass
        {
            [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Test")]
            private string member { get; set; } = null!;
        }

        private class DerivedPropertyHavingClass : PrivatePropertyHavingClass
        { }

        public class PrivatePropertyHavingClass
        {
            [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Test")]
            private string member = null!;
        }
    }
}
