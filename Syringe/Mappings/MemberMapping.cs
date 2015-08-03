using System;
using System.Reflection;

namespace Syringe.Mappings
{
    public class MemberMapping
    {
        public MemberMapping(Type type, MemberInfo member, InjectAttribute attr)
        {
            Type = type;
            Member = member;
            Attribute = attr;

            MapMember();
        }

        public Type Type { get; set; }

        public MemberInfo Member { get; set; }

        public InjectAttribute Attribute { get; set; }

        public Type MemberType { get; set; }

        public Action<object, object> SetterMethod { get; set; }

        public Func<object, object> GetterMethod { get; set; }

        private void MapMember()
        {
            // get the setter method and field type
            if (Member.MemberType == MemberTypes.Field)
            {
                var field = ((FieldInfo)Member);
                if (field.IsInitOnly)
                {
                    Needle.HandleError(
                        "Cannot inject '{0}' on '{1}' because it is readonly.",
                        Member.Name,
                        Type.FullName);
                }
                else
                {
                    SetterMethod = field.SetValue;
                    GetterMethod = field.GetValue;
                    MemberType = field.FieldType;
                }
            }
            else if (Member.MemberType == MemberTypes.Property)
            {
                var property = ((PropertyInfo)Member);
                if (property.SetMethod == null)
                {
                    Needle.HandleError(
                        "Cannot inject '{0}' on '{1}' because it is readonly.",
                        Member.Name,
                        Type.FullName);
                }
                else
                {
                    SetterMethod = (t, v) => property.SetMethod.Invoke(t, new[] { v });
                    GetterMethod = (t) => property.GetMethod.Invoke(t, new object[0]);
                    MemberType = property.PropertyType;
                }
            }
            else
            {
                Needle.HandleError(
                    "The injection of '{0}' on '{1}' is not yet supported: {2}.",
                    Member.Name,
                    Type.FullName,
                    Member.MemberType);
            }
        }
    }
}
