using System;

namespace XillioEngineSDK.model.decorators
{
    public class ModifiedDecorator : Decorator
    {
        public DateTime Date { get; set; }
        public SystemUser By { get; set; }
    }
}