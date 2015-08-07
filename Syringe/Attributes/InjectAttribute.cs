using System;

namespace Syringe.Attributes
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
    public class InjectAttribute : Attribute
    {
        public InjectAttribute(int resourceId)
        {
            ResourceId = resourceId;
            Optional = false;
            Collection = null;
        }

        public int ResourceId { get; private set; }

        public bool Optional { get; set; }

        public Type Collection { get; set; }

        public bool DisposeOnWithdraw { get; set; }
    }
}
