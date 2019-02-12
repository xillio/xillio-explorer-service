using System.Collections.Generic;
using Flurl;
using XillioEngineSDK.model;

namespace XillioEngineSDK
{
    public partial class XillioApi
    {
        public List<Entity> GetEntityVersions(Entity entity)
        {
            return this.GetEntityVersions(entity.Id);
        }

        public List<Entity> GetEntityVersions(Configuration configuration, string path)
        {
            return GetEntityVersions(baseUrl.AppendPathSegments("v2", "entities", configuration.Id, path));
        }

        private List<Entity> GetEntityVersions(string id)
        {
            return CallAPI(id, EntityScope.VERSIONS).Versions;
        }

        public List<EntityReference> GetEntityVersionList(EntityReference reference)
        {
            return GetEntityVersionList(reference.Id);
        }

        public List<EntityReference> GetEntityVersionList(Entity entity)
        {
            return GetEntityVersionList(entity.Id);
        }

        public List<EntityReference> GetEntityVersionList(Configuration configuration, string path)
        {
            return GetEntityVersionList(baseUrl.AppendPathSegments("v2", "entities", configuration.Id, path));
        }

        private List<EntityReference> GetEntityVersionList(string id)
        {
            return CallAPI(id, EntityScope.VERSION_LIST).VersionList;
        }
    }
}