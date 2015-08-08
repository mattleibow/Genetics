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
#pragma warning disable 0219, 0649
        static bool falseflag = false;
        static InjectClickAttribute()
        {
            if (falseflag)
            {
                var ignore = new View(null);
                ignore.Click += null;
            }
        }
#pragma warning restore 0219, 0649

        public InjectClickAttribute(int viewId)
            : base(viewId, "Click")
        {
        }
    }

    [AttributeUsage(AttributeTargets.Method)]
    public class InjectLongClickAttribute : InjectEventAttribute
    {
#pragma warning disable 0219, 0649
        static bool falseflag = false;
        static InjectLongClickAttribute()
        {
            if (falseflag)
            {
                var ignore = new View(null);
                ignore.LongClick += null;
            }
        }
#pragma warning restore 0219, 0649

        public InjectLongClickAttribute(int viewId)
            : base(viewId, "LongClick")
        {
        }
    }

    [AttributeUsage(AttributeTargets.Method)]
    public class InjectItemClickAttribute : InjectEventAttribute
    {
#pragma warning disable 0219, 0649
        static bool falseflag = false;
        static InjectItemClickAttribute()
        {
            if (falseflag)
            {
                var ignore = new ListView(null);
                ignore.ItemClick += null;
            }
        }
#pragma warning restore 0219, 0649

        public InjectItemClickAttribute(int viewId)
                : base(viewId, "ItemClick")
        {
        }
    }

    [AttributeUsage(AttributeTargets.Method)]
    public class InjectCheckedChangeAttribute : InjectEventAttribute
    {
#pragma warning disable 0219, 0649
        static bool falseflag = false;
        static InjectCheckedChangeAttribute()
        {
            if (falseflag)
            {
                var ignore = new CheckBox(null);
                ignore.CheckedChange += null;
            }
        }
#pragma warning restore 0219, 0649

        public InjectCheckedChangeAttribute(int viewId)
            : base(viewId, "CheckedChange")
        {
        }
    }

    [AttributeUsage(AttributeTargets.Method)]
    public class InjectItemLongClickAttribute : InjectEventAttribute
    {
#pragma warning disable 0219, 0649
        static bool falseflag = false;
        static InjectItemLongClickAttribute()
        {
            if (falseflag)
            {
                var ignore = new ListView(null);
                ignore.ItemLongClick += null;
            }
        }
#pragma warning restore 0219, 0649

        public InjectItemLongClickAttribute(int viewId)
            : base(viewId, "ItemLongClick")
        {
        }
    }

    [AttributeUsage(AttributeTargets.Method)]
    public class InjectItemSelectedAttribute : InjectEventAttribute
    {
#pragma warning disable 0219, 0649
        static bool falseflag = false;
        static InjectItemSelectedAttribute()
        {
            if (falseflag)
            {
                var ignore = new ListView(null);
                ignore.ItemSelected += null;
            }
        }
#pragma warning restore 0219, 0649

        public InjectItemSelectedAttribute(int viewId)
            : base(viewId, "ItemSelected")
        {
        }
    }

    [AttributeUsage(AttributeTargets.Method)]
    public class InjectTextChangedAttribute : InjectEventAttribute
    {
#pragma warning disable 0219, 0649
        static bool falseflag = false;
        static InjectTextChangedAttribute()
        {
            if (falseflag)
            {
                var ignore = new TextView(null);
                ignore.TextChanged += null;
            }
        }
#pragma warning restore 0219, 0649

        public InjectTextChangedAttribute(int viewId)
            : base(viewId, "TextChanged")
        {
        }
    }

    [AttributeUsage(AttributeTargets.Method)]
    public class InjectTouchAttribute : InjectEventAttribute
    {
#pragma warning disable 0219, 0649
        static bool falseflag = false;
        static InjectTouchAttribute()
        {
            if (falseflag)
            {
                var ignore = new View(null);
                ignore.Touch += null;
            }
        }
#pragma warning restore 0219, 0649

        public InjectTouchAttribute(int viewId)
            : base(viewId, "Touch")
        {
        }
    }

    [AttributeUsage(AttributeTargets.Method)]
    public class InjectFocusChangeAttribute : InjectEventAttribute
    {
#pragma warning disable 0219, 0649
        static bool falseflag = false;
        static InjectFocusChangeAttribute()
        {
            if (falseflag)
            {
                var ignore = new View(null);
                ignore.FocusChange += null;
            }
        }
#pragma warning restore 0219, 0649

        public InjectFocusChangeAttribute(int viewId)
            : base(viewId, "FocusChange")
        {
        }
    }

    [AttributeUsage(AttributeTargets.Method)]
    public class InjectEditorActionAttribute : InjectEventAttribute
    {
#pragma warning disable 0219, 0649
        static bool falseflag = false;
        static InjectEditorActionAttribute()
        {
            if (falseflag)
            {
                var ignore = new TextView(null);
                ignore.EditorAction += null;
            }
        }
#pragma warning restore 0219, 0649

        public InjectEditorActionAttribute(int viewId)
            : base(viewId, "EditorAction")
        {
        }
    }
}
