using System;

namespace Syringe
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
    public class InjectAttribute : Attribute
    {
        public InjectAttribute(params int[] resourceIds)
        {
            ResourceIds = resourceIds;
            Optional = false;
            Collection = null;
            //Type = ResourceType.Automatic;
        }

        public int[] ResourceIds { get; private set; }

        public bool Optional { get; set; }

        public Type Collection { get; set; }

        //public ResourceType Type { get; set; }

        public bool DisposeOnWithdraw { get; set; }
    }

    //  public enum ResourceType
    //  {
    //      Automatic,
    //      View,
    //
    //      Animation,
    //      Animator,
    //      Boolean,
    //      Bitmap,
    //      Color,
    //      ColorStateList,
    //      Dimension,
    //      Drawable,
    //      Integer,
    //      IntegerArray,
    //      String,
    //      StringArray,
    //      TypedArray,
    //      Xml
    //  }
}
