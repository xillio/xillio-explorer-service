using System.Collections.Generic;
using XillioEngineSDK.model;

namespace XillioEngineSDK
{
    class EntityResponse
    {
        public Entity Entity { get; set; }
        public List<Entity> Entities  { get; set; }
        public List<object> EntityList  { get; set; }
        public List<Entity> Children  { get; set; }
        public List<object> ChildList  { get; set; }
        public List<Entity> Translations  { get; set; }
        public List<object> TranslationList  { get; set; }
        public List<Entity> Versions  { get; set; }
        public List<object> VersionList { get; set; }
        public object Metadata  { get; set; }
    }
}