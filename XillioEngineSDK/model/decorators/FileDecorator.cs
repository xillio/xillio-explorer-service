namespace XillioEngineSDK.model.decorators
{
    public class FileDecorator
    {
        public string extension { get; set; }
        public string rawExtension { get; set; }
        public long size {get; set;} 

        public FileDecorator()
        {
        }

        public FileDecorator(string extension)
        {
            Extension = extension;
        }

        public FileDecorator(string extension, string rawextension, long size)
        {
            Extension = extension;
            RawExtension = rawExtension;
            Size = size;
        }
    }
}