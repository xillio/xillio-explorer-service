namespace XillioEngineSDK.model.decorators
{
    public class ContainerDecorator
    {
        public bool HasChildren { get; set; }

        public ContainerDecorator()
        {
        }

        public ContainerDecorator(bool hasChildren)
        {
            HasChildren = hasChildren;
        }
    }
}