using System;
using System.Collections.Generic;
using System.IO;
using XillioEngineSDK.model;
using XillioEngineSDK.responses;

namespace XillioEngineSDK
{
    public partial class XillioApi
    {
        public List<Entity> ListRepositories(AuthenticationInfo authenticationInfo)
        {
            throw new NotImplementedException();
        }
        
        public Entity GetEntity(AuthenticationInfo authenticationInfo, string id)
        {
            throw new NotImplementedException();
        }
        
        public object GetEntityMetadata(AuthenticationInfo authenticationInfo, string id)
        {
            throw new NotImplementedException();
        }
        
        public List<Entity> GetChildren(AuthenticationInfo authenticationInfo, string id)
        {
            throw new NotImplementedException();
        }
        
        public List<EntityReference> GetChildrenList(AuthenticationInfo authenticationInfo, string id)
        {
            throw new NotImplementedException();
        }

        public List<object> GetEntityTranslations(AuthenticationInfo authenticationInfo, Entity entity)
        {
            return this.GetEntityTranslations(authenticationInfo, entity.Id);
        }
        
        public List<object> GetEntityTranslations(AuthenticationInfo authenticationInfo, string id)
        {
            throw new NotImplementedException();
        }

        public List<object> GetEntityTranslationList(AuthenticationInfo authenticationInfo, Entity entity)
        {
            return this.GetEntityTranslationList(authenticationInfo, entity.Id);
        }

        public List<object> GetEntityTranslationList(AuthenticationInfo authenticationInfo, string id)
        {
            throw new NotImplementedException();
        }

        public List<Entity> GetEntityVersions(AuthenticationInfo authenticationInfo, Entity entity)
        {
            return this.GetEntityVersions(authenticationInfo, entity.Id);
        }

        public List<Entity> GetEntityVersions(AuthenticationInfo authenticationInfo, string id)
        {
            throw new NotImplementedException();
        }

        public Entity CreateEntity(AuthenticationInfo authenticationInfo, Entity entity, StreamWriter content = null)
        {
            throw new NotImplementedException();
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