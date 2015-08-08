using System;
using System.Reflection;
using Android.Content;
using Android.Views;

using Genetics.Mappings;

namespace Genetics.EventGenes
{
    public class ViewEventGene : IEventGene
    {
        public virtual bool Splice(object target, object source, View view, Context context, MethodMapping methodMapping)
        {
            return AttachDelegate(target, view, methodMapping);
        }

        public virtual void Sever(object target, object source, View view, Context context, MethodMapping methodMapping)
        {
            DetachDelegate(target, view, methodMapping);
        }

        protected bool AttachDelegate(object target, View view, MethodMapping methodMapping)
        {
            // get the handler
            EventInfo evnt;
            var del = CreateEventHandler(target, view, methodMapping, out evnt);

            // attach it
            var attached = false;
            try
            {
                evnt.AddEventHandler(view, del);
                attached = true;
            }
            catch (Exception ex)
            {
                Geneticist.HandleError(
                    ex,
                    "Unable to attach delegate '{0}' to event '{1}'.",
                    methodMapping.Method.Name,
                    methodMapping.Attribute.EventName);
            }
            return attached;
        }

        protected bool DetachDelegate(object target, View view, MethodMapping methodMapping)
        {
            // get the handler
            EventInfo evnt;
            var del = CreateEventHandler(target, view, methodMapping, out evnt);

            // detach it
            var detached = false;
            try
            {
                evnt.RemoveEventHandler(view, del);
                detached = true;
            }
            catch (Exception ex)
            {
                Geneticist.HandleError(
                    ex,
                    "Unable to detach delegate '{0}' from event '{1}'.",
                    methodMapping.Method.Name,
                    methodMapping.Attribute.EventName);
            }
            return detached;
        }

        protected Delegate CreateEventHandler(object target, View view, MethodMapping methodMapping, out EventInfo evnt)
        {
            Delegate del = null;
            evnt = null;
            if (view != null)
            {
                evnt = view.GetType().GetEvent(methodMapping.Attribute.EventName);
                if (evnt != null)
                {
                    try
                    {
                        del = CreateDelegate(target, methodMapping, evnt);
                    }
                    catch (Exception ex)
                    {
                        Geneticist.HandleError(
                            ex,
                            "Error creating delegate from '{0}' for event '{1}'.",
                            methodMapping.Method.Name,
                            methodMapping.Attribute.EventName);
                    }
                }
                else
                {
                    Geneticist.HandleError(
                        "Unable to find event '{0}' for method '{1}'.",
                        methodMapping.Attribute.EventName,
                        methodMapping.Method.Name);
                }
            }
            return del;
        }

        protected static Delegate CreateDelegate(object target, MethodMapping methodMapping, EventInfo evnt)
        {
            var match = MethodMapping.CompareMethods(evnt.EventHandlerType.GetMethod("Invoke"), methodMapping.Method);
            if (match == MethodMapping.MethodMatch.SimilarParameters)
            {
                return Delegate.CreateDelegate(evnt.EventHandlerType, target, methodMapping.Method);
            }
            //else if (match == MethodMapping.MethodMatch.SenderParameter)
            //{
            //    return null;
            //}
            //else if (match == MethodMapping.MethodMatch.NoParameters)
            //{
            //    return null;
            //}

            return null;
        }
    }
}
