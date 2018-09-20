using System;
using System.Collections.Generic;
using System.IO;
using Flurl;
using Flurl.Http;
using Flurl.Http.Content;
using XillioEngineSDK.model;
using XillioEngineSDK.responses;

namespace XillioEngineSDK
{
    public partial class XillioApi
    {
        public List<Entity> ListRepositories(AuthenticationInfo authenticationInfo, Configuration configuration)
        {
            return this.baseUrl
                .AppendPathSegments("v2", "entities", configuration.Id)
                .SetQueryParam("scope", EntityScope.CHILDREN)
                .WithOAuthBearerToken(authenticationInfo.AccessToken)
                .GetJsonAsync<EntityResponse>()
                    .Result.Children;
        }
        
        public Entity GetEntity(AuthenticationInfo authenticationInfo, Configuration configuration, string id)
        {
            return this.baseUrl
                .AppendPathSegments("v2", "entities", configuration.Id, id)
                .SetQueryParam("scope", EntityScope.ENTITY)
                .WithOAuthBearerToken(authenticationInfo.AccessToken)
                .GetJsonAsync<EntityResponse>()
                .Result.Entity;
        }
        
        public object GetEntityMetadata(AuthenticationInfo authenticationInfo, Configuration configuration, string id)
        {
            return this.baseUrl
                .AppendPathSegments("v2", "entities", configuration.Id, id)
                .SetQueryParam("scope", EntityScope.METATDATA)
                .WithOAuthBearerToken(authenticationInfo.AccessToken)
                .GetJsonAsync<EntityResponse>()
                .Result.Metadata;
        }

        public List<Entity> GetChildren(AuthenticationInfo authenticationInfo, Configuration configuration, Entity entity)
        {
            return this.GetChildren(authenticationInfo, configuration, entity.Id);
        }
        
        public List<Entity> GetChildren(AuthenticationInfo authenticationInfo, Configuration configuration, string id)
        {
            string url = this.baseUrl
                .AppendPathSegments($"v2/entities/{configuration.Id}/{id}")
                .SetQueryParam("scope", EntityScope.CHILDREN);
            return this.baseUrl
                .AppendPathSegments($"v2/entities/{configuration.Id}/{id}")
                .SetQueryParam("scope", EntityScope.CHILDREN)
                .WithOAuthBearerToken(authenticationInfo.AccessToken)
                .GetJsonAsync<EntityResponse>()
                .Result.Children;
        }

        public List<object> GetChildrenList(AuthenticationInfo authenticationInfo, Configuration configuration, Entity entity)
        {
            return this.GetChildrenList(authenticationInfo, configuration, entity.Id);
        }
        
        public List<object> GetChildrenList(AuthenticationInfo authenticationInfo, Configuration configuration, string id)
        {
            return this.baseUrl
                .AppendPathSegments("v2", "entities", configuration.Id, id)
                .SetQueryParam("scope", EntityScope.CHILD_LIST)
                .WithOAuthBearerToken(authenticationInfo.AccessToken)
                .GetJsonAsync<EntityResponse>()
                .Result.ChildList;
        }

        public List<Entity> GetEntityTranslations(AuthenticationInfo authenticationInfo, Configuration configuration, Entity entity)
        {
            return this.GetEntityTranslations(authenticationInfo, configuration, entity.Id);
        }
        
        public List<Entity> GetEntityTranslations(AuthenticationInfo authenticationInfo, Configuration configuration, string id)
        {
            return this.baseUrl
                .AppendPathSegments("v2", "entities", configuration.Id, id)
                .SetQueryParam("scope", EntityScope.TRANSLATIONS)
                .WithOAuthBearerToken(authenticationInfo.AccessToken)
                .GetJsonAsync<EntityResponse>()
                .Result.Translations;
        }

        public List<object> GetEntityTranslationList(AuthenticationInfo authenticationInfo, Configuration configuration, Entity entity)
        {
            return this.GetEntityTranslationList(authenticationInfo, configuration, entity.Id);
        }

        public List<object> GetEntityTranslationList(AuthenticationInfo authenticationInfo, Configuration configuration, string id)
        {
            return this.baseUrl
                .AppendPathSegments("v2", "entities", configuration.Id, id)
                .SetQueryParam("scope", EntityScope.TRANSLATION_LIST)
                .WithOAuthBearerToken(authenticationInfo.AccessToken)
                .GetJsonAsync<EntityResponse>()
                .Result.TranslationList;
        }

        public List<Entity> GetEntityVersions(AuthenticationInfo authenticationInfo, Configuration configuration, Entity entity)
        {
            return this.GetEntityVersions(authenticationInfo, configuration, entity.Id);
        }

        public List<Entity> GetEntityVersions(AuthenticationInfo authenticationInfo, Configuration configuration, string id)
        {
            return this.baseUrl
                .AppendPathSegments("v2", "entities", configuration.Id, id)
                .SetQueryParam("scope", EntityScope.VERSIONS)
                .WithOAuthBearerToken(authenticationInfo.AccessToken)
                .GetJsonAsync<EntityResponse>()
                .Result.Versions;
        }
        
        public List<object> GetEntityVersionList(AuthenticationInfo authenticationInfo, Configuration configuration, Entity entity)
        {
            return this.GetEntityVersionList(authenticationInfo, configuration, entity.Id);
        }

        public List<object> GetEntityVersionList(AuthenticationInfo authenticationInfo, Configuration configuration, string id)
        {
            return this.baseUrl
                .AppendPathSegments("v2", "entities", configuration.Id, id)
                .SetQueryParam("scope", EntityScope.VERSION_LIST)
                .WithOAuthBearerToken(authenticationInfo.AccessToken)
                .GetJsonAsync<EntityResponse>()
                .Result.VersionList;
        }

        public Entity CreateEntity(AuthenticationInfo authenticationInfo, Entity entity, Configuration configuration, Stream content = null)
        {          
            if (content == null)
            {
                return this.baseUrl
                    .AppendPathSegments("v2", "entities", configuration.Id)
                    .WithOAuthBearerToken(authenticationInfo.AccessToken)
                    .PostMultipartAsync( mp => mp
                        .AddJson("entities", entity)
                    )
                    .ReceiveJson<EntityResponse>()
                    .Result.Entity;
            }
            else
            {
                return this.baseUrl
                    .AppendPathSegments("v2", "entities", configuration.Id)
                    .WithOAuthBearerToken(authenticationInfo.AccessToken)
                    .PostMultipartAsync( mp => mp
                            .AddJson("entities", entity)
                            .AddFile("contents", content, "test.txt")
                    )
                    .ReceiveJson<EntityResponse>()
                    .Result.Entity;
            }
        }

        public Entity UpdateEntity(AuthenticationInfo authenticationInfo, Configuration configuration, Entity entity)
        {
            return this.UpdateEntity(authenticationInfo, configuration, entity.Id, entity);
        }

        public Entity UpdateEntity(AuthenticationInfo authenticationInfo, Configuration configuration, string id, Entity entity)
        {
            throw new NotImplementedException();
        }

        public void DeleteEntity(AuthenticationInfo authenticationInfo, Configuration configuration, Entity entity)
        {
            this.DeleteEntity(authenticationInfo, configuration, entity.Id);
        }

        public void DeleteEntity(AuthenticationInfo authenticationInfo, Configuration configuration, string id)
        {
            throw new NotImplementedException();
        }
    }
}