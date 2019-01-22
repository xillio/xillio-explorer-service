using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace XillioEngineSDK.model.decorators
{
    public class DecoratorList : ISerializable
    {
        [JsonProperty("container")]
        public ContainerDecorator ContainerDecorator { get; set; }

        [JsonProperty("name")]
        public NameDecorator NameDecorator { get; set; }

        [JsonProperty("file")]
        public FileDecorator FileDecorator { get; set; }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("container", ContainerDecorator);
            info.AddValue("name", NameDecorator);
            info.AddValue("file", FileDecorator);
        }
    }
}