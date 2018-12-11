using System.Collections.Generic;
using Flurl;
using Flurl.Http;
using XillioEngineSDK.model;

namespace XillioEngineSDK
{
    public partial class XillioApi
    {
        public List<Configuration> GetConfigurations()
        {
            return this.baseUrl
                .AppendPathSegments("v2", "configurations")
                .WithOAuthBearerToken(authentication.GetToken())
                .GetJsonAsync<List<Configuration>>()
                .Result;
        }

        public Configuration GetConfiguration(string id)
        {
            return this.baseUrl
                .AppendPathSegments("v2", "configurations", id)
                .WithOAuthBearerToken(authentication.GetToken())
                .GetJsonAsync<Configuration>()
                .Result;
        }

        public Configuration CreateConfiguration(Configuration configuration)
        {
            return this.baseUrl
                .AppendPathSegments("v2", "configurations")
                .WithOAuthBearerToken(authentication.GetToken())
                .PostJsonAsync(configuration)
                .ReceiveJson<Configuration>()
                .Result;
        }

        public Configuration UpdateConfiguration(Configuration configuration)
        {
            return this.UpdateConfiguration(configuration.Id, configuration);
        }

        public Configuration UpdateConfiguration(string configurationId, Configuration configuration)
        {
            return this.baseUrl
                .AppendPathSegments("v2", "configurations", configurationId)
                .WithOAuthBearerToken(authentication.GetToken())
                .PutJsonAsync(configuration)
                .ReceiveJson<Configuration>()
                .Result;
        }

        public void DeleteConfiguration(Configuration configuration)
        {
            this.DeleteConfiguration(configuration.Id);
        }

        public void DeleteConfiguration(string configurationId)
        {
            this.baseUrl
                .AppendPathSegments("v2", "configurations", configurationId)
                .WithOAuthBearerToken(authentication.GetToken())
                .DeleteAsync();
        }
    }
}