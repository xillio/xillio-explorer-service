using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Timers;
using XillioEngineSDK;
using XillioEngineSDK.model;
using XillioEngineSDK.model.decorators;

namespace XillioAPIService
{
    public class PingService : IService
    {
        Timer ConfigurationRefreshDelay = new Timer(60000);
        public XillioApi api { get; set; }

        public void Start()
        {
            ConfigurationRefreshDelay.Elapsed += delegate(object sender, ElapsedEventArgs args)
            {
                RefreshConfigurations();
            };
            ConfigurationRefreshDelay.Enabled = true;
            RefreshConfigurations();
        }

        public void Pause()
        {
            ConfigurationRefreshDelay.Enabled = false;
            foreach (var configurationsValue in InfoHolder.Configurations.Values)
            {
                configurationsValue.Item2.Enabled = false;
            }
        }

        public void Resume()
        {
            ConfigurationRefreshDelay.Enabled = true;
            foreach (var configurationsValue in InfoHolder.Configurations.Values)
            {
                configurationsValue.Item2.Enabled = true;
            }
        }

        public void Stop()
        {
            ConfigurationRefreshDelay.Dispose();
            foreach (var configurationsValue in InfoHolder.Configurations.Values)
            {
                configurationsValue.Item2.Dispose();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void RefreshConfigurations()
        {
            // time to do a pull.
            ConfigurationRefreshDelay.Enabled = false;
            LogService.Log("Going to do a pull from Xillio API");

            List<Configuration> configurations = api.GetConfigurations();

            List<Configuration> newConfigs =
                configurations.Where(c => !(InfoHolder.Configurations.ContainsKey(c.Name))).ToList();


            foreach (Configuration configuration in newConfigs)
            {
                LogService.Log("found a new config: " + configuration.Name);

                Timer timer = new Timer(2000);
                var tuple = Tuple.Create(configuration, timer);

                InfoHolder.Configurations.Add(configuration.Name, tuple);
                timer.Elapsed += delegate(Object sender, ElapsedEventArgs args) { RefreshRepository(tuple); };
                RefreshRepository(tuple);

                string path = InfoHolder.syncFolder + "/" + configuration.Name;
                Directory.CreateDirectory(path);
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
                                    ((NameDecorator) c.Original.Find(d => d is NameDecorator)).SystemName + "/")
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
                    File.SetAttributes(path, FileAttributes.Offline);
                }

                children.Remove(child);
            }

            return children;
        }
    }
}