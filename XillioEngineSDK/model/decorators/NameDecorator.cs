using System.Runtime.Serialization;

namespace XillioEngineSDK.model.decorators
{
    public class NameDecorator : Decorator
    {
        public string SystemName { get; set; }
        public string DisplayName { get; set; }
        
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("systemName", SystemName);
            info.AddValue("displayName", DisplayName);
        }
    }
}