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
    public class ViewNeedle : INeedle
    {
		private static readonly Type javaObjectType;

		static ViewNeedle()
		{
			javaObjectType = typeof(IJavaObject);
		}

        public bool Inject(object target, object source, string resourceType, int resourceId, Context context, MemberMapping memberMapping)
        {
            var assigned = false;
            var value = FindViewById(source, resourceId);
            if (value != null)
            {
                var valueType = value.GetType();
                var memberType = memberMapping.MemberType;

                if (memberType.IsAssignableFrom(valueType))
                {
                    // first see if it a direct inheritance
                    memberMapping.SetterMethod(target, value);
                    assigned = true;
                }
                else if (javaObjectType.IsAssignableFrom(memberType) && javaObjectType.IsAssignableFrom(memberType))
                {
                    // it may be inheritance inside Java
                    memberMapping.SetterMethod(target, value.JavaCast(memberType));
                    assigned = true;
                }
            }
            return assigned;
        }

        public void Withdraw(object target, object source, string resourceType, int resourceId, Context context, MemberMapping memberMapping)
        {
            memberMapping.SetterMethod(target, null);
        }

        private View FindViewById(object source, int resourceId)
        {
            var activity = source as Activity;
            if (activity != null)
            {
                return activity.FindViewById(resourceId);
            }

            var view = source as View;
            if (view != null)
            {
                return view.FindViewById(resourceId);
            }

            var dialog = source as Dialog;
            if (dialog != null)
            {
                return dialog.FindViewById(resourceId);
            }

            Needle.HandleError(
                "Unknown view container type '{0}'.",
                source.GetType().FullName);

            return null;
        }
    }
}
