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
    /// <summary>
    /// This type is used to inject views and resources into members using attributes.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Views and resource values can be injected into both fields and properties using 
    /// the <see cref="Attributes.SpliceAttribute">[Splice]</see> attribute. Event handlers can be attached 
    /// using the various <see cref="Attributes.SpliceEventAttribute">[SpliceEvent]</see> attributes.
    /// </para>
    /// <para>
    /// Finding views from your activity is as easy as:
    /// <code>
    /// public class ExampleActivity : Activity
    /// {
    ///   [Splice(Resource.Id.title)]
    ///   private EditText titleView;
    ///   [Splice(Resource.Id.subtitle)]
    ///   private EditText subtitleView;
    ///
    ///   protected override void OnCreate(Bundle savedInstanceState)
    ///   {
    ///     base.OnCreate(savedInstanceState);
    ///     SetContentView(Resource.Layout.ExampleLayout);
    ///     
    ///     Geneticist.Splice(this);
    ///   }
    /// }
    /// </code>
    /// </para>
    /// <para>
    /// View and resource injection can be performed directly on an <see cref="Splice(Activity)">Activity</see>,
    /// a <see cref="Splice(View)">View</see> or a <see cref="Splice(Dialog)">Dialog</see>. 
    /// Alternate objects with view members can be specified along with an 
    /// <see cref="Splice(object, Activity)">Activity</see>,
    /// <see cref="Splice(object, View)">View</see> or
    /// <see cref="Splice(object, Dialog)">Dialog</see>.
    /// </para>
    /// <para>
    /// In the case where the object only contains resource members, and no UI component, such as in the case
    /// of a <see cref="Android.Widget.IListAdapter" />, the <see cref="Context" /> 
    /// can be specified along with a <see langword="null" /> source object:
    /// <code>
    /// Geneticist.Splice(this, null, context);
    /// </code>
    /// </para>
    /// <para>
    /// Resource values can also be injected to fields or properties:
    /// <code>
    /// [Splice(Resource.Boolean.isTablet)]
    /// private bool isTablet;
    /// 
    /// [Splice(Resource.Integer.columns)]
    /// public int Columns { get; set; }
    /// 
    /// [Splice(Resource.Xml.manifest)]
    /// public XDocument Manifest { get; private set; }
    /// </code>
    /// </para>
    /// <para>
    /// To inject event handlers, you can attribute your methods. As long as the method parameters are compatible, 
    /// they will be automatically hooked up:
    /// <code>
    /// [SpliceClick(Resource.Id.submit)]
    /// private void OnSubmit(object sender, EventArgs e)
    /// {
    ///   // React to button click.
    /// }
    /// </code>
    /// </para>
    /// <para>
    /// Be default, views and resource are required to be present for member bindings.
    /// If a view is optional set the <see cref="Attributes.SpliceAttribute.Optional">Optional</see> 
    /// property to <c>true</c>:
    /// <code>
    /// [Splice(Resource.Id.title, Optional = true)]
    /// private EditText titleView;
    /// 
    /// [Splice(Resource.Xml.manifest, Optional = true)]
    /// public XDocument Manifest { get; private set; }
    /// </code>
    /// </para>
    /// </remarks>
    public static class Geneticist
    {
        private readonly static Dictionary<string, Dictionary<Type, IGene>> genes;
        private readonly static Dictionary<Type, IEventGene> eventGenes;
        private readonly static Dictionary<Type, TypeMapping> typeMappings;

        // performance improvements
        private static bool debug;
        private static TextWriter debugTextWriter;
        private static bool throwOnError;
        private static bool willWriteToDebug;

        static Geneticist()
        {
            typeMappings = new Dictionary<Type, TypeMapping>();
            genes = new Dictionary<string, Dictionary<Type, IGene>>();
            eventGenes = new Dictionary<Type, IEventGene>();
            ThrowOnError = true;
            debug = false;
            debugTextWriter = Console.Out;
            willWriteToDebug = false;

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


        /// <summary>
        /// Gets or sets a value indicating whether to write to the <see cref="DebugTextWriter" />.
        /// </summary>
        /// <value><c>true</c> if it should debug; otherwise, <c>false</c>.</value>
        public static bool Debug
        {
            get { return debug; }
            set
            {
                debug = value;
                willWriteToDebug = (debug && debugTextWriter != null);
            }
        }

        public static TextWriter DebugTextWriter
        {
            get { return debugTextWriter; }
            set
            {
                debugTextWriter = value;
                willWriteToDebug = (debug && debugTextWriter != null);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether errors should raise exceptions.
        /// </summary>
        /// <value><c>true</c> if errors should raise exceptions; otherwise, <c>false</c>.</value>
        public static bool ThrowOnError
        {
            get { return throwOnError; }
            set { throwOnError = value; }
        }

        /// <summary>
        /// Splice attributed members in the specified <see cref="Activity"/>. 
        /// The current content view is used as the view root.
        /// </summary>
        /// <param name="target">The target activity for member injection.</param>
        public static void Splice(Activity target)
        {
            Splice(target, target);
        }

        /// <summary>
        /// Splice attributed members in the specified <see cref="View"/>. 
        /// The specified view is used as the view root.
        /// </summary>
        /// <param name="target">The target view for member injection.</param>
        public static void Splice(View target)
        {
            Splice(target, target);
        }

        /// <summary>
        /// Splice attributed members in the specified <see cref="Dialog"/>. 
        /// The current content view is used as the view root.
        /// </summary>
        /// <param name="target">The target dialog for member injection.</param>
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
                foreach (var methodMapping in typeMapping.Methods.Values.SelectMany(x => x))
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
            // we delay the reading of the context to as late as possible
            var resourceType = context.Resources.GetResourceTypeName(resourceId);

            var gene = GetGene(resourceType, memberMapping);
            if (gene != null)
            {
                memberSpliced = gene.Splice(target, source, resourceType, resourceId, context, memberMapping);
                if (memberSpliced)
                {
                    if (willWriteToDebug)
                    {
                        HandleMessage(
                            "Spliced resource '{0}' with id '{1}' to member '{2}'.",
                            context.Resources.GetResourceName(resourceId),
                            resourceId,
                            memberMapping.Member.Name);
                    }
                }
                else
                {
                    if (memberMapping.Attribute.Optional)
                    {
                        if (willWriteToDebug)
                        {
                            HandleMessage(
                                "Skipping splice for resource '{0}' with id '{1}' to member '{2}'.",
                                context.Resources.GetResourceName(resourceId),
                                resourceId,
                                memberMapping.Member.Name);
                        }
                    }
                    else
                    {
                        if (willWriteToDebug || throwOnError)
                        {
                            HandleError(
                                "Unable to splice resource '{0}' with id '{1}' to member '{2}'.",
                                context.Resources.GetResourceName(resourceId),
                                resourceId,
                                memberMapping.Member.Name);
                        }
                    }
                }
            }
            else
            {
                if (willWriteToDebug || throwOnError)
                {
                    HandleError(
                        "No member gene found for resource type '{0}' and member type '{1}'.",
                        resourceType,
                        memberMapping.MemberType.FullName);
                }
            }
        }

        private static void ProcessMethodMapping(object target, object source, Context context, MethodMapping methodMapping)
        {
            var methodSpliced = false;
            var attr = methodMapping.Attribute;
            // we delay the finding of the view to as late as possible
            var view = GeneticsExtensions.FindViewById(source, attr.ViewId);

            if (view != null)
            {
                var gene = GetEventGene(view.GetType(), methodMapping);
                if (gene != null)
                {
                    methodSpliced = gene.Splice(target, source, view, attr.ViewId, context, methodMapping);

                    if (methodSpliced)
                    {
                        if (willWriteToDebug)
                        {
                            HandleMessage(
                                "Spliced handler for event '{0}' on view with id '{1}' for method '{2}'.",
                                attr.EventName,
                                context.Resources.GetResourceName(attr.ViewId),
                                methodMapping.Method.Name);
                        }
                    }
                    else
                    {
                        if (methodMapping.Attribute.Optional)
                        {
                            if (willWriteToDebug)
                            {
                                HandleMessage(
                                    "Skipping splice of handler for event '{0}' with id '{1}' for method '{2}'.",
                                    attr.EventName,
                                    context.Resources.GetResourceName(attr.ViewId),
                                    methodMapping.Method.Name);
                            }
                        }
                        else
                        {
                            if (willWriteToDebug || throwOnError)
                            {
                                HandleError(
                                    "Unable to splice handler for event '{0}' with id '{1}' for method '{2}'.",
                                    attr.EventName,
                                    context.Resources.GetResourceName(attr.ViewId),
                                    methodMapping.Method.Name);
                            }
                        }
                    }
                }
                else
                {
                    if (willWriteToDebug || throwOnError)
                    {
                        HandleError(
                            "No event gene found for event '{0}' on view with id '{1}' for method '{2}'.",
                            attr.EventName,
                            context.Resources.GetResourceName(attr.ViewId),
                            methodMapping.Method.Name);
                    }
                }
            }
            else
            {
                if (willWriteToDebug || throwOnError)
                {
                    HandleError(
                        "No view found with id '{0}' for method '{1}'.",
                        context.Resources.GetResourceName(attr.ViewId),
                        methodMapping.Method.Name);
                }
            }
        }

        public static void Sever(Activity target)
        {
            Sever(target, target);
        }

        public static void Sever(View target)
        {
            Sever(target, target);
        }

        public static void Sever(Dialog target)
        {
            Sever(target, target);
        }

        public static void Sever(object target, Context context)
        {
            Sever(target, target, (src) => context);
        }

        public static void Sever(object target, Activity source)
        {
            Sever(target, source, (src) => src);
        }

        public static void Sever(object target, View source)
        {
            Sever(target, source, (src) => src.Context);
        }

        public static void Sever(object target, Dialog source)
        {
            Sever(target, source, (src) => src.Context);
        }

        public static void Sever(object target, object source, Context context)
        {
            Sever(target, source, (src) => context);
        }

        public static void Sever<T>(object target, T source, Func<T, Context> getContext)
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
                    SeverMemberMapping(target, source, context, memberMapping);
                }
            }

            if (typeMapping != null && typeMapping.Methods.Count > 0)
            {
                if (context == null)
                {
                    context = getContext(source);
                }
                foreach (var methodMapping in typeMapping.Methods.Values.SelectMany(x => x))
                {
                    SeverMethodMapping(target, source, context, methodMapping);
                }
            }
        }

        private static void SeverMemberMapping(object target, object source, Context context, MemberMapping memberMapping)
        {
            var attr = memberMapping.Attribute;
            var memberType = memberMapping.MemberType;
            var resourceId = attr.ResourceId;
            // we delay the reading of the context to as late as possible
            var resourceType = context.Resources.GetResourceTypeName(resourceId);

            var gene = GetGene(resourceType, memberMapping);
            if (gene != null)
            {
                gene.Sever(target, source, resourceType, resourceId, context, memberMapping);

                if (willWriteToDebug)
                {
                    HandleMessage(
                        "Severed resource '{0}' with id '{1}' to member '{2}'.",
                        context.Resources.GetResourceName(resourceId),
                        resourceId,
                        memberMapping.Member.Name);
                }
            }
            else
            {
                if (willWriteToDebug || throwOnError)
                {
                    HandleError(
                        "No member gene found for resource type '{0}' and member type '{1}'.",
                        resourceType,
                        memberMapping.MemberType.FullName);
                }
            }
        }

        private static void SeverMethodMapping(object target, object source, Context context, MethodMapping methodMapping)
        {
            var attr = methodMapping.Attribute;
            // we delay the finding of the view to as late as possible
            var view = GeneticsExtensions.FindViewById(source, attr.ViewId);

            if (view != null)
            {
                var gene = GetEventGene(view.GetType(), methodMapping);
                if (gene != null)
                {
                    gene.Sever(target, source, view, attr.ViewId, context, methodMapping);
                    HandleMessage(
                        "Spliced handler for event '{0}' on view with id '{1}' for method '{2}'.",
                        attr.EventName,
                        context.Resources.GetResourceName(attr.ViewId),
                        methodMapping.Method.Name);
                }
                else
                {
                    if (willWriteToDebug || throwOnError)
                    {
                        HandleError(
                            "No event gene found for event '{0}' on view with id '{1}' for method '{2}'.",
                            attr.EventName,
                            context.Resources.GetResourceName(attr.ViewId),
                            methodMapping.Method.Name);
                    }
                }
            }
            else
            {
                if (willWriteToDebug)
                {
                    HandleMessage(
                        "No view found with id '{0}' for method '{1}'.",
                        context.Resources.GetResourceName(attr.ViewId),
                        methodMapping.Method.Name);
                }
            }
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
                if (willWriteToDebug)
                {
                    // we have done it already
                    HandleMessage("Using cached TypeMapping for type {0}.", type);
                }
                mapping = typeMappings[type];
            }
            else
            {
                if (willWriteToDebug)
                {
                    // we need to build the splice
                    HandleMessage("Creating a new TypeMapping for type {0}.", type);
                }
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
                if (willWriteToDebug)
                {
                    HandleMessage(
                        "Found member gene for resource type '{0}' and member type '{1}': {2}",
                        resourceType,
                        member.MemberType.FullName,
                        gene.GetType().FullName);
                }
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
                if (willWriteToDebug)
                {
                    HandleMessage(
                        "Found event gene for view type '{0}': {1}",
                        viewType.FullName,
                        gene.GetType().FullName);
                }
            }

            return gene;
        }

        public static void RegisterGene(string resourceType, Type targetType, IGene gene)
        {
            Dictionary<Type, IGene> geneCollection;
            var exists = genes.TryGetValue(resourceType, out geneCollection);
            if (!exists)
            {
                geneCollection = new Dictionary<Type, IGene>();
                genes.Add(resourceType, geneCollection);
                if (willWriteToDebug)
                {
                    HandleMessage(
                        "Registered a new resource type: '{0}'",
                        resourceType);
                }
            }

            if (willWriteToDebug)
            {
                if (exists)
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
            }
            geneCollection[targetType] = gene;
        }

        public static void RegisterEventGene(Type targetType, IEventGene gene)
        {
            if (willWriteToDebug)
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
            if (debug && debugTextWriter != null)
            {
                debugTextWriter.WriteLine(format, args);
            }
            if (throwOnError)
            {
                throw new SpliceException(string.Format(format, args), exception);
            }
        }

        internal static void HandleMessage(string format, params object[] args)
        {
            // make sure we don't multi-splice
            if (debug && debugTextWriter != null)
            {
                debugTextWriter.WriteLine(format, args);
            }
        }
    }
}
