using System;

namespace XillioAPIService
{
    public class PingService
    {
        System.Timers.Timer ConfigurationRefreshDelay = new System.Timers.Timer(100000);
        System.Timers.Timer AuthenticationRefreshDelay = new System.Timers.Timer();
        private XillioEngineAPI api;

        public PingService(XillioEngineAPI api)
        {
            this.api = api;
            ConfigurationRefreshDelay.Elapsed += new System.Timers.ElapsedEventHandler(RefreshConfigurations);
            AuthenticationRefreshDelay.Elapsed += new System.Timers.ElapsedEventHandler(RunAuthentication);
        }

        public void Start()
        {
            ConfigurationRefreshDelay.Enabled = true;
            RunAuthentication(null, null);
            AuthenticationRefreshDelay.Enabled = true;
            RefreshConfigurations(null, null);
        }

        public void Stop()
        {
            ConfigurationRefreshDelay.Enabled = false;
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RefreshConfigurations(object sender, System.Timers.ElapsedEventArgs e)
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
        
        private void RunAuthentication(object sender, System.Timers.ElapsedEventArgs e)
        {
            LogService.Log("authenticating");
            InfoHolder.auth = api.Authenticate("user", "password", "client", "secret");
            AuthenticationRefreshDelay.Interval = InfoHolder.auth.expires_in;
        }
    }
}