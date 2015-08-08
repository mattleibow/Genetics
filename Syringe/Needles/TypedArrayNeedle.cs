using System;
using System.Xml;
using System.Xml.Linq;
using Android.Animation;
using Android.App;
using Android.Content;
using Android.Content.Res;
using Android.Graphics;
using Android.Runtime;
using Android.Views;
using Android.Views.Animations;
using Org.XmlPull.V1;

using Genetics.Mappings;

namespace Genetics.Genes
{
    public class TypedArrayGene : SimpleResourceGene
    {
        public override object GetValue(Resources resources, int resourceId, Type memberType)
        {
            return resources.ObtainTypedArray(resourceId);
        }

        public override void Sever(object target, object source, string resourceType, int resourceId, Context context, MemberMapping memberMapping)
        {
            if (memberMapping.Attribute.DisposeOnSever)
            {
                var typedArray = memberMapping.GetterMethod(target) as TypedArray;
                if (typedArray != null)
                {
                    typedArray.Recycle();
                    typedArray.Dispose();
                }
            }
            memberMapping.SetterMethod(target, null);
        }
    }
}
