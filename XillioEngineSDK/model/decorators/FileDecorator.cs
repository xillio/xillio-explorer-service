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
    }
}