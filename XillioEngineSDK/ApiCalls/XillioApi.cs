using System.Collections.Generic;
using System.Net.Http;
using Flurl;
using Flurl.Http;
using XillioEngineSDK.responses;
using Version = XillioEngineSDK.responses.Version;

namespace XillioEngineSDK
{
    public partial class XillioApi
    {
        private string baseUrl;
        private Authentication authentication;

        public XillioApi(string baseUrl, bool refreshToken)
        {
            this.baseUrl = baseUrl;
            authentication = refreshToken ? new Authentication(this) : new Authentication();
        }

        public AuthenticationInfo Authenticate(string username, string password, string clientId, string clientSecret)
        {
            AuthenticationInfo info = baseUrl
                .AppendPathSegments("oauth", "token")
                .WithHeader("Content-Type", "application/x-www-form-urlencoded")
                .WithBasicAuth(clientId, clientSecret)
                .PostAsync(new FormUrlEncodedContent(new Dictionary<string, string>()
                {
                    {"grant_type", "password"},
                    {"username", username},
                    {"password", password}
                }))
                .ReceiveJson<AuthenticationInfo>()
                .Result;

            return authentication.IsAutoRefresh()
                ? authentication.RegisterAuthentication(username, password, clientId, clientSecret, info)
                : authentication.RegisterAuthentication(info);
        }

        public Version Version()
        {
            return this.baseUrl
                .AppendPathSegments("v2", "system", "version")
                .GetJsonAsync<Version>()
                .Result;
        }

        public Ping Ping()
        {
            return this.baseUrl
                .AppendPathSegments("v2", "system", "version")
                .GetJsonAsync<Ping>()
                .Result;
        }

        private EntityResponse CallAPI(string uri, string scope)
        {
            return uri.SetQueryParam("scope", scope)
                .WithOAuthBearerToken(authentication.GetToken())
                .GetJsonAsync<EntityResponse>()
                .Result;
        }
        
    }
}