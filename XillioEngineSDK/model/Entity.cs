using System;
using System.Runtime.Serialization;
using XillioEngineSDK.model.decorators;

namespace XillioEngineSDK.model
{
    [Serializable]
    public class Entity : ISerializable
    {
        public string Id { get; set; }
        public string Kind { get; set; }
        public string Xdip { get; set; }
        
        public DecoratorList Original { get; set; }
        public DecoratorList Modified { get; set; }

        public Entity()
        {
        }

        public Entity(SerializationInfo info, StreamingContext context)
        {
            Id = info.GetString("id");
            Kind = info.GetString("kind");
            Xdip = info.GetString("xdip");
            Original = (DecoratorList)info.GetValue("original", typeof(DecoratorList));
            Modified = (DecoratorList)info.GetValue("modified", typeof(DecoratorList));
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("id", Id);
            info.AddValue("xdip", Xdip);
            info.AddValue("kind", Kind);
            info.AddValue("original", Original);
            info.AddValue("modified", Modified);
        }
    }
}