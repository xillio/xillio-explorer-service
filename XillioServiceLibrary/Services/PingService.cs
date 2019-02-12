using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;
using XillioEngineSDK;
using XillioEngineSDK.model;

namespace XillioAPIService
{
    public class PingService : IService
    {
        private readonly Timer configurationRefreshDelay;
        public XillioApi api;

        public PingService(XillioApi api)
        {
            this.api = api;
            configurationRefreshDelay = new Timer(600000);
            Start();
        }

        public void Start()
        {
            configurationRefreshDelay.Elapsed += delegate(object sender, ElapsedEventArgs args)
            {
                RefreshConfigurations();
            };
            configurationRefreshDelay.Enabled = true;
            RefreshConfigurations();
        }

        public void Pause()
        {
            configurationRefreshDelay.Enabled = false;
            foreach (var configurationsValue in InfoHolder.Configurations.Values)
            {
                configurationsValue.Item2.Enabled = false;
            }
        }

        public void Resume()
        {
            configurationRefreshDelay.Enabled = true;
            foreach (var configurationsValue in InfoHolder.Configurations.Values)
            {
                configurationsValue.Item2.Enabled = true;
            }
        }

        public void Stop()
        {
            configurationRefreshDelay.Dispose();
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
            configurationRefreshDelay.Enabled = false;
            LogService.Log("Going to do a pull from Xillio API");

            List<Configuration> configurations = api.GetConfigurations();
            
            DeleteUnavailableConfigs(configurations);

            List<Configuration> newConfigs =
                configurations.Where(c => !(InfoHolder.Configurations.ContainsKey(c.Name))).ToList();

            Parallel.ForEach(newConfigs, HandleRepo);

            configurationRefreshDelay.Enabled = true;
        }

        private void HandleRepo(Configuration configuration)
        {
            LogService.Log("found a new config: " + configuration.Name);

            Timer timer = new Timer(180000);
            var tuple = Tuple.Create(configuration, timer);

            InfoHolder.Configurations.Add(configuration.Name, tuple);

            string path = InfoHolder.syncFolder + "/" + configuration.Name;
            Directory.CreateDirectory(path);

            timer.Elapsed += delegate(Object sender, ElapsedEventArgs args) { RefreshRepository(tuple); };
            RefreshRepository(tuple);
        }

        private void RefreshRepository(Tuple<Configuration, Timer> configurationInfo)
        {
            // time to scrape.
            configurationInfo.Item2.Enabled = false;
            LogService.Log("starting to scrape for " + configurationInfo.Item1.Name);
            string path = Path.Combine(InfoHolder.syncFolder, configurationInfo.Item1.Name);

            List<Tuple<Entity, string>> children = api.GetChildren(configurationInfo.Item1)
                .Select(c => Tuple.Create(c, path))
                .ToList();


            LogService.Log("Level 0 has " + children.Count + " entities.");
            int level = 0;
            
            DeleteUnavailableChildren(path, children);

            while (children.Count > 0)
            {
                children = IndexChildren(children);
                level++;
                LogService.Log($"Level {level} has {children.Count} entities.");
            }

            configurationInfo.Item2.Enabled = true;
        }

        /// <summary>
        /// Indexes the children of an entity and creates files for them.
        /// </summary>
        /// <param name="children"></param>
        /// <returns></returns>
        private List<Tuple<Entity, string>> IndexChildren(List<Tuple<Entity, string>> children)
        {
            var newChildren = new List<Tuple<Entity, string>>();
            Parallel.ForEach(children, c => IndexChild(c, newChildren));
            return newChildren;
        }

        private void IndexChild(Tuple<Entity, string> child, List<Tuple<Entity, string>> newChildren)
        {
            string childName = child.Item1.Original.NameDecorator.SystemName;
            string path = Path.Combine(child.Item2, childName);
            if (child.Item1.Original.ContainerDecorator != null)
            {
                var childChildren = api.GetChildren(child.Item1).Select(c =>
                    Tuple.Create(c, path)
                );
                var childChildrenList = childChildren.ToList();
                newChildren.AddRange(childChildrenList);

                DeleteUnavailableChildren(path, childChildrenList);
            }

            FileReaderWriter.CreateFile(path, child.Item1);
        }

        private void DeleteUnavailableChildren(string path, List<Tuple<Entity, string>> childChildren)
        {
            if (!Directory.Exists(path))
            {
                return;
            }
            var files = Directory.GetFiles(path);
            files = files.Except(Directory.GetFiles(path, "*.xillioEntity")).ToArray();

            files = files.Union(Directory.GetDirectories(path)).ToArray();
            
            //LogService.Log($"Found {files.Length} files and pulled {childChildren.Count()} for {path}");
            if (files.Length < childChildren.Count()) return;
            foreach (var file in files)
            {
                if (ExtractChildNames(childChildren).Contains(Path.GetFileName(file))) continue;
                LogService.Log($"Removing {file}");
                FileAttributes attr = File.GetAttributes(file);
                if ((attr & FileAttributes.Directory) == FileAttributes.Directory)
                {
                    Directory.Delete(file, true);
                }
                else
                {
                    File.Delete(file);
                }
            }
        }


        private void DeleteUnavailableConfigs(List<Configuration> configurations)
        {
            String[] allConfigDirs = Directory.GetDirectories(InfoHolder.syncFolder);

            List<string> configNames = configurations.Select(c => c.Name).ToList();

            foreach (var configDir in allConfigDirs)
            {
                if (!configNames.Contains(Path.GetFileName(configDir)))
                {
                    Directory.Delete(configDir, true);
                }
            }
        }


        private List<string> ExtractChildNames(IEnumerable<Tuple<Entity, string>> children)
        {
            List<string> childNames = new List<string>();
            foreach (var child in children)
            {
                childNames.Add(child.Item1.Original.NameDecorator.SystemName);
            }

            return childNames;
        }
    }
}