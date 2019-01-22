using System.Runtime.Serialization;

namespace XillioEngineSDK.model.decorators
{
    public class FileDecorator : Decorator
    {
        public string Extension { get; set; }
        public string RawExtension { get; set; }
        public long Size {get; set;} 

        public FileDecorator()
        {
        }

        public FileDecorator(string extension)
        {
            Extension = extension;
        }

        public FileDecorator(string extension, string rawExtension, long size)
        {
            Extension = extension;
            RawExtension = rawExtension;
            Size = size;
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("extension", Extension);
            info.AddValue("rawExtension", RawExtension);
            info.AddValue("size", Size);
        }
    }
}