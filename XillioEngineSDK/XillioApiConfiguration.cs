using System;
using System.Collections.Generic;
using Flurl;
using Flurl.Http;
using XillioEngineSDK.model;
using XillioEngineSDK.responses;

namespace XillioEngineSDK
{
    public partial class XillioApi
    {
        public List<Configuration> GetConfigurations(AuthenticationInfo authenticationInfo)
        {
            return this.baseUrl
                .AppendPathSegments("v2", "configurations")
                .WithOAuthBearerToken(authenticationInfo.AccessToken)
                .GetJsonAsync<List<Configuration>>()
                .Result;
        }
        
        public Configuration GetConfiguration(AuthenticationInfo authenticationInfo, string id)
        {
            return this.baseUrl
                .AppendPathSegments("v2", "configurations", id)
                .WithOAuthBearerToken(authenticationInfo.AccessToken)
                .GetJsonAsync<Configuration>()
                .Result;
        }

        public Configuration CreateConfiguration(AuthenticationInfo authenticationInfo, Configuration configuration)
        {
            return this.baseUrl
                .AppendPathSegments("v2", "configurations")
                .WithOAuthBearerToken(authenticationInfo.AccessToken)
                .PostJsonAsync(configuration)
                .ReceiveJson<Configuration>()
                .Result;
        }

        public Configuration UpdateConfiguration(AuthenticationInfo authenticationInfo, Configuration configuration)
        {
            return this.UpdateConfiguration(authenticationInfo, configuration.Id, configuration);
        }
        
        public Configuration UpdateConfiguration(AuthenticationInfo authenticationInfo, string configurationId, Configuration configuration)
        {
            return this.baseUrl
                .AppendPathSegments("v2", "configurations", configurationId)
                .WithOAuthBearerToken(authenticationInfo.AccessToken)
                .PutJsonAsync(configuration)
                .ReceiveJson<Configuration>()
                .Result;
        }

        public void DeleteConfiguration(AuthenticationInfo authenticationInfo, Configuration configuration)
        {
            this.DeleteConfiguration(authenticationInfo, configuration.Id);
        }

        public void DeleteConfiguration(AuthenticationInfo authenticationInfo, string configurationId)
        {
            this.baseUrl
                .AppendPathSegments("v2", "configurations", configurationId)
                .WithOAuthBearerToken(authenticationInfo.AccessToken)
                .DeleteAsync();
        }
    }
}