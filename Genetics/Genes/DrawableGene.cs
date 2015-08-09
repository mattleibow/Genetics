using System;
using System.Xml;
using System.Xml.Linq;
using Android.Animation;
using Android.App;
using Android.Content;
using Android.Content.Res;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Runtime;
using Android.Views;
using Android.Views.Animations;
using Org.XmlPull.V1;

using Genetics.Mappings;

namespace Genetics.Genes
{
    public class DrawableGene : SimpleResourceGene
    {
        public override object GetValue(Resources resources, int resourceId, Type memberType)
        {
            return resources.GetDrawable(resourceId);
        }

        public override void Sever(object target, object source, string resourceType, int resourceId, Context context, MemberMapping memberMapping)
        {
            if (memberMapping.Attribute.DisposeOnSever)
            {
                var drawable = memberMapping.GetterMethod(target) as Drawable;
                if (drawable != null)
                {
                    drawable.Dispose();
                }
            }
            memberMapping.SetterMethod(target, null);
        }
    }
}
