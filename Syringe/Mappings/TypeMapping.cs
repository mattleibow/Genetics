using System;
using System.Collections.Generic;
using System.Reflection;

namespace Syringe.Mappings
{
    public class TypeMapping
    {
        public TypeMapping(Type type)
        {
            Type = type;
            Members = new Dictionary<MemberInfo, MemberMapping>();

            MapMembers();
        }

        public Type Type { get; set; }

        public Dictionary<MemberInfo, MemberMapping> Members { get; set; }

        private void MapMembers()
        {
            // we want to loop through all the attributes on all the members of the source
            var targetMembers = Type.GetMembers(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            foreach (var member in targetMembers)
            {
                var attr = member.GetCustomAttribute<InjectAttribute>(false);
                if (attr != null)
                {
                    if (attr.ResourceIds == null && attr.ResourceIds.Length == 0)
                    {
                        // nothing to inject

                        Needle.HandleError(
                            "Nothing to inject for '{0}' on '{1}'.",
                            member.Name,
                            Type.FullName);
                    }
                    else if (attr.ResourceIds.Length > 1)
                    {
                        // multiple injection

                        //var mapping = new MemberMapping(Type, member, attr);
                        //Members.Add(member, mapping);
                    }
                    else
                    {
                        // single injection

                        var mapping = new MemberMapping(Type, member, attr);
                        Members.Add(member, mapping);
                    }
                }
            }
        }
    }
}
