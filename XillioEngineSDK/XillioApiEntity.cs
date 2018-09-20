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
        public List<Entity> ListRepositories(AuthenticationInfo authenticationInfo)
        {
            return this.baseUrl
                .AppendPathSegments("v2", "entities")
                .SetQueryParam("scope", EntityScope.CHILDREN)
                .WithOAuthBearerToken(authenticationInfo.AccessToken)
                .GetJsonAsync<EntityResponse>()
                    .Result.Children;
        }
        
        public Entity GetEntity(AuthenticationInfo authenticationInfo, string id)
        {
            return this.baseUrl
                .AppendPathSegments("v2", "entities", id)
                .SetQueryParam("scope", EntityScope.ENTITY)
                .WithOAuthBearerToken(authenticationInfo.AccessToken)
                .GetJsonAsync<EntityResponse>()
                .Result.Entity;
        }
        
        public object GetEntityMetadata(AuthenticationInfo authenticationInfo, string id)
        {
            return this.baseUrl
                .AppendPathSegments("v2", "entities", id)
                .SetQueryParam("scope", EntityScope.METATDATA)
                .WithOAuthBearerToken(authenticationInfo.AccessToken)
                .GetJsonAsync<EntityResponse>()
                .Result.Metadata;
        }

        public List<Entity> GetChildren(AuthenticationInfo authenticationInfo, Entity entity)
        {
            return this.GetChildren(authenticationInfo, entity.Id);
        }
        
        public List<Entity> GetChildren(AuthenticationInfo authenticationInfo, string id)
        {
            return this.baseUrl
                .AppendPathSegments("v2", "entities", id)
                .SetQueryParam("scope", EntityScope.CHILDREN)
                .WithOAuthBearerToken(authenticationInfo.AccessToken)
                .GetJsonAsync<EntityResponse>()
                .Result.Children;
        }

        public List<object> GetChildrenList(AuthenticationInfo authenticationInfo, Entity entity)
        {
            return this.GetChildrenList(authenticationInfo, entity.Id);
        }
        
        public List<object> GetChildrenList(AuthenticationInfo authenticationInfo, string id)
        {
            return this.baseUrl
                .AppendPathSegments("v2", "entities", id)
                .SetQueryParam("scope", EntityScope.CHILD_LIST)
                .WithOAuthBearerToken(authenticationInfo.AccessToken)
                .GetJsonAsync<EntityResponse>()
                .Result.ChildList;
        }

        public List<Entity> GetEntityTranslations(AuthenticationInfo authenticationInfo, Entity entity)
        {
            return this.GetEntityTranslations(authenticationInfo, entity.Id);
        }
        
        public List<Entity> GetEntityTranslations(AuthenticationInfo authenticationInfo, string id)
        {
            return this.baseUrl
                .AppendPathSegments("v2", "entities", id)
                .SetQueryParam("scope", EntityScope.TRANSLATIONS)
                .WithOAuthBearerToken(authenticationInfo.AccessToken)
                .GetJsonAsync<EntityResponse>()
                .Result.Translations;
        }

        public List<object> GetEntityTranslationList(AuthenticationInfo authenticationInfo, Entity entity)
        {
            return this.GetEntityTranslationList(authenticationInfo, entity.Id);
        }

        public List<object> GetEntityTranslationList(AuthenticationInfo authenticationInfo, string id)
        {
            return this.baseUrl
                .AppendPathSegments("v2", "entities", id)
                .SetQueryParam("scope", EntityScope.TRANSLATION_LIST)
                .WithOAuthBearerToken(authenticationInfo.AccessToken)
                .GetJsonAsync<EntityResponse>()
                .Result.TranslationList;
        }

        public List<Entity> GetEntityVersions(AuthenticationInfo authenticationInfo, Entity entity)
        {
            return this.GetEntityVersions(authenticationInfo, entity.Id);
        }

        public List<Entity> GetEntityVersions(AuthenticationInfo authenticationInfo, string id)
        {
            return this.baseUrl
                .AppendPathSegments("v2", "entities", id)
                .SetQueryParam("scope", EntityScope.VERSIONS)
                .WithOAuthBearerToken(authenticationInfo.AccessToken)
                .GetJsonAsync<EntityResponse>()
                .Result.Versions;
        }
        
        public List<object> GetEntityVersionList(AuthenticationInfo authenticationInfo, Entity entity)
        {
            return this.GetEntityVersionList(authenticationInfo, entity.Id);
        }

        public List<object> GetEntityVersionList(AuthenticationInfo authenticationInfo, string id)
        {
            return this.baseUrl
                .AppendPathSegments("v2", "entities", id)
                .SetQueryParam("scope", EntityScope.VERSION_LIST)
                .WithOAuthBearerToken(authenticationInfo.AccessToken)
                .GetJsonAsync<EntityResponse>()
                .Result.VersionList;
        }

        public Entity CreateEntity(AuthenticationInfo authenticationInfo, Entity entity, Stream content = null)
        {          
            if (content == null)
            {
                return this.baseUrl
                    .AppendPathSegments("v2", "entities")
                    .WithOAuthBearerToken(authenticationInfo.AccessToken)
                    .PostMultipartAsync( mp => mp
                        .AddJson("entity", entity)
                    )
                    .ReceiveJson<EntityResponse>()
                    .Result.Entity;
            }
            else
            {
                return this.baseUrl
                    .AppendPathSegments("v2", "entities")
                    .WithOAuthBearerToken(authenticationInfo.AccessToken)
                    .PostMultipartAsync( mp => mp
                            .AddJson("entity", entity)
                            .AddFile("contents", content, "test.txt")
                    )
                    .ReceiveJson<EntityResponse>()
                    .Result.Entity;
            }
        }

        public Entity UpdateEntity(AuthenticationInfo authenticationInfo, Entity entity)
        {
            return this.UpdateEntity(authenticationInfo, entity.Id, entity);
        }

        public Entity UpdateEntity(AuthenticationInfo authenticationInfo, string id, Entity entity)
        {
            throw new NotImplementedException();
        }

        public void DeleteEntity(AuthenticationInfo authenticationInfo, Entity entity)
        {
            this.DeleteEntity(authenticationInfo, entity.Id);
        }

        public void DeleteEntity(AuthenticationInfo authenticationInfo, string id)
        {
            throw new NotImplementedException();
        }
    }
}