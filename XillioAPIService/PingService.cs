using System;
using System.Timers;
using XillioAPIService.Model;

namespace XillioAPIService
{
    public class PingService
    {
        Timer ConfigurationRefreshDelay = new System.Timers.Timer(600000);
        Timer AuthenticationRefreshDelay = new System.Timers.Timer();
        private XillioEngineAPI api;

        public PingService(XillioEngineAPI api)
        {
            this.api = api;
            ConfigurationRefreshDelay.Elapsed += delegate(object sender, ElapsedEventArgs args) { RefreshConfigurations(); };
            AuthenticationRefreshDelay.Elapsed += delegate(object sender, ElapsedEventArgs args) { RunAuthentication(); };
        }

        public void Start()
        {
            ConfigurationRefreshDelay.Enabled = true;
            RunAuthentication();
            AuthenticationRefreshDelay.Enabled = true;
            RefreshConfigurations();
        }

        public void Stop()
        {
            ConfigurationRefreshDelay.Enabled = false;
        }
        
        /// <summary>
        /// 
        /// </summary>
        private void RefreshConfigurations()
        {
            // time to do a pull.
            ConfigurationRefreshDelay.Enabled = false;
            LogService.Log("Going to do a pull from Xillio API");
            
            InfoHolder.Configurations = api.GetConfigurations();
            if (InfoHolder.Configurations == null)
            {
                ConfigurationRefreshDelay.Enabled = true;
                return;
            }

            LogService.Log("this is the list of configurations: " + InfoHolder.Configurations.Count);
            ConfigurationRefreshDelay.Enabled = true;
        }

        private void RefreshRepository(BaseConfiguration configuration)
        {
            // time to scrape.
            configuration.RefreshDelay.Enabled = false;

            configuration.RefreshDelay.Enabled = true;
        }
        
        private void RunAuthentication()
        {
            LogService.Log("authenticating");
            InfoHolder.auth = api.Authenticate("user", "password", "client", "secret");
            AuthenticationRefreshDelay.Interval = InfoHolder.auth.expires_in;
        }
    }
}