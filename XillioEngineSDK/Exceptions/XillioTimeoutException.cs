using System;
using System.Net.NetworkInformation;

namespace XillioEngineSDK
{
    public class XillioTimeoutException : PingException
    {
        public XillioTimeoutException(string message) : base(message)
        {
        }

        public XillioTimeoutException(string message, Exception innerException) : base(message, innerException)
        {
            
        }
    }
}