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
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Genetics.Genes
{
    public abstract class ArrayResourceGene : IGene
    {
        public virtual bool Splice(object target, object source, string resourceType, int resourceId, Context context, MemberMapping memberMapping)
        {
            var value = GetArrayValue(context.Resources, resourceId, memberMapping.MemberType);
            if (value != null)
            {
                value = memberMapping.MemberType.CreateEnumerable(value);
                memberMapping.SetterMethod(target, value);
            }
            return value != null;
        }
        
        public void Sever(object target, object source, string resourceType, int resourceId, Context context, MemberMapping memberMapping)
        {
        }

        public abstract IEnumerable GetArrayValue(Resources resources, int resourceId, Type memberType);
    }

    public class IntegerArrayGene : ArrayResourceGene
    {
        public override IEnumerable GetArrayValue(Resources resources, int resourceId, Type memberType)
        {
            IEnumerable collection = null;

            // get the type fromthe member
            var elementType = memberType.GetEnumerableElementType();
            if (elementType != null)
            {
                if (elementType == typeof(int))
                    collection = resources.GetIntArray(resourceId);
                else if (elementType == typeof(string))
                    collection = resources.GetStringArray(resourceId);
            }

            // we don't know how to handle collections without a type
            return collection;
        }
    }

    public class StringArrayGene : SimpleResourceGene
    {
        public override object GetValue(Resources resources, int resourceId, Type memberType)
        {
            return resources.GetStringArray(resourceId);
        }
    }
}
