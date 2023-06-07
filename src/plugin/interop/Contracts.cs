namespace SmartySqlServerPlugin.Interop
{
    public static class Contracts
    {
        public const int StatusOkay = 200;
        public const int StatusBadRequest = 400;
        public const int StatusUnauthenticated = 401;
        public const int StatusPaymentRequired = 402;
        public const int StatusForbidden = 403;
        public const int StatusRequestEntityTooLarge = 413;
        public const int StatusUnprocessableEntity = 422;
        public const int StatusTooManyRequests = 429;
        public const int StatusInternalServerError = 500;
        public const int StatusServiceUnavailable = 503;
        public const int StatusGatewayTimeout = 504;
    }
}