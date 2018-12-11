using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Timers;
using XillioEngineSDK.model;

namespace XillioAPIService
{
    public static class FileReaderWriter
    {
        public static void CreateFile(string path, Entity entity)
        {
            if (File.Exists(path)) return;

            var attributes = new List<FileAttributes>();
            if (entity.Original.ContainerDecorator != null) attributes.Add(FileAttributes.Directory);

            if (entity.Original.FileDecorator != null)
            {
                if (!path.Contains(".")) path = path + "." + entity.Original.FileDecorator.Extension;

                try
                {
                    //LogService.Log($"Doing the actual create of {path}");
                    File.Create(path).Close();
                }
                catch (IOException e)
                {
                    var timer = new Timer();
                    timer.AutoReset = false;
                    timer.Interval = 20000;
                    timer.Enabled = true;
                    timer.Elapsed += delegate { timer.Dispose(); };
                    while (!IsFileReady(path) && timer.Enabled)
                    {
                    }

                    File.Create(path).Close();
                }

                attributes.Add(FileAttributes.Offline);
            }
            else
            {
                Directory.CreateDirectory(path);
            }

            //LogService.Log($"Now set the properties of the File at {path}");

            WriteCustomProperty(path, "XDIP", entity.Xdip);
            SetMultipleFileAttributes(path, attributes);
            //LogService.Log($"creating {path} is done.");
        }

        public static Entity ReadFile(string path, bool isFolder)
        {
            //Create Entity and create Decorators
            var entity = new Entity();
            //entity.Original.NameDecorator = new NameDecorator(Path.GetFileName(path));

            //Fill in data from custom properties


            //after making changes, you need to use this line to save them


            return entity;
        }

        private static void SetMultipleFileAttributes(string path, List<FileAttributes> attributes)
        {
            switch (attributes.Count)
            {
                case 0:
                    break;
                case 1:
                    File.SetAttributes(path, attributes[0]);
                    break;
                case 2:
                    File.SetAttributes(path, attributes[0] | attributes[1]);
                    break;
                case 3:
                    File.SetAttributes(path, attributes[0] | attributes[1] | attributes[2]);
                    break;
                default:
                    throw new ArgumentException("No more than 3 attributes are allowed. And no less then 0.");
            }
        }

        private static bool IsFileReady(string filename)
        {
            // If the file can be opened for exclusive access it means that the file
            // is no longer locked by another process.
            try
            {
                using (var inputStream = File.Open(filename, FileMode.Create, FileAccess.Write, FileShare.None))
                {
                    return true;
                }
            }
            catch (IOException)
            {
                return false;
            }
        }

        private static void WriteCustomProperty(string path, string propertyName, string propertyValue)
        {
            var propertiesPath = path + ".properties";
            var property = Encoding.ASCII.GetBytes(propertyName + " : " + propertyValue);
            if (!File.Exists(propertiesPath))
                File.WriteAllBytes(propertiesPath, property);
            else
                using (var stream = File.OpenWrite(propertiesPath))
                {
                    stream.Write(property, 0, property.Length);
                }

            File.SetAttributes(propertiesPath, FileAttributes.Hidden | FileAttributes.NotContentIndexed);
        }
    }
}