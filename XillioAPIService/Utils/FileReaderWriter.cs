using System.IO;
using System.Security;
using DSOFile;
using XillioEngineSDK.model;
using XillioEngineSDK.model.decorators;

namespace XillioAPIService
{
    public class FileReaderWriter
    {
        public void CreateFile(string path, Entity entity)
        {
            //creates new class of oledocumentproperties
            var doc = new OleDocumentPropertiesClass();

            //open your selected file
            doc.Open(path);

            //you can set properties with summaryproperties.nameOfProperty = value; for example.
            
            
            //you can put metadata that does not fit in summaryProperties inside of a custom property.
            doc.CustomProperties.Add("XDIP", entity.Id);

            //after making changes, you need to use this line to save them
            doc.Save();
        }

        public Entity ReadFile(string path)
        {
            //creates new class of oledocumentproperties
            var doc = new OleDocumentPropertiesClass();

            //open your selected file
            doc.Open(path);
            
            //Create Entity and create Decorators
            Entity entity = new Entity();
            entity.Original.NameDecorator = new NameDecorator(Path.GetFileName(path));
            
            //Fill in additional data from properties
            entity.Original.NameDecorator.DisplayName = doc.SummaryProperties.Title;

            //Fill in data from custom properties
            foreach (CustomProperty property in doc.CustomProperties)
            {
                if (property.Name.Equals("XDIP"))
                {
                    LogService.Log(property.get_Value().ToString());
                    entity.Id = property.get_Value().ToString();
                }
            }
            //after making changes, you need to use this line to save them
            doc.Close();
            return entity;
        }
    }
}