using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Management.Instrumentation;
using System.Timers;
using XillioEngineSDK;
using XillioEngineSDK.model;
using XillioEngineSDK.model.decorators;

namespace XillioAPIService
{
    public class PingService
    {
        Timer ConfigurationRefreshDelay = new System.Timers.Timer(60000);
        Timer AuthenticationRefreshDelay = new System.Timers.Timer();
        private XillioApi api;

        public PingService(XillioApi api)
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

            List<Configuration> configurations = api.GetConfigurations(InfoHolder.auth);

            List<Configuration> newConfigs =
                configurations.Where(c => !(InfoHolder.Configurations.Select(t => t.Item1).Contains(c))).ToList();

            InfoHolder.Configurations.AddRange(configurations
                .Where(c => !(InfoHolder.Configurations.Select(t => t.Item1).Contains(c)))
                .Select(c =>
                {
                    Timer timer = new Timer(2000);
                    var tuple = Tuple.Create(c, timer);
                    timer.Elapsed += delegate(object sender, ElapsedEventArgs args) { RefreshRepository(tuple); };
                    timer.Enabled = true;
                    return tuple;
                }).ToList());

            foreach (Configuration configuration in newConfigs)
            {
                LogService.Log("found a new config: " + configuration.Name);
                string path = InfoHolder.syncFolder + "/" + configuration.Name;
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
            }
            
            ConfigurationRefreshDelay.Enabled = true;
        }

        private void RefreshRepository(Tuple<Configuration, Timer> configurationInfo)
        {
            // time to scrape.
            configurationInfo.Item2.Enabled = false;
            LogService.Log("starting to scrape for " + configurationInfo.Item1.Name);
            List<Entity> rootEntities = api.GetChildren(InfoHolder.auth, configurationInfo.Item1, "");
            LogService.Log("ammount of 1st level objects is " + rootEntities.Count);
            
            foreach (Entity rootEntity in rootEntities)
            {
                string path = InfoHolder.syncFolder + "/" + configurationInfo.Item1.Name + "/" + ((NameDecorator) rootEntity.Original.Find(d => d is NameDecorator)).SystemName;
                if (((ContainerDecorator) rootEntity.Original.Find(d => d is ContainerDecorator)).HasChildren)
                {
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    else if (!File.Exists(path))
                    {
                        File.Create(path);
                    }
                }
            }
            
            configurationInfo.Item2.Enabled = true;
        }
        
        private void RunAuthentication()
        {
            LogService.Log("authenticating");
            InfoHolder.auth = api.Authenticate("user", "password", "client", "secret");
            AuthenticationRefreshDelay.Interval = InfoHolder.auth.ExpiresIn;
        }
        
    }
    
    
}