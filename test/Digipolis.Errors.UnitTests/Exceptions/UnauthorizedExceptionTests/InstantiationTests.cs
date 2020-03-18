﻿using System;
using System.Collections.Generic;
using System.Linq;
using Digipolis.Errors.Exceptions;
using Digipolis.Errors.Internal;
using Xunit;

namespace Digipolis.Errors.UnitTests.Exceptions.UnauthorizedExceptionTests
{
    public class InstantiationTests
    {
        [Fact]
        private void PropertiesAreDefaulted()
        {
            var ex = new UnauthorizedException();
            Assert.Equal(Defaults.UnauthorizedException.Title, ex.Message);
            Assert.NotNull(ex.Messages);
        }

        [Fact]
        private void MessageIsSet()
        {
            var ex = new UnauthorizedException("access denied");
            Assert.Equal("access denied", ex.Message);
            Assert.NotNull(ex.Messages);
        }

        [Fact]
        private void CodeIsSet()
        {
            string code = "NOACC1";

            var ex = new UnauthorizedException("access denied", code);

            Assert.Equal(code, ex.Code);
        }

        [Fact]
        private void MessageAndInnerExceptionAreSetInProperties()
        {
            var innerEx = new Exception("innerMessage");
            var ex = new UnauthorizedException("access denied", "NOACC1", innerEx);
            Assert.Equal("access denied", ex.Message);
            Assert.Same(innerEx, ex.InnerException);
        }

        [Fact]
        private void MessageAndInnerExceptionAndExtraInfoAreSetInProperties()
        {
            var messages = new Dictionary<string, IEnumerable<string>>();
            var message = new[] { "message1", "message2" };
            messages.Add("key1", message);
            var innerEx = new Exception("innerMessage");

            var ex = new UnauthorizedException("access denied", "NOACC1", innerEx, messages);
            Assert.Equal("access denied", ex.Message);
            Assert.Same(innerEx, ex.InnerException);
            Assert.Same(messages, ex.Messages);
        }
    }
}
