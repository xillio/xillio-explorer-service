using System;
using Flurl.Http.Configuration;

namespace XillioEngineSDK.model.decorators
{
    public class CreatedDecorator: Decorator
    {
        public DateTime Date { get; set; }
        public SystemUser By { get; set; }
    }
}