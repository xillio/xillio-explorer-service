using System.Collections.Generic;
using Flurl;
using Flurl.Http;
using XillioEngineSDK.model;
using XillioEngineSDK.responses;

namespace XillioEngineSDK
{
    public partial class XillioApi
    {
        public List<Entity> GetEntityVersions(AuthenticationInfo authenticationInfo, Configuration configuration, Entity entity)
        {
            return this.GetEntityVersions(authenticationInfo, configuration, entity.Id);
        }

        public List<Entity> GetEntityVersions(AuthenticationInfo authenticationInfo, Configuration configuration, string id)
        {
            return this.baseUrl
                .AppendPathSegments("v2", "entities", configuration.Id, id)
                .SetQueryParam("scope", EntityScope.VERSIONS)
                .WithOAuthBearerToken(authenticationInfo.AccessToken)
                .GetJsonAsync<EntityResponse>()
                .Result.Versions;
        }

        public List<EntityReference> GetEntityVersionList(AuthenticationInfo authenticationInfo,
            EntityReference reference)
        {
            return this.GetEntityVersionList(authenticationInfo, reference.Id);
        }
        
        public List<EntityReference> GetEntityVersionList(AuthenticationInfo authenticationInfo, Entity entity)
        {
            return this.GetEntityVersionList(authenticationInfo, entity.Id);
        }

        public List<EntityReference> GetEntityVersionList(AuthenticationInfo authenticationInfo, Configuration configuration, string path)
        {
            return this.GetEntityVersionList(authenticationInfo, baseUrl
                .AppendPathSegments("v2", "entities", configuration.Id, path));
        }

        private List<EntityReference> GetEntityVersionList(AuthenticationInfo authenticationInfo, string id)
        {
            return id
                .SetQueryParam("scope", EntityScope.VERSION_LIST)
                .WithOAuthBearerToken(authenticationInfo.AccessToken)
                .GetJsonAsync<EntityResponse>()
                .Result.VersionList;
        }
    }
}