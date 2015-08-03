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
    public abstract class SimpleResourceNeedle : INeedle
    {
        public virtual bool Inject(object target, object source, string resourceType, int resourceId, Context context, MemberMapping memberMapping)
        {
            var value = GetValue(context.Resources, resourceId, memberMapping.MemberType);
            if (value != null)
            {
                memberMapping.SetterMethod(target, value);
            }
            return value != null;
        }

        public virtual void Withdraw(object target, object source, string resourceType, int resourceId, Context context, MemberMapping memberMapping)
        {
        }

        public abstract object GetValue(Resources resources, int resourceId, Type memberType);
    }
}
