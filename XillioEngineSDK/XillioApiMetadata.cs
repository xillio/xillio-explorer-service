using Flurl;
using Flurl.Http;
using XillioEngineSDK.model;
using XillioEngineSDK.responses;

namespace XillioEngineSDK
{
    public partial class XillioApi
    {
        public object GetEntityMetadata(AuthenticationInfo authenticationInfo, Configuration configuration, string id)
        {
            return this.baseUrl
                .AppendPathSegments("v2", "entities", configuration.Id, id)
                .SetQueryParam("scope", EntityScope.METATDATA)
                .WithOAuthBearerToken(authenticationInfo.AccessToken)
                .GetJsonAsync<EntityResponse>()
                .Result.Metadata;
        }
    }
}