using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Threading;
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
        public bool reachable;

        public XillioApi(string baseUrl, bool refreshToken)
        {
            this.baseUrl = baseUrl;

            Ping();

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
            try
            {
                return this.baseUrl
                    .AppendPathSegments("v2", "system", "version")
                    .GetJsonAsync<Ping>()
                    .Result;
            }
            catch (AggregateException e)
            {
                if (e.InnerExceptions[0] is FlurlHttpException)
                {
                    reachable = false;
                    return null;
                }

                throw;
            }
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