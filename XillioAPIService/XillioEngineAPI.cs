using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Security.Authentication;
using System.Text;
using System.Threading.Tasks;
using Flurl;
using Flurl.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using XillioAPIService.Model;

namespace XillioAPIService
{
    public class XillioEngineAPI
    {
        private string baseUrl;
        private string token;
        
        public XillioEngineAPI(string baseUrl)
        {
            this.baseUrl = baseUrl;
        }

        public AuthorizationResponse Authenticate(String username, String password, String clientId, String clientSecret)
        {
            try
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
                    .ReceiveJson<AuthorizationResponse>().Result;
            }
            catch (FlurlHttpException exception)
            {
                throw new AuthenticationException();
            }

            return null;
        }

        public List<BaseConfiguration> GetConfigurations()
        {
            return this.baseUrl
                .AppendPathSegments("v2", "configurations")
                .WithOAuthBearerToken(token)
                .GetJsonAsync<List<BaseConfiguration>>().Result;
        }

        public JObject GetEntity(string configuration, string id)
        {
            return this.baseUrl
                .AppendPathSegment($"v2/{configuration}/entities/{id}")
                .SetQueryParams(new {scope = "entity"})
                .WithOAuthBearerToken(token)
                .GetJsonAsync<JObject>().Result;
        }

        public List<JObject> GetChildren(string id)
        {
            return null;
        }
        
        public Task<T> ParseResponse<T>(Task<HttpResponseMessage> response)
        {
            return response.ContinueWith(r => r.Result.Content.ReadAsStringAsync())
                .ContinueWith(r => JsonConvert.DeserializeObject<T>(r.Result.Result));
        }
    }
}