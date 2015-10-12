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
using Genetics.Mappings;

namespace Genetics.Attributes
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class SpliceEventAttribute : Attribute
    {
        public SpliceEventAttribute(int viewId, string eventName)
        {
            ViewId = viewId;
            EventName = eventName;
            Optional = false;
        }

        public int ViewId { get; set; }

        public string EventName { get; set; }

        public bool Optional { get; set; }
    }

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class SpliceClickAttribute : SpliceEventAttribute
    {
#pragma warning disable 0219, 0649
        static bool falseflag = false;
        static SpliceClickAttribute()
        {
            if (falseflag)
            {
                var ignore = new View(null);
                ignore.Click += null;
            }
        }
#pragma warning restore 0219, 0649

        public SpliceClickAttribute(int viewId)
            : base(viewId, "Click")
        {
        }
    }

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class SpliceLongClickAttribute : SpliceEventAttribute
    {
#pragma warning disable 0219, 0649
        static bool falseflag = false;
        static SpliceLongClickAttribute()
        {
            if (falseflag)
            {
                var ignore = new View(null);
                ignore.LongClick += null;
            }
        }
#pragma warning restore 0219, 0649

        public SpliceLongClickAttribute(int viewId)
            : base(viewId, "LongClick")
        {
        }
    }

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class SpliceItemClickAttribute : SpliceEventAttribute
    {
#pragma warning disable 0219, 0649
        static bool falseflag = false;
        static SpliceItemClickAttribute()
        {
            if (falseflag)
            {
                var ignore = new ListView(null);
                ignore.ItemClick += null;
            }
        }
#pragma warning restore 0219, 0649

        public SpliceItemClickAttribute(int viewId)
                : base(viewId, "ItemClick")
        {
        }
    }

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class SpliceCheckedChangeAttribute : SpliceEventAttribute
    {
#pragma warning disable 0219, 0649
        static bool falseflag = false;
        static SpliceCheckedChangeAttribute()
        {
            if (falseflag)
            {
                var ignore = new CheckBox(null);
                ignore.CheckedChange += null;
            }
        }
#pragma warning restore 0219, 0649

        public SpliceCheckedChangeAttribute(int viewId)
            : base(viewId, "CheckedChange")
        {
        }
    }

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class SpliceItemLongClickAttribute : SpliceEventAttribute
    {
#pragma warning disable 0219, 0649
        static bool falseflag = false;
        static SpliceItemLongClickAttribute()
        {
            if (falseflag)
            {
                var ignore = new ListView(null);
                ignore.ItemLongClick += null;
            }
        }
#pragma warning restore 0219, 0649

        public SpliceItemLongClickAttribute(int viewId)
            : base(viewId, "ItemLongClick")
        {
        }
    }

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class SpliceItemSelectedAttribute : SpliceEventAttribute
    {
#pragma warning disable 0219, 0649
        static bool falseflag = false;
        static SpliceItemSelectedAttribute()
        {
            if (falseflag)
            {
                var ignore = new ListView(null);
                ignore.ItemSelected += null;
            }
        }
#pragma warning restore 0219, 0649

        public SpliceItemSelectedAttribute(int viewId)
            : base(viewId, "ItemSelected")
        {
        }
    }

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class SpliceTextChangedAttribute : SpliceEventAttribute
    {
#pragma warning disable 0219, 0649
        static bool falseflag = false;
        static SpliceTextChangedAttribute()
        {
            if (falseflag)
            {
                var ignore = new TextView(null);
                ignore.TextChanged += null;
            }
        }
#pragma warning restore 0219, 0649

        public SpliceTextChangedAttribute(int viewId)
            : base(viewId, "TextChanged")
        {
        }
    }

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class SpliceTouchAttribute : SpliceEventAttribute
    {
#pragma warning disable 0219, 0649
        static bool falseflag = false;
        static SpliceTouchAttribute()
        {
            if (falseflag)
            {
                var ignore = new View(null);
                ignore.Touch += null;
            }
        }
#pragma warning restore 0219, 0649

        public SpliceTouchAttribute(int viewId)
            : base(viewId, "Touch")
        {
        }
    }

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class SpliceFocusChangeAttribute : SpliceEventAttribute
    {
#pragma warning disable 0219, 0649
        static bool falseflag = false;
        static SpliceFocusChangeAttribute()
        {
            if (falseflag)
            {
                var ignore = new View(null);
                ignore.FocusChange += null;
            }
        }
#pragma warning restore 0219, 0649

        public SpliceFocusChangeAttribute(int viewId)
            : base(viewId, "FocusChange")
        {
        }
    }

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class SpliceEditorActionAttribute : SpliceEventAttribute
    {
#pragma warning disable 0219, 0649
        static bool falseflag = false;
        static SpliceEditorActionAttribute()
        {
            if (falseflag)
            {
                var ignore = new TextView(null);
                ignore.EditorAction += null;
            }
        }
#pragma warning restore 0219, 0649

        public SpliceEditorActionAttribute(int viewId)
            : base(viewId, "EditorAction")
        {
        }
    }
}
