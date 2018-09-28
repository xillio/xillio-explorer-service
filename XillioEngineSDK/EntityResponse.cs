using System.Collections.Generic;
using XillioEngineSDK.model;

namespace XillioEngineSDK
{
    class EntityResponse
    {
        public Entity Entity { get; set; }
        public List<Entity> Entities  { get; set; }
        public List<EntityReference> EntityList  { get; set; }
        public List<Entity> Children  { get; set; }
        public List<EntityReference> ChildList  { get; set; }
        public List<Entity> Translations  { get; set; }
        public List<EntityReference> TranslationList  { get; set; }
        public List<Entity> Versions  { get; set; }
        public List<EntityReference> VersionList { get; set; }
        public object Metadata  { get; set; }
    }
}