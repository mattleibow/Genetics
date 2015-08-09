using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Android.App;
using Android.Content;
using Android.Views;

using Genetics.Mappings;
using Genetics.Genes;
using Genetics.EventGenes;

namespace Genetics
{
    public static class Geneticist
    {
        private readonly static Dictionary<string, Dictionary<Type, IGene>> genes;
        private readonly static Dictionary<Type, IEventGene> eventGenes;
        private readonly static Dictionary<Type, TypeMapping> typeMappings;

        static Geneticist()
        {
            typeMappings = new Dictionary<Type, TypeMapping>();
            genes = new Dictionary<string, Dictionary<Type, IGene>>();
            eventGenes = new Dictionary<Type, IEventGene>();
            ThrowOnError = true;
            Debug = false;
            DebugTextWriter = null;

            RegisterDefaultGenes();
            RegisterDefaultEventGenes();
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

        public static void Splice(Activity target)
        {
            Splice(target, target);
        }

        public static void Splice(View target)
        {
            Splice(target, target);
        }

        public static void Splice(Dialog target)
        {
            Splice(target, target);
        }

        public static void Splice(object target, Context context)
        {
            Splice(target, target, (src) => context);
        }

        public static void Splice(object target, Activity source)
        {
            Splice(target, source, (src) => src);
        }

        public static void Splice(object target, View source)
        {
            Splice(target, source, (src) => src.Context);
        }

        public static void Splice(object target, Dialog source)
        {
            Splice(target, source, (src) => src.Context);
        }

        public static void Splice(object target, object source, Context context)
        {
            Splice(target, source, (src) => context);
        }

        public static void Splice<T>(object target, T source, Func<T, Context> getContext)
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
            var memberSpliced = false;
            var attr = memberMapping.Attribute;
            var memberType = memberMapping.MemberType;
            var resourceId = attr.ResourceId;
            var resourceType = context.Resources.GetResourceTypeName(resourceId);

            var gene = GetGene(resourceType, memberMapping);
            if (gene != null)
            {
                memberSpliced = gene.Splice(target, source, resourceType, resourceId, context, memberMapping);
                if (memberSpliced)
                {
                    HandleMessage(
                        "Spliced resource '{0}' with id '{1}' to member '{2}'.",
                        context.Resources.GetResourceName(resourceId),
                        resourceId,
                        memberMapping.Member.Name);
                }
                else
                {
                    if (memberMapping.Attribute.Optional)
                    {
                        HandleMessage(
                            "Skipping splice for resource '{0}' with id '{1}' to member '{2}'.",
                            context.Resources.GetResourceName(resourceId),
                            resourceId,
                            memberMapping.Member.Name);
                    }
                    else
                    {
                        HandleError(
                            "Unable to splice resource '{0}' with id '{1}' to member '{2}'.",
                            context.Resources.GetResourceName(resourceId),
                            resourceId,
                            memberMapping.Member.Name);
                    }
                }
            }
            else
            {
                HandleError(
                    "No member gene found for resource type '{0}' and member type '{1}'.",
                    resourceType,
                    memberMapping.MemberType.FullName);
            }
        }

