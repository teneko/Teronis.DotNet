// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Xunit;

namespace Teronis.Collections
{
    public class CollectionToolsTests
    {
        [Fact]
        public void Test_ascended_move_range()
        {
            var moveRange = CollectionTools.GetMoveRange(1, 2, 2);
            Assert.Equal(1, moveRange.LowerIndex);
            Assert.Equal(3, moveRange.Distance);
        }

        [Fact]
        public void Test_descended_move_range()
        {
            var moveRange = CollectionTools.GetMoveRange(2, 4, 1);
            Assert.Equal(2, moveRange.LowerIndex);
            Assert.Equal(3, moveRange.Distance);
        }
    }
}
