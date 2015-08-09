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
    public interface IGene
    {
        bool Splice(object target, object source, string resourceType, int resourceId, Context context, MemberMapping memberMapping);

        void Sever(object target, object source, string resourceType, int resourceId, Context context, MemberMapping memberMapping);
    }
}
