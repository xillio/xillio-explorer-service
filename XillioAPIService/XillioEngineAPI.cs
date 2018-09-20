using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
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
        private const string BASE_URL = "http://tenant.localhost:8080";
        private HttpClient client = new HttpClient();
        private string token;
        
        public XillioEngineAPI()
        {
            client.BaseAddress = new Uri(BASE_URL);
        }

        public XillioEngineAPI(Uri baseUrl)
        {
            client.BaseAddress = baseUrl;
        }

        public AuthorizationResponse Authenticate(String username, String password, String clientId, String clientSecret)
        {
            return BASE_URL
                .AppendPathSegments("oauth", "token")
                .WithHeader("Content-Type", "application/x-www-form-urlencoded")
                .WithBasicAuth(clientId, clientSecret)
                .PostJsonAsync(new
                {
                    grant_type = "password",
                    username = username,
                    password = password
                }).ReceiveJson<AuthorizationResponse>().Result;
        }

        public List<BaseConfiguration> GetConfigurations()
        {
            return BASE_URL
                .AppendPathSegments("v2", "configurations")
                .WithOAuthBearerToken(token)
                .GetJsonAsync<List<BaseConfiguration>>().Result;
        }

        public JObject GetEntity(string configuration, string id)
        {
            return BASE_URL
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