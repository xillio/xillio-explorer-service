using XillioEngineSDK;

namespace XillioAPIService
{
    public interface IService
    {
        XillioApi api { get; set; }

        void Start();
        void Pause();
        void Resume();
        void Stop();
    }
}