using System.IO;

namespace XillioAPIService
{
    public class UpdateService
    {
        public void HandleFileChanges(object sender, FileSystemEventArgs e)
        {
            //a file in the directory has changed.
            LogService.Log($"The file {e.FullPath} has changed.");

            switch (e.ChangeType)
            {
                case WatcherChangeTypes.Created:
                    break;
                case WatcherChangeTypes.Deleted:
                    break;
                case WatcherChangeTypes.Renamed:
                    break;
                case WatcherChangeTypes.Changed:
                    break;
                default:
                    //cannot be reached
                    return;
            }
        }
    }
}