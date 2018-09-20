using System;
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

        public XillioApi(string baseUrl)
        {
            this.baseUrl = baseUrl;
        }

        public AuthenticationInfo Authenticate(string username, string password, string clientId, string clientSecret)
        {
            return this.baseUrl
                .AppendPathSegments("oauth", "token")
                .WithHeader("Content-Type", "application/x-www-form-urlencoded")
                .WithBasicAuth(clientId, clientSecret)
                .PostAsync(new FormUrlEncodedContent(new Dictionary<string ,string>()
                {
                    {"grant_type", "password"},
                    {"username", username},
                    {"password", password}
                }))
                .ReceiveJson<AuthenticationInfo>()
                .Result;
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
    }
    
}