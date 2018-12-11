using System.Collections.Generic;
using Flurl;
using XillioEngineSDK.model;

namespace XillioEngineSDK
{
    public partial class XillioApi
    {
        public List<Entity> GetEntityTranslations(Configuration configuration, Entity entity)
        {
            return GetEntityTranslations(entity.Id);
        }

        public List<Entity> GetEntityTranslations(Configuration configuration, string path)
        {
            return GetEntityTranslations(baseUrl.AppendPathSegments("v2", "entities", configuration.Id, path));
        }

        private List<Entity> GetEntityTranslations(string id)
        {
            return CallAPI(id, EntityScope.TRANSLATIONS).Translations;
        }

        public List<EntityReference> GetEntityTranslationList(Entity entity)
        {
            return GetEntityTranslationList(entity.Id);
        }

        public List<EntityReference> GetEntityTranslationList(Configuration configuration, string path)
        {
            return GetEntityTranslationList(baseUrl.AppendPathSegments("v2", "entities", configuration.Id, path));
        }

        private List<EntityReference> GetEntityTranslationList(string id)
        {
            return CallAPI(id, EntityScope.TRANSLATION_LIST).TranslationList;
        }
    }
}