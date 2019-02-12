using System;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace XillioEngineSDK.model.decorators
{
    public class DecoratorList
    {
        [JsonProperty("container")]
        public ContainerDecorator ContainerDecorator { get; set; }

        [JsonProperty("name")]
        public NameDecorator NameDecorator { get; set; }

        [JsonProperty("file")]
        public FileDecorator FileDecorator { get; set; }
    }
}