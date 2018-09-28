using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Management.Instrumentation;
using System.Runtime.Remoting.Messaging;
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
            ConfigurationRefreshDelay.Elapsed += delegate(object sender, ElapsedEventArgs args)
            {
                RefreshConfigurations();
            };
            AuthenticationRefreshDelay.Elapsed += delegate(object sender, ElapsedEventArgs args)
            {
                RunAuthentication();
            };
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
            
            List<Tuple<Entity, string>> children = api.GetChildren(configurationInfo.Item1)
                .Select(c => 
                    Tuple.Create(c, InfoHolder.syncFolder + "/" + configurationInfo.Item1.Name + 
                                    ((NameDecorator)c.Original.Find(d => d is NameDecorator)).SystemName + "/")
                    )
                .ToList();
            
            
            LogService.Log("Level 0 has " + children.Count + " entities.");
            int level = 0;

            while (children.Count > 0)
            {
                level++;
                children = IndexChildren(children);
                LogService.Log($"Level {level} has {children.Count} entities.");
            }

            configurationInfo.Item2.Enabled = true;
        }

        private List<Tuple<Entity, string>> IndexChildren(List<Tuple<Entity, string>> children)
        {
            foreach (Tuple<Entity, string> child in children)
            {
                string path = child.Item2 +
                              ((NameDecorator) child.Item1.Original.Find(d => d is NameDecorator)).SystemName;
                if (((ContainerDecorator) child.Item1.Original.Find(d => d is ContainerDecorator)).HasChildren)
                {
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }

                    children.AddRange(
                        api.GetChildren(child.Item1).Select(c =>
                            Tuple.Create(c,
                                child.Item2 + ((NameDecorator) c.Original.Find(d => d is NameDecorator)).SystemName +
                                "/")
                        ));
                }
                else if (!File.Exists(path))
                {
                    File.Create(path);
                }

                children.Remove(child);
            }

            return children;
        }

        private void RunAuthentication()
        {
            LogService.Log("authenticating");
            InfoHolder.auth = api.Authenticate("user", "password", "client", "secret");
            AuthenticationRefreshDelay.Interval = InfoHolder.auth.ExpiresIn;
        }
    }
}