        private static void ProcessMethodMapping(object target, object source, Context context, MethodMapping methodMapping)
        {
            var methodSpliced = false;
            var attr = methodMapping.Attribute;
            var view = GeneticsExtensions.FindViewById(source, attr.ViewId);

            if (view != null)
            {
                var gene = GetEventGene(view.GetType(), methodMapping);
                if (gene != null)
                {
                    methodSpliced = gene.Splice(target, source, view, context, methodMapping);

                    if (methodSpliced)
                    {
                        HandleMessage(
                            "Spliced handler for event '{0}' on view with id '{1}' for method '{2}'.",
                            attr.EventName,
                            context.Resources.GetResourceName(attr.ViewId),
                            methodMapping.Method.Name);
                    }
                    else
                    {
                        if (methodMapping.Attribute.Optional)
                        {
                            HandleMessage(
                                "Skipping splice of handler for event '{0}' with id '{1}' for method '{2}'.",
                                attr.EventName,
                                context.Resources.GetResourceName(attr.ViewId),
                                methodMapping.Method.Name);
                        }
                        else
                        {
                            HandleError(
                                "Unable to splice handler for event '{0}' with id '{1}' for method '{2}'.",
                                attr.EventName,
                                context.Resources.GetResourceName(attr.ViewId),
                                methodMapping.Method.Name);
                        }
                    }
                }
                else
                {
                    HandleError(
                        "No event gene found for event '{0}' on view with id '{1}' for method '{2}'.",
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

        public static void Sever(object target)
        {
            //Class <?> targetClass = target.getClass();
            //try
            //{
            //    if (debug) Log.d(TAG, "Looking up view gene for " + targetClass.getName());
            //    ViewGene<Object> viewGene = findViewGeneForClass(targetClass);
            //    if (viewGene != null)
            //    {
            //        viewGene.unsplice(target);
            //    }
            //}
            //catch (Exception e)
            //{
            //    throw new RuntimeException("Unable to unsplice views for " + targetClass.getName(), e);
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
                // we need to build the splice
                HandleMessage("Creating a new TypeMapping for type {0}.", type);
                mapping = new TypeMapping(type);
                typeMappings.Add(type, mapping);
            }

            return mapping;
        }

        public static IGene GetGene(string resourceType, MemberMapping member)
        {
            IGene gene = null;

            Dictionary<Type, IGene> possibleGenes;
            if (genes.TryGetValue(resourceType, out possibleGenes))
            {
                if (!possibleGenes.TryGetValue(member.MemberType, out gene))
                {
                    // this could be enhanced to find the most specific possibility
                    foreach (var possibility in possibleGenes)
                    {
                        if (possibility.Key.IsAssignableFrom(member.MemberType))
                        {
                            gene = possibility.Value;
                            break;
                        }
                    }
                }
                //else
                //{
                //    // gene is already set by TryGetValue
                //}
            }

            if (gene != null)
            {
                HandleMessage(
                    "Found member gene for resource type '{0}' and member type '{1}': {2}",
                    resourceType,
                    member.MemberType.FullName,
                    gene.GetType().FullName);
            }

            return gene;
        }

        public static IEventGene GetEventGene(Type viewType, MethodMapping method)
        {
            IEventGene gene = null;
            if (!eventGenes.TryGetValue(viewType, out gene))
            {
                // this could be enhanced to find the most specific possibility
                foreach (var possibility in eventGenes)
                {
                    if (possibility.Key.IsAssignableFrom(viewType))
                    {
                        gene = possibility.Value;
                        break;
                    }
                }
            }
            //else
            //{
            //    // gene is already set by TryGetValue
            //}

            if (gene != null)
            {
                HandleMessage(
                    "Found event gene for view type '{0}': {1}",
                    viewType.FullName,
                    gene.GetType().FullName);
            }

            return gene;
        }

        public static void RegisterGene(string resourceType, Type targetType, IGene gene)
        {
            Dictionary<Type, IGene> geneCollection;
            if (!genes.TryGetValue(resourceType, out geneCollection))
            {
                geneCollection = new Dictionary<Type, IGene>();
                genes.Add(resourceType, geneCollection);
                HandleMessage(
                    "Registered a new resource type: '{0}'",
                    resourceType);
            }
            if (geneCollection.ContainsKey(targetType))
            {
                HandleMessage(
                    "Replacing a gene registration for resource type '{0}' and member type '{1}': '{2}'",
                    resourceType,
                    targetType.FullName,
                    gene.GetType().FullName);
            }
            else
            {
                HandleMessage(
                    "Registered a new gene for resource type '{0}' and member type '{1}': '{2}'",
                    resourceType,
                    targetType.FullName,
                    gene.GetType().FullName);
            }
            geneCollection[targetType] = gene;
        }

        public static void RegisterEventGene(Type targetType, IEventGene gene)
        {
            if (eventGenes.ContainsKey(targetType))
            {
                HandleMessage(
                    "Replacing an event gene registration for view type '{0}': '{2}'",
                    targetType.FullName,
                    gene.GetType().FullName);
            }
            else
            {
                HandleMessage(
                    "Registered a new event gene for view type '{1}': '{2}'",
                    targetType.FullName,
                    gene.GetType().FullName);
            }
            eventGenes[targetType] = gene;
        }

        internal static void RegisterDefaultGenes()
        {
            RegisterGene("id", typeof(Android.Views.View), new ViewGene());

            RegisterGene("anim", typeof(Android.Views.Animations.Animation), new AnimationGene());
            RegisterGene("animator", typeof(Android.Animation.Animator), new AnimatorGene());
            RegisterGene("bool", typeof(System.Boolean), new BooleanGene());
            RegisterGene("drawable", typeof(Android.Graphics.Bitmap), new BitmapGene());
            var colorGene = new ColorGene();
            RegisterGene("color", typeof(Android.Graphics.Color), colorGene);
            RegisterGene("color", typeof(System.Drawing.Color), colorGene);
            RegisterGene("color", typeof(Android.Content.Res.ColorStateList), new ColorStateListGene());
            RegisterNumberGene("dimen", new DimensionGene());
            RegisterGene("drawable", typeof(Android.Graphics.Drawables.Drawable), new DrawableGene());
            RegisterNumberGene("integer", new IntegerGene());
            RegisterGene("string", typeof(System.String), new StringGene());

            RegisterGene("array", typeof(System.Collections.Generic.IEnumerable<System.Int32>), new IntegerArrayGene());
            RegisterGene("array", typeof(System.Collections.Generic.IEnumerable<System.String>), new StringArrayGene());
            RegisterGene("array", typeof(Android.Content.Res.TypedArray), new TypedArrayGene());

            var xmlGene = new XmlGene();
            RegisterGene("xml", typeof(System.Xml.XmlReader), xmlGene);
            RegisterGene("xml", typeof(System.Xml.XmlDocument), xmlGene);
            RegisterGene("xml", typeof(System.Xml.Linq.XDocument), xmlGene);
            RegisterGene("xml", typeof(System.String), xmlGene);
        }

        internal static void RegisterNumberGene(string resourceType, IGene gene)
        {
            RegisterGene(resourceType, typeof(System.Int16), gene);
            RegisterGene(resourceType, typeof(System.Int32), gene);
            RegisterGene(resourceType, typeof(System.Int64), gene);
            RegisterGene(resourceType, typeof(System.UInt16), gene);
            RegisterGene(resourceType, typeof(System.UInt32), gene);
            RegisterGene(resourceType, typeof(System.UInt64), gene);
            RegisterGene(resourceType, typeof(System.Byte), gene);
            RegisterGene(resourceType, typeof(System.SByte), gene);
            RegisterGene(resourceType, typeof(System.Single), gene);
            RegisterGene(resourceType, typeof(System.Double), gene);
            RegisterGene(resourceType, typeof(System.Decimal), gene);
        }

        internal static void RegisterDefaultEventGenes()
        {
            RegisterEventGene(typeof(Android.Views.View), new ViewEventGene());
        }

        internal static void HandleError(string format, params object[] args)
        {
            HandleError(null, format, args);
        }

        internal static void HandleError(Exception exception, string format, params object[] args)
        {
            // make sure we don't multi-splice
            if (Debug && DebugTextWriter != null)
            {
                DebugTextWriter.WriteLine(format, args);
            }
            if (ThrowOnError)
            {
                throw new SpliceException(string.Format(format, args), exception);
            }
        }

        internal static void HandleMessage(string format, params object[] args)
        {
            // make sure we don't multi-splice
            if (Debug && DebugTextWriter != null)
            {
                DebugTextWriter.WriteLine(format, args);
            }
        }
    }
}
