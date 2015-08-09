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

namespace Genetics.Genes
{
    public class XmlGene : IGene
    {
        public bool Splice(object target, object source, string resourceType, int resourceId, Context context, MemberMapping memberMapping)
        {
            var assigned = false;
            var reader = new WrappedXmlReader(context.Resources.GetXml(resourceId));
            try
            {
                if (memberMapping.MemberType.IsAssignableFrom(typeof(XmlReader)))
                {
                    memberMapping.SetterMethod(target, reader);
                    assigned = true;
                }
                else if (memberMapping.MemberType.IsAssignableFrom(typeof(XmlDocument)))
                {
                    var doc = new XmlDocument();
                    doc.Load(reader);
                    reader.Close();
                    memberMapping.SetterMethod(target, doc);
                    assigned = true;
                }
                else if (memberMapping.MemberType.IsAssignableFrom(typeof(XDocument)))
                {
                    var doc = XDocument.Load(reader);
                    reader.Close();
                    memberMapping.SetterMethod(target, doc);
                    assigned = true;
                }
                else if (memberMapping.MemberType.IsAssignableFrom(typeof(string)))
                {
                    var xml = XDocument.Load(reader).ToString();
                    memberMapping.SetterMethod(target, xml);
                    assigned = true;
                }
            }
            catch (Exception exception)
            {
                reader.Dispose();

                Geneticist.HandleError(
                    exception,
                    "Unable to splice resource '{0}' with id '{1}' to member '{2}'.",
                    context.Resources.GetResourceName(resourceId),
                    resourceId,
                    memberMapping.Member.Name);
            }
            return assigned;
        }

        public void Sever(object target, object source, string resourceType, int resourceId, Context context, MemberMapping memberMapping)
        {
            var reader = memberMapping.GetterMethod(target) as XmlReader;
            if (memberMapping.Attribute.DisposeOnSever)
            {
                reader.Dispose();
            }
            memberMapping.SetterMethod(target, null);
        }

        /// <summary>
        /// Temporary work around until the Xamarin.Android bug (#32696) is fixed:
        /// https://bugzilla.xamarin.com/show_bug.cgi?id=32696
        /// </summary>
        private class WrappedXmlReader : XmlReader, IXmlLineInfo
        {
            private readonly XmlReader source;
            private readonly IXmlLineInfo lineInfo;

            public WrappedXmlReader(XmlReader source)
            {
                this.source = source;
                this.lineInfo = source as IXmlLineInfo;
            }

            public override int AttributeCount
            {
                get { return source.AttributeCount; }
            }
            public override string BaseURI
            {
                get { return source.BaseURI; }
            }
            public override int Depth
            {
                get { return source.Depth; }
            }
            public override bool EOF
            {
                get { return source.EOF; }
            }
            public override bool HasAttributes
            {
                get { return source.HasAttributes; }
            }
            public override bool HasValue
            {
                get { return source.HasValue; }
            }
            public override bool IsDefault
            {
                get { return source.IsDefault; }
            }
            public override bool IsEmptyElement
            {
                get { return source.IsEmptyElement; }
            }
            public int LineNumber
            {
                get { return lineInfo.LineNumber; }
            }
            public int LinePosition
            {
                get { return lineInfo.LinePosition; }
            }
            public override string LocalName
            {
                get { return source.LocalName; }
            }
            public override string Name
            {
                get { return source.Name; }
            }
            public override string NamespaceURI
            {
                get { return source.NamespaceURI; }
            }
            public override XmlNameTable NameTable
            {
                get { return source.NameTable; }
            }
            public override XmlNodeType NodeType
            {
                get { return source.NodeType; }
            }
            public override string Prefix
            {
                get { return source.Prefix; }
            }
            public override ReadState ReadState
            {
                get { return source.ReadState; }
            }
            public override string Value
            {
                get { return source.Value; }
            }
            public override void Close()
            {
                source.Close();
            }
            public override string GetAttribute(string name)
            {
                return source.GetAttribute(name);
            }
            public override string GetAttribute(int i)
            {
                return source.GetAttribute(i);
            }
            public override string GetAttribute(string localName, string namespaceName)
            {
                return source.GetAttribute(localName, namespaceName);
            }
            public bool HasLineInfo()
            {
                return lineInfo != null && lineInfo.HasLineInfo();
            }
            public override string LookupNamespace(string prefix)
            {
                return source.LookupNamespace(prefix);
            }
            public override bool MoveToAttribute(string name)
            {
                return source.MoveToAttribute(name);
            }
            public override void MoveToAttribute(int i)
            {
                source.MoveToAttribute(i);
            }
            public override bool MoveToAttribute(string localName, string namespaceName)
            {
                return source.MoveToAttribute(localName, namespaceName);
            }
            public override bool MoveToElement()
            {
                return source.MoveToElement();
            }
            public override bool MoveToFirstAttribute()
            {
                return source.MoveToFirstAttribute();
            }
            public override bool MoveToNextAttribute()
            {
                // Temporary work around until the Xamarin.Android bug (#32696) is fixed: 
                // https://bugzilla.xamarin.com/show_bug.cgi?id=32696
                var result = source.MoveToNextAttribute();
                if (result)
                {
                    try
                    {
                        var name = source.Name;
                    }
                    catch (XmlPullParserException)
                    {
                        result = false;
                    }
                }
                return result;
            }
            public override bool Read()
            {
                return source.Read();
            }
            public override bool ReadAttributeValue()
            {
                return source.ReadAttributeValue();
            }
            public override void ResolveEntity()
            {
                source.ResolveEntity();
            }
        }
    }
}
