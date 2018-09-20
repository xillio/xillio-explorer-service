using System.Collections.Generic;

namespace XillioEngineSDK.model
{
    public class Configuration
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string ConfigurationType { get; set; }
        public bool PassthroughAuthorization { get; set; }
        public Dictionary<string, object> Config { get; set; }
    }
}