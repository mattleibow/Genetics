using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Android.App;
using Android.Content;
using Android.Views;

using Syringe.Mappings;
using Syringe.Needles;
using Syringe.EventNeedles;

namespace Syringe
{
    public static class Needle
    {
        private readonly static Dictionary<string, Dictionary<Type, INeedle>> needles;
        private readonly static Dictionary<Type, IEventNeedle> eventNeedles;
        private readonly static Dictionary<Type, TypeMapping> typeMappings;

        static Needle()
        {
            typeMappings = new Dictionary<Type, TypeMapping>();
            needles = new Dictionary<string, Dictionary<Type, INeedle>>();
            eventNeedles = new Dictionary<Type, IEventNeedle>();
            ThrowOnError = true;
            Debug = false;
            DebugTextWriter = null;

            RegisterDefaultNeedles();
            RegisterDefaultEventNeedles();
        }

        public static Type[] MappedTypes
        {
            get { return typeMappings.Keys.ToArray(); }
        }

        public static TypeMapping[] Mappings
        {
            get { return typeMappings.Values.ToArray(); }
        }

        public static bool Debug { get; set; }

        public static TextWriter DebugTextWriter { get; set; }

        public static bool ThrowOnError { get; set; }

        public static void Inject(Activity target)
        {
            Inject(target, target);
        }

        public static void Inject(View target)
        {
            Inject(target, target);
        }

        public static void Inject(Dialog target)
        {
            Inject(target, target);
        }

        public static void Inject(object target, Context context)
        {
            Inject(target, target, (src) => context);
        }

        public static void Inject(object target, Activity source)
        {
            Inject(target, source, (src) => src);
        }

        public static void Inject(object target, View source)
        {
            Inject(target, source, (src) => src.Context);
        }

        public static void Inject(object target, Dialog source)
        {
            Inject(target, source, (src) => src.Context);
        }

        public static void Inject(object target, object source, Context context)
        {
            Inject(target, source, (src) => context);
        }

        public static void Inject<T>(object target, T source, Func<T, Context> getContext)
        {
            Context context = null;

            var typeMapping = GetTypeMapping(target.GetType());
            if (typeMapping != null && typeMapping.Members.Count > 0)
            {
                if (context == null)
                {
                    context = getContext(source);
                }
                foreach (var memberMapping in typeMapping.Members.Values)
                {
                    ProcessMemberMapping(target, source, context, memberMapping);
                }
            }

            if (typeMapping != null && typeMapping.Methods.Count > 0)
            {
                if (context == null)
                {
                    context = getContext(source);
                }
                foreach (var methodMapping in typeMapping.Methods.Values)
                {
                    ProcessMethodMapping(target, source, context, methodMapping);
                }
            }
        }

        private static void ProcessMemberMapping(object target, object source, Context context, MemberMapping memberMapping)
        {
            var memberInjected = false;
            var attr = memberMapping.Attribute;
            var memberType = memberMapping.MemberType;
            var resourceId = attr.ResourceId;
            var resourceType = context.Resources.GetResourceTypeName(resourceId);

            var needle = GetNeedle(resourceType, memberMapping);
            if (needle != null)
            {
                memberInjected = needle.Inject(target, source, resourceType, resourceId, context, memberMapping);
                if (memberInjected)
                {
                    HandleMessage(
                        "Injected resource '{0}' with id '{1}' to member '{2}'.",
                        context.Resources.GetResourceName(resourceId),
                        resourceId,
                        memberMapping.Member.Name);
                }
                else
                {
                    if (memberMapping.Attribute.Optional)
                    {
                        HandleMessage(
                            "Skipping injection for resource '{0}' with id '{1}' to member '{2}'.",
                            context.Resources.GetResourceName(resourceId),
                            resourceId,
                            memberMapping.Member.Name);
                    }
                    else
                    {
                        HandleError(
                            "Unable to inject resource '{0}' with id '{1}' to member '{2}'.",
                            context.Resources.GetResourceName(resourceId),
                            resourceId,
                            memberMapping.Member.Name);
                    }
                }
            }
            else
            {
                HandleError(
                    "No member needle found for resource type '{0}' and member type '{1}'.",
                    resourceType,
                    memberMapping.MemberType.FullName);
            }
        }

