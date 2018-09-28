using System;
using System.IO;

namespace XillioEngineSDK
{
    public class NotAuthenticatedException : Exception
    {
        public NotAuthenticatedException(string message) : base(message)
        {
        }

        public NotAuthenticatedException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}