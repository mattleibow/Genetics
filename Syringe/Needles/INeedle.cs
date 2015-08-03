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

using Syringe.Mappings;

namespace Syringe.Needles
{
    public interface INeedle
    {
        bool Inject(object target, object source, string resourceType, int resourceId, Context context, MemberMapping memberMapping);

        void Withdraw(object target, object source, string resourceType, int resourceId, Context context, MemberMapping memberMapping);
    }
}
