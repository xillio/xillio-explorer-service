using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using XillioAPIService.Model;

namespace XillioAPIService
{
    public class XillioEngineAPI
    {
        private HttpClient client = new HttpClient();
        
        public XillioEngineAPI()
        {
            client.BaseAddress = new Uri("http://tenant.localhost:8080");
        }

        public XillioEngineAPI(Uri baseUrl)
        {
            client.BaseAddress = baseUrl;
        }

        public AuthorizationResponse Authenticate(String username, String password, String clientId, String clientSecret)
        {
            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            dictionary.Add("grant_type", "password");
            dictionary.Add("username", username);
            dictionary.Add("password", password);
            var request = new HttpRequestMessage(HttpMethod.Post, "/oauth/token");
            request.Content = new FormUrlEncodedContent(dictionary);
            request.Headers.Authorization = new AuthenticationHeaderValue("Basic",
                Convert.ToBase64String(Encoding.UTF8.GetBytes(clientId + ":" + clientSecret)));
            
            var parsed = ParseResponse<AuthorizationResponse>(client.SendAsync(request));
            parsed.ContinueWith(r =>
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer",
                    (r.Result).access_token);
            });
            return parsed.Result;
        }

        public List<BaseConfiguration> GetConfigurations()
        {
            var response = client.GetAsync("v2/configurations");
            if (response.Result.StatusCode != HttpStatusCode.OK)
            {
                return null;
            }
            var result = ParseResponse<List<BaseConfiguration>>(response);
            result.Wait();
            return result.Result;
        }

        public Task<T> ParseResponse<T>(Task<HttpResponseMessage> response)
        {
            return response.ContinueWith(r => r.Result.Content.ReadAsStringAsync())
                .ContinueWith(r => JsonConvert.DeserializeObject<T>(r.Result.Result));
        }
    }
}