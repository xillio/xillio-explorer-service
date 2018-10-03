namespace XillioEngineSDK.model.decorators
{
    public class NameDecorator
    {
        public string SystemName { get; set; }
        public string DisplayName { get; set; }

        public NameDecorator()
        {
        }

        public NameDecorator(string systemName)
        {
            SystemName = systemName;
        }

        public NameDecorator(string systemName, string displayName)
        {
            SystemName = systemName;
            DisplayName = displayName;
        }
    }
}