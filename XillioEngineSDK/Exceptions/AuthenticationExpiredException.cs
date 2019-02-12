using System;

namespace XillioEngineSDK
{
    public class AuthenticationExpiredException : Exception
    {
        public AuthenticationExpiredException(string message) : base(message)
        {
        }

        public AuthenticationExpiredException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}