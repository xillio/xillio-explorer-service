using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Timers;
using DSOFile;
using XillioEngineSDK.model;
using XillioEngineSDK.model.decorators;

namespace XillioAPIService
{
    public static class FileReaderWriter
    {
        public static void CreateFile(string path, Entity entity)
        {
            LogService.Log($"creating a file in {path}");
            //creates new class of oledocumentproperties
            var doc = new OleDocumentPropertiesClass();
            
            List<FileAttributes> attributes = new List<FileAttributes>();
            if (entity.Original.ContainerDecorator != null)
            {
                attributes.Add(FileAttributes.Directory);
            }
            if (entity.Original.FileDecorator != null)
            {
                path = path + "." + entity.Original.FileDecorator.Extension;
                try
                {
                    File.Create(path).Close();
                }
                catch (IOException e)
                {
                    /*
                Timer timer = new Timer();
                    timer.AutoReset = false;
                    timer.Interval = 20000;
                    timer.Enabled = true;
                    timer.Elapsed += delegate { timer.Dispose(); }; 
                    while (!IsFileReady(path) && timer.Enabled)
                    {
                    }
                    File.Create(path).Close();
                    */
                }

                //attributes.Add(FileAttributes.Offline);
            }
            else
            {
                Directory.CreateDirectory(path);
            }

            //open your selected file
            doc.Open(path);

            //you can set properties with summaryproperties.nameOfProperty = value; for example.


            //you can put metadata that does not fit in summaryProperties inside of a custom property.
            try
            {
                CustomProperty xdip = doc.CustomProperties["XDIP"];
                xdip.set_Value(entity.Id);
            }
            catch (COMException e)
            {
                if (e.Message.Contains("does not exist"))
                {
                    doc.CustomProperties.Add("XDIP", entity.Id);
                }
                else
                {
                    throw e;
                }
            }

            //after making changes, you need to use this line to save them
            doc.Save();
            SetMultipleFileAttributes(path, attributes);
            LogService.Log($"creating {path} is done.");
        }

        public static Entity ReadFile(string path, bool isFolder)
        {
            //creates new class of oledocumentproperties
            var doc = new OleDocumentPropertiesClass();

            //open your selected file
            doc.Open(path);

            //Create Entity and create Decorators
            Entity entity = new Entity();
            //entity.Original.NameDecorator = new NameDecorator(Path.GetFileName(path));

            //Fill in additional data from properties
            entity.Original.NameDecorator.DisplayName = doc.SummaryProperties.Title;

            //Fill in data from custom properties
            foreach (CustomProperty property in doc.CustomProperties)
            {
                if (property.Name.Equals("XDIP"))
                {
                    // ReSharper disable once UseIndexedProperty
                    LogService.Log(property.get_Value().ToString());
                    // ReSharper disable once UseIndexedProperty
                    entity.Id = property.get_Value().ToString();
                }
            }

            //after making changes, you need to use this line to save them
            doc.Close();
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
                using (FileStream inputStream = File.Open(filename, FileMode.Create, FileAccess.Write, FileShare.None))
                    return true;
            }
            catch (IOException)
            {
                return false;
            }
        }
    }
}