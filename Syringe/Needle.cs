using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Android.App;
using Android.Content;
using Android.Views;

using Syringe.Mappings;
using Syringe.Needles;

namespace Syringe
{
    public static class Needle
    {
        private readonly static Dictionary<string, Dictionary<Type, INeedle>> needles;
        private readonly static Dictionary<Type, TypeMapping> typeMappings;

        static Needle()
        {
            typeMappings = new Dictionary<Type, TypeMapping>();
            needles = new Dictionary<string, Dictionary<Type, INeedle>>();
            ThrowOnError = true;
            Debug = false;
            DebugTextWriter = null;

            RegisterDefaultNeedles();
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
            var typeMapping = GetTypeMapping(target.GetType());
            if (typeMapping != null && typeMapping.Members.Count > 0)
            {
                var context = getContext(source);

                foreach (var memberMapping in typeMapping.Members.Values)
                {
                    var attr = memberMapping.Attribute;
                    var memberType = memberMapping.MemberType;

                    var resourceId = attr.ResourceIds[0];
                    var isMulti = attr.ResourceIds.Length > 1;

                    // set the field value
                    if (isMulti)
                    {
                        // try and inject a collection to the field
                        var container = CreateCollection(memberType, attr.Collection);
                        if (container == null)
                        {
                            HandleError(
                                "Cannot inject a collection to '{0}' on '{1}'.",
                                memberMapping.Member.Name,
                                typeMapping.Type.FullName);
                        }
                    }
                    else
                    {
                        var resourceType = context.Resources.GetResourceTypeName(resourceId);
                        var needle = GetNeedle(resourceType, memberMapping);
                        var memberInjected = needle.Inject(target, source, resourceType, resourceId, context, memberMapping);
                        if (memberInjected)
                        {
                            HandleMessage(
                                "Bound resource '{0}' with id '{1}' to member '{2}'.",
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
                }
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

            if (needle == null)
            {
                HandleError(
                    "No member needle found for resource type '{0}' and member type '{1}'.",
                    resourceType,
                    member.MemberType.FullName);
            }
            else
            {
                HandleMessage(
                    "Found member needle for resource type '{0}' and member type '{1}': {2}",
                    resourceType,
                    member.MemberType.FullName,
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
