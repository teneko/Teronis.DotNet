// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using Xunit;

namespace Teronis.Extensions
{
    public class ExceptionExtensionsTests
    {
        public static readonly string NewLine = Environment.NewLine;

        [Theory]
        [ClassData(typeof(ExceptionGenerator))]
        public void Should_join_inner_messages(Exception exception, string resultMessage)
        {
            Assert.Equal(exception.JoinInnerMessages(NewLine), resultMessage);
        }

        public class ExceptionGenerator : TheoryData<Exception, string>
        {
            public ExceptionGenerator()
            {
                Add(new Exception(message: string.Empty,
                        new Exception("test",
                            new Exception(message: string.Empty,
                                new Exception("123")))),
                    "test" + Environment.NewLine + "123");
            }
        }
    }
}
