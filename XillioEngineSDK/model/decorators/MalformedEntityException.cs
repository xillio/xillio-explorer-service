using System;

namespace XillioEngineSDK.model.decorators
{
    public class MalformedEntityException : ArgumentException
    {
        public MalformedEntityException(string message) : base(message)
        {
        }

        public MalformedEntityException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public MalformedEntityException(string message, string paramName) : base(message, paramName)
        {
        }

        public MalformedEntityException(string message, string paramName, Exception innerException) : base(message, paramName, innerException)
        {
        }
    }
}