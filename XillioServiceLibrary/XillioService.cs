using System;
using XillioAPIService;
using XillioEngineSDK;

namespace XillioServiceLibrary
{
    public class XillioService
    {
        private XillioApi api;
        private WatcherService watcher;
        private PingService ping;
        
        public void Start()
        {
            LogService.Clear();
            LogService.Log("starting up the service.");

            api = new XillioApi("http://tenant.localhost:8080/", true);
            RunAuthentication();
            
            //Setup other services
            watcher = new WatcherService(api);
            try
            {
                ping = new PingService(api);
            }
            catch (Exception e)
            {
                LogService.Log(e);
                throw;
            }
            LogService.Log("service started.");
        }

        public void Pause()
        {
            watcher.Pause();
            ping.Pause();
        }
        
        public void Resume()
        {
            watcher.Resume();
            ping.Resume();
        }

        public void Stop()
        {
            watcher.Stop();
            ping.Stop();
            LogService.Clear();
        }
        
        private void RunAuthentication()
        {
            LogService.Log("authenticating");
            try
            {
                api.Authenticate("user", "password", "client", "secret");
            }
            catch (AggregateException e)
            {
                LogService.Log("Could not authenticate with xillio API.");
                throw;
            }

            LogService.Log("Authentication Complete");
        }
    }
}