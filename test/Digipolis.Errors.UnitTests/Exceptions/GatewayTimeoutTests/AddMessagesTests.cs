﻿using System;
using System.Linq;
using Digipolis.Errors.Exceptions;
using Digipolis.Errors.Internal;
using Digipolis.Errors.UnitTests.Exceptions.BaseExceptionTests;
using Xunit;

namespace Digipolis.Errors.UnitTests.Exceptions.GatewayTimeoutTests
{
    public class AddMessagesTests
    {
        [Fact]
        private void MessagesAreAddedToMessagesCollection()
        {
            var message1 = "message1";
            var message2 = "messages2";
            var messages = new string[] { message1, message2 };

            var error = new GatewayTimeoutException();
            error.AddMessages(messages);

            var message = error.Messages.First();

            Assert.Single(error.Messages);

            Assert.Contains(message1, message.Value);
            Assert.Contains(message2, message.Value);
            Assert.Equal(message.Key, Defaults.ErrorMessage.Key);
        }

        [Fact]
        private void MessagesAreAddedToMessagesCollectionUnderSameKey()
        {
            var message1 = "message1";
            var message2 = "messages2";
            var messages1 = new string[] { message1 };
            var messages2 = new string[] { message2 };

            var ex = new GatewayTimeoutException();
            ex.AddMessages(messages1);
            ex.AddMessages(messages2);

            Assert.Single(ex.Messages);

            var values = ex.Messages.SelectMany(m => m.Value);
            var keys = ex.Messages.Select(m => m.Key);

            Assert.Contains(message1, values);
            Assert.Contains(message2, values);
            Assert.All(keys, k => Assert.Equal(Defaults.ErrorMessage.Key, k));
        }

        [Fact]
        private void MessagesAreAddedToMessagesCollectionUnderDifferentKey()
        {
            var message1 = "message1";
            var message2 = "messages2";
            var messages1 = new string[] { message1 };
            var messages2 = new string[] { message2 };

            var ex = new GatewayTimeoutException();
            ex.AddMessages("key1", messages1);
            ex.AddMessages("key2", messages2);

            Assert.Equal(2, ex.Messages.Count);

            Assert.True(ex.Messages.ContainsKey("key1"));
            Assert.True(ex.Messages.ContainsKey("key2"));
            Assert.Contains(message1, ex.Messages["key1"]);
            Assert.Contains(message2, ex.Messages["key2"]);
        }

        [Fact]
        private void NullMessagesCollectionWithKeyIsNotAddedToMessagesCollection()
        {
            var ex = new GatewayTimeoutException();
            ex.AddMessages("Key1", null);
            Assert.Empty(ex.Messages);
        }

        [Fact]
        private void EmptyMessagesCollectionWithKeyIsNotAddedToMessagesCollection()
        {
            var ex = new GatewayTimeoutException();
            ex.AddMessages("Key1", new string[0]);
            Assert.Empty(ex.Messages);
        }

        [Fact]
        private void NullMessagesCollectionIsNotAddedToMessagesCollection()
        {
            var ex = new GatewayTimeoutException();
            ex.AddMessages(null);
            Assert.Empty(ex.Messages);
        }

        [Fact]
        private void EmptyMessagesCollectionIsNotAddedToMessagesCollection()
        {
            var ex = new GatewayTimeoutException();
            ex.AddMessages(new string[0]);
            Assert.Empty(ex.Messages);
        }

        [Fact]
        private void NullKeyMessagesCollectionThrowsException()
        {
            var message1 = "message1";
            var message2 = "messages2";
            var messages = new string[] { message1, message2 };

            var ex = new GatewayTimeoutException();
            var result = Assert.Throws<ArgumentNullException>(() => ex.AddMessages(null, messages));
            Assert.Equal("key", result.ParamName);
        }

        [Fact]
        private void NonDefaultEmptyKeyMessagesCollectionThrowsException()
        {
            var message1 = "message1";
            var message2 = "messages2";
            var messages = new string[] { message1, message2 };

            var ex = new GatewayTimeoutException();
            var result = Assert.Throws<ArgumentNullException>(() => ex.AddMessages(" ", messages));
            Assert.Equal("key", result.ParamName);
        }

        [Fact]
        private void DefaultEmptyKeyMessagesCollectionAddedToCollection()
        {
            var message1 = "message1";
            var message2 = "messages2";
            var messages = new string[] { message1, message2 };

            var ex = new ExceptionTester();
            ex.AddMessages(Defaults.ErrorMessage.Key, messages);

            Assert.Single(ex.Messages);
            Assert.Contains(message1, ex.Messages.First().Value);
            Assert.Contains(message2, ex.Messages.First().Value);
        }
    }
}
