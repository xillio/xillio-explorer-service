using System.Runtime.Serialization;

namespace XillioEngineSDK.model.decorators
{
    public class ContainerDecorator : Decorator
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