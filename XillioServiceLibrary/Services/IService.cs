using XillioEngineSDK;

namespace XillioAPIService
{
    public interface IService
    {
        void Start();
        void Pause();
        void Resume();
        void Stop();
    }
}