﻿namespace Digipolis.Errors.Internal
{
    internal class Defaults
    {
        internal class ErrorMessage
        {
            internal const string Key = "";
            internal const string NullString = "null";
        }

        internal class BaseException
        {
        }

        internal class NotFoundException
        {
            internal const string Title = "Not found.";
            internal const string Code = "NFOUND001";
        }

        internal class UnauthorizedException
        {
            internal const string Title = "Access denied.";
            internal const string Code = "UNAUTH001";
        }

        internal class ForbiddenException
        {
            internal const string Title = "Forbidden.";
            internal const string Code = "FORBID001";
        }

        internal class TooManyRequestsException
        {
            internal const string Title = "Too many requests.";
            internal const string Code = "TMREQU001";
        }

        internal class ValidationException
        {
            internal const string Title = "Bad request.";
            internal const string Code = "UNVALI001";
        }

        internal class BadGatewayException
        {
            internal const string Title = "Bad Gateway.";
            internal const string Code = "GTWAY001";
        }
        internal class GatewayTimeoutException
        {
            internal const string Title = "Gateway Timeout.";
            internal const string Code = "GTWAY002";
        }
    }
}
