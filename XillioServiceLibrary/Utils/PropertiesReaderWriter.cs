using System;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using XillioEngineSDK.model;

namespace XillioAPIService
{
    public class PropertiesReaderWriter
    {
        public void WriteEntityPropertyFile(string path, Entity entity)
        {
            //Encode the entity
            Byte[] entityBytes = Encoding.ASCII.GetBytes(JsonConvert.SerializeObject(entity));

            var propertiesPath = $"{Directory.GetParent(path).FullName}/.xillioEntity/{Path.GetFileName(path)}";

            using (var stream = File.Open(propertiesPath, FileMode.OpenOrCreate))
            {
                stream.Write(entityBytes, 0, entityBytes.Length);
            }

            File.SetAttributes(propertiesPath, FileAttributes.Hidden | FileAttributes.NotContentIndexed);
        }

        public static Entity ReadFile(string path, bool isFolder)
        {
            throw new NotImplementedException();
            //Create Entity and create Decorators
            

            //Fill in data from custom properties


            //after making changes, you need to use this line to save them

        }
    }
}