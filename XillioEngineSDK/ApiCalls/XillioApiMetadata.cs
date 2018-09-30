using Flurl;
using Flurl.Http;
using XillioEngineSDK.model;
using XillioEngineSDK.responses;

namespace XillioEngineSDK
{
    public partial class XillioApi
    {
        
        
        public object GetEntityMetadata(Configuration configuration, string path)
        {
            return GetEntityMetadata(baseUrl.AppendPathSegments("v2", "entities", configuration.Id, path));
        }

        private object GetEntityMetadata(string id)
        {
            return CallAPI(id, EntityScope.METATDATA).Metadata;
        }
    }
}