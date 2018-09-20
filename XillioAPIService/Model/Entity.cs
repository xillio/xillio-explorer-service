using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace XillioAPIService.Model
{
    public class Entity
    {
        public string ID { get; set; }
        public string Kind { get; set; }
        public XDIP Xdip { get; set; }
        
        public List<JObject> Original { get; set; }
        public List<JObject> Modified { get; set; }

        public Entity()
        {
        }
    }

    public class XDIP
    {
    }
}
