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
    public class ColorGene : IGene
    {
        public bool Splice(object target, object source, string resourceType, int resourceId, Context context, MemberMapping memberMapping)
        {
            var assigned = false;
            var value = context.Resources.GetColor(resourceId);
            if (memberMapping.MemberType.IsAssignableFrom(typeof(Color)))
            {
                memberMapping.SetterMethod(target, value);
                assigned = true;
            }
            else if (memberMapping.MemberType.IsAssignableFrom(typeof(System.Drawing.Color)))
            {
                memberMapping.SetterMethod(target, System.Drawing.Color.FromArgb(value.ToArgb()));
                assigned = true;
            }
            return assigned;
        }

        public void Sever(object target, object source, string resourceType, int resourceId, Context context, MemberMapping memberMapping)
        {
        }
    }
}
