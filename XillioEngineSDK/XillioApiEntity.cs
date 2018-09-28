using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Flurl;
using Flurl.Http;
using Flurl.Http.Content;
using XillioEngineSDK.model;
using XillioEngineSDK.responses;

namespace XillioEngineSDK
{
    public partial class XillioApi
    {
        

        public Entity GetEntity(EntityReference reference)
        {
            return GetEntity(reference.Id);
        }

        public Entity GetEntity(Configuration configuration, string path)
        {
            return GetEntity(baseUrl
                .AppendPathSegments("v2", "entities", configuration.Id, path));
        }

        private Entity GetEntity(string id)
        {
            return CallAPI(id, EntityScope.ENTITY).Entity;
        }

        public List<Entity> GetChildren(Entity entity)
        {
            return GetChildren(entity.Id);
        }
        
        public List<Entity> GetChildren(EntityReference reference)
        {
            return GetChildren(reference.Id);
        }
        
        /// <summary>
        /// Gets the root files and folders of a configuration
        /// </summary>
        /// <param name=""></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public List<Entity> GetChildren(Configuration configuration)
        {
            return GetChildren(baseUrl
                .AppendPathSegments("v2", "entities", configuration.Id));
        }

        public List<Entity> GetChildren(Configuration configuration, string path)
        {
            return GetChildren(baseUrl
                .AppendPathSegments($"v2/entities/{configuration.Id}/{path}"));
        }

        private List<Entity> GetChildren(string id)
        {
            return CallAPI(id, EntityScope.CHILDREN).Children;
        }

        public List<EntityReference> GetChildrenList(Entity entity)
        {
            return GetChildrenList(entity.Id);
        }
        
        public List<EntityReference> GetChildrenList(EntityReference reference)
        {
            return GetChildrenList(reference.Id);
        }
        
        public List<EntityReference> GetChildrenList(Configuration configuration, string id)
        {
            return GetChildrenList(baseUrl.AppendPathSegments("v2", "entities", configuration.Id, id));
        }

        public List<EntityReference> GetChildrenList(string id)
        {
            return CallAPI(id, EntityScope.CHILD_LIST).ChildList;
        }

        public Entity CreateEntity(Entity entity, Configuration configuration,
            Stream content = null)
        {
            if (content == null)
            {
                return baseUrl
                    .AppendPathSegments("v2", "entities", configuration.Id)
                    .WithOAuthBearerToken(authentication.GetToken())
                    .PostMultipartAsync(mp => mp
                        .AddJson("entities", entity)
                    )
                    .ReceiveJson<EntityResponse>()
                    .Result.Entity;
            }
            else
            {
                return baseUrl
                    .AppendPathSegments("v2", "entities", configuration.Id)
                    .WithOAuthBearerToken(authentication.GetToken())
                    .PostMultipartAsync(mp => mp
                        .AddJson("entities", entity)
                        .AddFile("contents", content, "test.txt")
                    )
                    .ReceiveJson<EntityResponse>()
                    .Result.Entity;
            }
        }

        public Entity UpdateEntity(Configuration configuration, Entity entity)
        {
            return UpdateEntity(configuration, entity.Id, entity);
        }

        public Entity UpdateEntity(Configuration configuration, string id,
            Entity entity)
        {
            throw new NotImplementedException();
        }

        public void DeleteEntity(Configuration configuration, Entity entity)
        {
            DeleteEntity(configuration, entity.Id);
        }

        public void DeleteEntity(Configuration configuration, string id)
        {
            throw new NotImplementedException();
        }
    }
}