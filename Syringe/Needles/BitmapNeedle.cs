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

    public class BitmapNeedle : INeedle
    {
        public bool Inject(object target, object source, string resourceType, int resourceId, Context context, MemberMapping memberMapping)
        {
            Bitmap bitmap = null;
            try
            {
                bitmap = BitmapFactory.DecodeResource(context.Resources, resourceId);
                memberMapping.SetterMethod(target, bitmap);
            }
            catch (Exception exception)
            {
                DisposeBitmap(bitmap);
                bitmap = null;

                Needle.HandleError(
                    exception,
                    "Unable to inject resource '{0}' with id '{1}' to member '{2}'.",
                    context.Resources.GetResourceName(resourceId),
                    resourceId,
                    memberMapping.Member.Name);
            }
            return bitmap != null;
        }

        public void Withdraw(object target, object source, string resourceType, int resourceId, Context context, MemberMapping memberMapping)
        {
            if (memberMapping.Attribute.DisposeOnWithdraw)
            {
                var bitmap = memberMapping.GetterMethod(target) as Bitmap;
                DisposeBitmap(bitmap);
            }
            memberMapping.SetterMethod(target, null);
        }

        private static void DisposeBitmap(Bitmap bitmap)
        {
            if (bitmap != null)
            {
                if (!bitmap.IsRecycled)
                {
                    bitmap.Recycle();
                }
                bitmap.Dispose();
            }
        }
    }
    
}
