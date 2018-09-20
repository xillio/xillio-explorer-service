using System;
using System.Collections.Generic;
using System.Timers;
using Newtonsoft.Json;

namespace XillioAPIService.Model
{
    public class BaseConfiguration
    {

        [JsonProperty("id")]
        public string Id
        {
            get;
            set;
        }

        [JsonProperty("name")]
        public string Name
        {
            get;
            set;
        }

        [JsonProperty("configurationType")]
        public string ConfigurationType
        {
            get;
            set;
        }

        [JsonProperty("property")]
        public bool PassthroughAuthorization
        {
            get;
            set;
        }

        [JsonProperty("config")]
        public Dictionary<string, string> Config
        {
            get;
            set;
        }

        public Timer RefreshDelay
        {
            get;
            set;
        }
    }
}