using System;

namespace Genetics.Attributes
{
    /// <summary>
    /// Splice the specified view or resource into the field or property.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
    public class SpliceAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SpliceAttribute"/> class.
        /// </summary>
        /// <param name="resourceId">The Android view or resource ID.</param>
        public SpliceAttribute(int resourceId)
        {
            ResourceId = resourceId;
            Optional = false;
            //Collection = null;
        }

        /// <summary>
        /// Gets the Android view or resource ID.
        /// </summary>
        /// <value>The Android view or resource ID.</value>
        public int ResourceId { get; private set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="SpliceAttribute"/> is optional.
        /// </summary>
        /// <value><c>true</c> if optional; otherwise, <c>false</c>.</value>
        public bool Optional { get; set; }

        //public Type Collection { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the value should be disposed when Sever is invoked.
        /// </summary>
        /// <value><c>true</c> if the value should be disposed when Sever is invoked; otherwise, <c>false</c>.</value>
        /// <remarks>
        /// In the case of <see cref="Android.Graphics.Bitmap"/>, the bitmap is recycled. 
        /// And, an <see cref="System.Xml.XmlReader"/> is closed.
        /// </remarks>
        /// <seealso cref="Geneticist.Sever(Android.App.Activity)"/>
        /// <seealso cref="Geneticist.Sever(Android.App.Dialog)"/>
        /// <seealso cref="Geneticist.Sever(Android.Views.View)"/>
        /// <seealso cref="Geneticist.Sever(object, Android.App.Activity)"/>
        /// <seealso cref="Geneticist.Sever(object, Android.App.Dialog)"/>
        /// <seealso cref="Geneticist.Sever(object, Android.Views.View)"/>
        /// <seealso cref="Geneticist.Sever(object, Android.Content.Context)"/>
        /// <seealso cref="Geneticist.Sever(object, object, Android.Content.Context)"/>
        /// <seealso cref="Geneticist.Sever{T}(object, T, Func{T, Android.Content.Context})"/>
        public bool DisposeOnSever { get; set; }
    }
}
