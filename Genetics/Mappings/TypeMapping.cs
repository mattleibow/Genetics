using System;
using System.Collections.Generic;
using System.Reflection;

using Genetics.Attributes;

namespace Genetics.Mappings
{
    public class TypeMapping
    {
        public TypeMapping(Type type)
        {
            Type = type;
            Members = new Dictionary<MemberInfo, MemberMapping>();
            Methods = new Dictionary<MethodInfo, MethodMapping>();

            MapMembers();
        }

        public Type Type { get; set; }

        public Dictionary<MemberInfo, MemberMapping> Members { get; private set; }

        public Dictionary<MethodInfo, MethodMapping> Methods { get; private set; }

        private void MapMembers()
        {
            // we want to loop through all the attributes on all the members of the source
            var targetMembers = Type.GetMembers(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            foreach (var member in targetMembers)
            {
                var attr = member.GetCustomAttribute<SpliceAttribute>(false);
                if (attr != null)
                {
                    var mapping = new MemberMapping(Type, member, attr);
                    Members.Add(member, mapping);
                }
            }

            // we want to loop through all the attributes on all the methods of the source
            var targeMethods = Type.GetMethods(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
            foreach (var method in targeMethods)
            {
                var attr = method.GetCustomAttribute<SpliceEventAttribute>(false);
                if (attr != null)
                {
                    var mapping = new MethodMapping(Type, method, attr);
                    Methods.Add(method, mapping);
                }
            }
        }
    }
}
