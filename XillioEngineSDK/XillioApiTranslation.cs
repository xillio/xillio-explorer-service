using System.Collections.Generic;
using Flurl;
using Flurl.Http;
using XillioEngineSDK.model;
using XillioEngineSDK.responses;

namespace XillioEngineSDK
{
    public partial class XillioApi
    {
        public List<Entity> GetEntityTranslations(AuthenticationInfo authenticationInfo, Configuration configuration, Entity entity)
        {
            return this.GetEntityTranslations(authenticationInfo, configuration, entity.Id);
        }
        
        public List<Entity> GetEntityTranslations(AuthenticationInfo authenticationInfo, Configuration configuration, string id)
        {
            return this.baseUrl
                .AppendPathSegments("v2", "entities", configuration.Id, id)
                .SetQueryParam("scope", EntityScope.TRANSLATIONS)
                .WithOAuthBearerToken(authenticationInfo.AccessToken)
                .GetJsonAsync<EntityResponse>()
                .Result.Translations;
        }

        public List<EntityReference> GetEntityTranslationList(AuthenticationInfo authenticationInfo, Configuration configuration, Entity entity)
        {
            return this.GetEntityTranslationList(authenticationInfo, configuration, entity.Id);
        }

        public List<EntityReference> GetEntityTranslationList(AuthenticationInfo authenticationInfo, Configuration configuration, string id)
        {
            return this.baseUrl
                .AppendPathSegments("v2", "entities", configuration.Id, id)
                .SetQueryParam("scope", EntityScope.TRANSLATION_LIST)
                .WithOAuthBearerToken(authenticationInfo.AccessToken)
                .GetJsonAsync<EntityResponse>()
                .Result.TranslationList;
        }
    }
}