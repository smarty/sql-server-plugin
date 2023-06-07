namespace SmartySqlServerPlugin.Interop
{
    using System;
    using SmartyStreets;

    internal static class ExtensionMethods
    {
        public static int ParseStatus(this Exception e)
        {
            switch (e)
            {
                case BadRequestException _:
                    return Contracts.StatusBadRequest;
                case ForbiddenException _:
                    return Contracts.StatusForbidden;
                case GatewayTimeoutException _:
                    return Contracts.StatusGatewayTimeout;
                case PaymentRequiredException _:
                    return Contracts.StatusPaymentRequired;
                case RequestEntityTooLargeException _:
                    return Contracts.StatusRequestEntityTooLarge;
                case UnprocessableEntityException _:
                    return Contracts.StatusUnprocessableEntity;
                case BadCredentialsException _:
                    return Contracts.StatusUnauthenticated;
                case TooManyRequestsException _:
                    return Contracts.StatusTooManyRequests;
                case ServiceUnavailableException _:
                    return Contracts.StatusServiceUnavailable;
                case InternalServerErrorException _:
                    return Contracts.StatusInternalServerError;
                default:
                    return 0;
            }
        }
    }
}