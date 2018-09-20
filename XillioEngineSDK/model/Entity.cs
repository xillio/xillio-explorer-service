using System;
using System.Collections.Generic;
using XillioEngineSDK.model.decorators;

namespace XillioEngineSDK.model
{
    public class Entity
    {
        public string Id { get; set; }
        public string Kind { get; set; }
        public string Xdip { get; set; }
        
        public List<Decorator> Original { get; set; }
        public List<Decorator> Modified { get; set; }

        public Entity()
        {
        }
    }
}