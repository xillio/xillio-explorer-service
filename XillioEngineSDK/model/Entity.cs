using System.Collections.Generic;

namespace XillioEngineSDK.model
{
    public class Entity
    {
        public string Id { get; set; }
        public string Kind { get; set; }
        public XDIP Xdip { get; set; }
        
        public List<object> Original { get; set; }
        public List<object> Modified { get; set; }

        public Entity()
        {
        }
    }
}