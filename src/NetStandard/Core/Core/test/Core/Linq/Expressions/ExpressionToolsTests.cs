// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Linq.Expressions;
using Xunit;

namespace Teronis.Linq.Expressions
{
    public class ExpressionToolsTests
    {
        [Fact]
        public void Should_get_anonymous_type_names_from_generic_func()
        {
            var ball = new Ball();
            Expression<Func<Ball, object>> action = ball => new { ball.Height };
            Assert.Equal(ExpressionGenericTools.GetAnonymousTypeNames(action), new string[] { nameof(ball.Height) });
        }

        [Fact]
        public void Should_get_anonymous_type_names_from_func()
        {
            var ball = new Ball();
            Expression<Func<object>> action = () => new { ball.Height };
            Assert.Equal(ExpressionGenericTools.GetAnonymousTypeNames(action), new string[] { nameof(ball.Height) });
        }

        private class Ball
        {
            public int Height { get; set; }
        }
    }
}