        private static void ProcessMethodMapping(object target, object source, Context context, MethodMapping methodMapping)
        {
            var methodInjected = false;
            var attr = methodMapping.Attribute;
            var view = SyringeExtensions.FindViewById(source, attr.ViewId);

            if (view != null)
            {
                var needle = GetEventNeedle(view.GetType(), methodMapping);
                if (needle != null)
                {
                    methodInjected = needle.Inject(target, source, view, context, methodMapping);

                    if (methodInjected)
                    {
                        HandleMessage(
                            "Injected handler for event '{0}' on view with id '{1}' for method '{2}'.",
                            attr.EventName,
                            context.Resources.GetResourceName(attr.ViewId),
                            methodMapping.Method.Name);
                    }
                    else
                    {
                        if (methodMapping.Attribute.Optional)
                        {
                            HandleMessage(
                                "Skipping injection of handler for event '{0}' with id '{1}' for method '{2}'.",
                                attr.EventName,
                                context.Resources.GetResourceName(attr.ViewId),
                                methodMapping.Method.Name);
                        }
                        else
                        {
                            HandleError(
                                "Unable to inject handler for event '{0}' with id '{1}' for method '{2}'.",
                                attr.EventName,
                                context.Resources.GetResourceName(attr.ViewId),
                                methodMapping.Method.Name);
                        }
                    }
                }
                else
                {
                    HandleError(
                        "No event needle found for event '{0}' on view with id '{1}' for method '{2}'.",
                        attr.EventName,
                        context.Resources.GetResourceName(attr.ViewId),
                        methodMapping.Method.Name);
                }
            }
            else
            {
                HandleError(
                    "No view found with id '{0}' for method '{1}'.",
                    context.Resources.GetResourceName(attr.ViewId),
                    methodMapping.Method.Name);
            }
        }

        public static void Withdraw(object target)
        {
            //Class <?> targetClass = target.getClass();
            //try
            //{
            //    if (debug) Log.d(TAG, "Looking up view needle for " + targetClass.getName());
            //    ViewNeedle<Object> viewNeedle = findViewNeedleForClass(targetClass);
            //    if (viewNeedle != null)
            //    {
            //        viewNeedle.uninject(target);
            //    }
            //}
            //catch (Exception e)
            //{
            //    throw new RuntimeException("Unable to uninject views for " + targetClass.getName(), e);
            //}
        }

        private static object CreateCollection(Type collectionType, Type specifiedType)
        {
            if (specifiedType == null)
            {
                if (collectionType.IsArray)
                {
                    specifiedType = collectionType;
                }
            }

            if (specifiedType != null)
            {
                return specifiedType.GetConstructor(null).Invoke(null);
            }

            return null;
        }

        public static TypeMapping GetTypeMapping(Type type)
        {
            TypeMapping mapping;

            if (typeMappings.ContainsKey(type))
            {
                // we have done it already
                HandleMessage("Using cached TypeMapping for type {0}.", type);
                mapping = typeMappings[type];
            }
            else
            {
                // we need to build the injection
                HandleMessage("Creating a new TypeMapping for type {0}.", type);
                mapping = new TypeMapping(type);
                typeMappings.Add(type, mapping);
            }

            return mapping;
        }

        public static INeedle GetNeedle(string resourceType, MemberMapping member)
        {
            INeedle needle = null;

            Dictionary<Type, INeedle> possibleNeedles;
            if (needles.TryGetValue(resourceType, out possibleNeedles))
            {
                if (!possibleNeedles.TryGetValue(member.MemberType, out needle))
                {
                    // this could be enhanced to find the most specific possibility
                    foreach (var possibility in possibleNeedles)
                    {
                        if (possibility.Key.IsAssignableFrom(member.MemberType))
                        {
                            needle = possibility.Value;
                            break;
                        }
                    }
                }
                //else
                //{
                //    // needle is already set by TryGetValue
                //}
            }

            if (needle != null)
            {
                HandleMessage(
                    "Found member needle for resource type '{0}' and member type '{1}': {2}",
                    resourceType,
                    member.MemberType.FullName,
                    needle.GetType().FullName);
            }

            return needle;
        }

        public static IEventNeedle GetEventNeedle(Type viewType, MethodMapping method)
        {
            IEventNeedle needle = null;
            if (!eventNeedles.TryGetValue(viewType, out needle))
            {
                // this could be enhanced to find the most specific possibility
                foreach (var possibility in eventNeedles)
                {
                    if (possibility.Key.IsAssignableFrom(viewType))
                    {
                        needle = possibility.Value;
                        break;
                    }
                }
            }
            //else
            //{
            //    // needle is already set by TryGetValue
            //}

            if (needle != null)
            {
                HandleMessage(
                    "Found event needle for view type '{0}': {1}",
                    viewType.FullName,
                    needle.GetType().FullName);
            }

            return needle;
        }

