using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
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
            authentication = refreshToken ? new Authentication(this) : new Authentication();
            Ping();
        }

        public AuthenticationInfo Authenticate(string username, string password, string clientId, string clientSecret)
        {
            AuthenticationInfo info = DoCall(baseUrl
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
            );

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
                return DoCall(baseUrl
                    .AppendPathSegments("v2", "system", "version")
                    .GetJsonAsync<Ping>());
            }
            catch (XillioTimeoutException)
            {
                return null;
            }
        }

        private EntityResponse CallAPI(string uri, string scope)
        {
            return DoCall(uri.SetQueryParam("scope", scope)
                .WithOAuthBearerToken(authentication.GetToken())
                .GetJsonAsync<EntityResponse>());
        }

        private T DoCall<T>(Task<T> call) where T : class
        {
            try
            {
                var result = call.Result;
                reachable = true;
                return result;
            }
            catch (AggregateException e)
            {
                if (!(e.InnerExceptions[0] is FlurlHttpTimeoutException))
                {
                    throw;
                }
                reachable = false;
                throw new XillioTimeoutException("A timeout was recieved from the API", e);
            }
        }
    }
}