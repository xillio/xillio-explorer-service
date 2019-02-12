using XillioEngineSDK;

namespace XillioAPIService
{
    //TODO find a way to to catch the opening of a file with the offline attribute and then do a get BinaryContent.
    //Maybe look at LastAccessed. (can be caught with the watcher.)
    public class DownloadService : IService
    {
        public XillioApi api;
        public void Start()
        {
            throw new System.NotImplementedException();
        }
        public void Pause()
        {
            throw new System.NotImplementedException();
        }
        public void Resume()
        {
            throw new System.NotImplementedException();
        }
        public void Stop()
        {
            throw new System.NotImplementedException();    
        }
    }
}