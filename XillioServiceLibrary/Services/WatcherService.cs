using System;
using System.IO;
using XillioEngineSDK;
using XillioEngineSDK.model;
using XillioEngineSDK.model.decorators;

namespace XillioAPIService
{
    public class WatcherService : IService
    {
        private XillioApi api;
        private FileSystemWatcher watcher;

        public WatcherService(XillioApi api)
        {
            this.api = api;
            Start();
        }

        public void Start()
        {
            LogService.Log("starting WatcherService");
            watcher = new FileSystemWatcher();
            ((System.ComponentModel.ISupportInitialize) (watcher)).BeginInit();

            //Add handlers
            watcher.Renamed += HandleRename;
            watcher.Created += HandleCreate;
            watcher.Deleted += HandleDelete;
            watcher.Changed += HandleChange;

            watcher.EnableRaisingEvents = true;
            ((System.ComponentModel.ISupportInitialize) (watcher)).EndInit();
            LogService.Log("Watcher started up.");
        }

        public void Pause()
        {
            watcher.EnableRaisingEvents = false;
        }

        public void Resume()
        {
            watcher.EnableRaisingEvents = true;
        }

        public void Stop()
        {
            watcher.Dispose();
        }

        private void HandleCreate(object sender, FileSystemEventArgs args)
        {
            string extension = Path.GetExtension(args.FullPath);
            //TODO get all info from file


            Entity entity = new Entity();
            //entity.Original.NameDecorator = new NameDecorator(args.Name);
            //TODO fix this
            entity.Original.ContainerDecorator = new ContainerDecorator(Directory.Exists(args.FullPath));

            using (FileStream stream = File.OpenRead(args.FullPath))
            {
                //api.CreateEntity(entity, GetConfiguration(args.FullPath), stream);
            }
        }

        private void HandleRename(object sender, RenamedEventArgs args)
        {
            throw new NotImplementedException();
        }

        private void HandleDelete(object sender, FileSystemEventArgs args)
        {
            api.DeleteEntity(GetConfiguration(args.FullPath), args.FullPath.Substring(InfoHolder.syncFolder.Length));
        }

        private void HandleChange(object sender, FileSystemEventArgs args)
        {
            throw new NotImplementedException();
        }

        private Configuration GetConfiguration(string path)
        {
            //No -1 because last / needs to be removed.
            string relativePath = path.Substring(InfoHolder.syncFolder.Length).Split(Path.DirectorySeparatorChar)[0];
            return InfoHolder.Configurations[relativePath].Item1;
        }
    }
}