        public static void RegisterNeedle(string resourceType, Type targetType, INeedle needle)
        {
            Dictionary<Type, INeedle> needleCollection;
            if (!needles.TryGetValue(resourceType, out needleCollection))
            {
                needleCollection = new Dictionary<Type, INeedle>();
                needles.Add(resourceType, needleCollection);
                HandleMessage(
                    "Registered a new resource type: '{0}'",
                    resourceType);
            }
            if (needleCollection.ContainsKey(targetType))
            {
                HandleMessage(
                    "Replacing a needle registration for resource type '{0}' and member type '{1}': '{2}'",
                    resourceType,
                    targetType.FullName,
                    needle.GetType().FullName);
            }
            else
            {
                HandleMessage(
                    "Registered a new needle for resource type '{0}' and member type '{1}': '{2}'",
                    resourceType,
                    targetType.FullName,
                    needle.GetType().FullName);
            }
            needleCollection[targetType] = needle;
        }

        public static void RegisterEventNeedle(Type targetType, IEventNeedle needle)
        {
            if (eventNeedles.ContainsKey(targetType))
            {
                HandleMessage(
                    "Replacing an event needle registration for view type '{0}': '{2}'",
                    targetType.FullName,
                    needle.GetType().FullName);
            }
            else
            {
                HandleMessage(
                    "Registered a new event needle for view type '{1}': '{2}'",
                    targetType.FullName,
                    needle.GetType().FullName);
            }
            eventNeedles[targetType] = needle;
        }

        internal static void RegisterDefaultNeedles()
        {
            RegisterNeedle("id", typeof(Android.Views.View), new ViewNeedle());

            RegisterNeedle("anim", typeof(Android.Views.Animations.Animation), new AnimationNeedle());
            RegisterNeedle("animator", typeof(Android.Animation.Animator), new AnimatorNeedle());
            RegisterNeedle("bool", typeof(System.Boolean), new BooleanNeedle());
            RegisterNeedle("drawable", typeof(Android.Graphics.Bitmap), new BitmapNeedle());
            var colorNeedle = new ColorNeedle();
            RegisterNeedle("color", typeof(Android.Graphics.Color), colorNeedle);
            RegisterNeedle("color", typeof(System.Drawing.Color), colorNeedle);
            RegisterNeedle("color", typeof(Android.Content.Res.ColorStateList), new ColorStateListNeedle());
            RegisterNumberNeedle("dimen", new DimensionNeedle());
            RegisterNeedle("drawable", typeof(Android.Graphics.Drawables.Drawable), new DrawableNeedle());
            RegisterNumberNeedle("integer", new IntegerNeedle());
            RegisterNeedle("string", typeof(System.String), new StringNeedle());

            RegisterNeedle("array", typeof(System.Collections.Generic.IEnumerable<System.Int32>), new IntegerArrayNeedle());
            RegisterNeedle("array", typeof(System.Collections.Generic.IEnumerable<System.String>), new StringArrayNeedle());
            RegisterNeedle("array", typeof(Android.Content.Res.TypedArray), new TypedArrayNeedle());

            var xmlNeedle = new XmlNeedle();
            RegisterNeedle("xml", typeof(System.Xml.XmlReader), xmlNeedle);
            RegisterNeedle("xml", typeof(System.Xml.XmlDocument), xmlNeedle);
            RegisterNeedle("xml", typeof(System.Xml.Linq.XDocument), xmlNeedle);
            RegisterNeedle("xml", typeof(System.String), xmlNeedle);
        }

        internal static void RegisterNumberNeedle(string resourceType, INeedle needle)
        {
            RegisterNeedle(resourceType, typeof(System.Int16), needle);
            RegisterNeedle(resourceType, typeof(System.Int32), needle);
            RegisterNeedle(resourceType, typeof(System.Int64), needle);
            RegisterNeedle(resourceType, typeof(System.UInt16), needle);
            RegisterNeedle(resourceType, typeof(System.UInt32), needle);
            RegisterNeedle(resourceType, typeof(System.UInt64), needle);
            RegisterNeedle(resourceType, typeof(System.Byte), needle);
            RegisterNeedle(resourceType, typeof(System.SByte), needle);
            RegisterNeedle(resourceType, typeof(System.Single), needle);
            RegisterNeedle(resourceType, typeof(System.Double), needle);
            RegisterNeedle(resourceType, typeof(System.Decimal), needle);
        }

        internal static void RegisterDefaultEventNeedles()
        {
            RegisterEventNeedle(typeof(Android.Views.View), new ViewEventNeedle());
        }

        internal static void HandleError(string format, params object[] args)
        {
            HandleError(null, format, args);
        }

        internal static void HandleError(Exception exception, string format, params object[] args)
        {
            // make sure we don't multi-inject
            if (Debug && DebugTextWriter != null)
            {
                DebugTextWriter.WriteLine(format, args);
            }
            if (ThrowOnError)
            {
                throw new InjectionException(string.Format(format, args), exception);
            }
        }

        internal static void HandleMessage(string format, params object[] args)
        {
            // make sure we don't multi-inject
            if (Debug && DebugTextWriter != null)
            {
                DebugTextWriter.WriteLine(format, args);
            }
        }
    }
}
