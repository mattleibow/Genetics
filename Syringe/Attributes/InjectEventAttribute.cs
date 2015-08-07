using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System.Reflection;
using Syringe.Mappings;

namespace Syringe.Attributes
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class InjectEventAttribute : Attribute
    {
        public InjectEventAttribute(int viewId, string eventName)
        {
            ViewId = viewId;
            EventName = eventName;
            Optional = false;
        }

        public int ViewId { get; set; }

        public string EventName { get; set; }

        public bool Optional { get; set; }
    }

    [AttributeUsage(AttributeTargets.Method)]
    public class InjectClickAttribute : InjectEventAttribute
    {
        public InjectClickAttribute(int viewId)
            : base(viewId, "Click")
        {
        }
    }

    [AttributeUsage(AttributeTargets.Method)]
    public class InjectLongClickAttribute : InjectEventAttribute
    {
        public InjectLongClickAttribute(int viewId)
            : base(viewId, "LongClick")
        {
        }
    }

    [AttributeUsage(AttributeTargets.Method)]
    public class InjectItemClickAttribute : InjectEventAttribute
    {
        public InjectItemClickAttribute(int viewId)
            : base(viewId, "ItemClick")
        {
        }
    }

    [AttributeUsage(AttributeTargets.Method)]
    public class InjectCheckedChangeAttribute : InjectEventAttribute
    {
        public InjectCheckedChangeAttribute(int viewId)
            : base(viewId, "CheckedChange")
        {
        }
    }

    [AttributeUsage(AttributeTargets.Method)]
    public class InjectItemLongClickAttribute : InjectEventAttribute
    {
        public InjectItemLongClickAttribute(int viewId)
            : base(viewId, "ItemLongClick")
        {
        }
    }

    [AttributeUsage(AttributeTargets.Method)]
    public class InjectItemSelectedAttribute : InjectEventAttribute
    {
        public InjectItemSelectedAttribute(int viewId)
            : base(viewId, "ItemSelected")
        {
        }
    }

    [AttributeUsage(AttributeTargets.Method)]
    public class InjectTextChangedAttribute : InjectEventAttribute
    {
        public InjectTextChangedAttribute(int viewId)
            : base(viewId, "TextChanged")
        {
        }
    }

    [AttributeUsage(AttributeTargets.Method)]
    public class InjectTouchAttribute : InjectEventAttribute
    {
        public InjectTouchAttribute(int viewId)
            : base(viewId, "Touch")
        {
        }
    }

    [AttributeUsage(AttributeTargets.Method)]
    public class InjectFocusChangeAttribute : InjectEventAttribute
    {
        public InjectFocusChangeAttribute(int viewId)
            : base(viewId, "FocusChange")
        {
        }
    }

    [AttributeUsage(AttributeTargets.Method)]
    public class InjectEditorActionAttribute : InjectEventAttribute
    {
        public InjectEditorActionAttribute(int viewId)
            : base(viewId, "EditorAction")
        {
        }
    }
}
