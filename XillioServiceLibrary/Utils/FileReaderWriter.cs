using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Timers;
using Newtonsoft.Json;
using XillioEngineSDK.model;

namespace XillioAPIService
{
    public static class FileReaderWriter
    {
        private static List<char> INVALID_WINDOWS_PATH_CHARACTERS =
            new List<char>() {':', '?', '*', '"', '<', '>', '|', '/'};

        private static PropertiesReaderWriter _propertiesReaderWriter = new PropertiesReaderWriter();

        public static void WriteEntityToDisk(string path, Entity entity)
        {
            path = MakeNameCompliant(path);

            if (File.Exists(path) || Directory.Exists(path)) return;

            if (entity.Original.FileDecorator != null)
            {
                path = CreateFile(path, entity);
            }
            else
            {
                CreateDirectory(path);
            }

            if (entity.Original.ContainerDecorator != null)
            {
                File.SetAttributes(path, FileAttributes.Directory);
            }

            //LogService.Log($"Now set the properties of the File at {path}");

            _propertiesReaderWriter.WriteEntityPropertyFile(path, entity);
            //LogService.Log($"creating {path} is done.");
        }

        public static void CreateConfigurationOnDisk(string path)
        {
            path = MakeNameCompliant(path);

            if (File.Exists(path)) return;

            CreateDirectory(path);
        }
        
        private static void CreateDirectory(string path)
        {
            Directory.CreateDirectory(path);
            Directory.CreateDirectory($"{path}/.xillioEntity");
            File.SetAttributes($"{path}/.xillioEntity", FileAttributes.Hidden);
        }

        private static string CreateFile(string path, Entity entity)
        {
            if (!path.Contains("."))
            {
                path = path + "." + entity.Original.FileDecorator.Extension;
            }

            try
            {
                LogService.Log($"Doing the actual create of {path}");
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
            catch (NotSupportedException e)
            {
                LogService.Log($"There was a problem with the path: {path}");
                throw;
            }

            File.SetAttributes(path, FileAttributes.Offline);
            return path;
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

        private static string MakeNameCompliant(string path)
        {
            int lastSlashIndex = path.LastIndexOf('\\');
            string relativePath = path.Substring(lastSlashIndex + 1);
            List<char> foundChars = INVALID_WINDOWS_PATH_CHARACTERS.Where(c => relativePath.Contains(c)).ToList();
            foreach (var character in foundChars)
            {
                relativePath = relativePath.Replace(character, ' ');
            }

            return Path.Combine(path.Substring(0, lastSlashIndex), relativePath);
        }
    }
}