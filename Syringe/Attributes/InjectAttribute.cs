using System;

namespace Genetics.Attributes
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
    public class SpliceAttribute : Attribute
    {
        public SpliceAttribute(int resourceId)
        {
            ResourceId = resourceId;
            Optional = false;
            Collection = null;
        }

        public int ResourceId { get; private set; }

        public bool Optional { get; set; }

        public Type Collection { get; set; }

        public bool DisposeOnSever { get; set; }
    }
}
