using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.ExceptionServices;
using Android.Runtime;
using Java.Interop;
using Android.Views;
using Android.App;

namespace Syringe
{
    public static class SyringeExtensions
    {
        private readonly static MethodInfo javaCastMethod;
        private readonly static Type genericListType;
        static SyringeExtensions()
        {
            javaCastMethod = typeof(JavaObjectExtensions).GetMethod("JavaCast", BindingFlags.Static | BindingFlags.Public);
            genericListType = typeof(List<>);
        }

        public static IJavaObject JavaCast(this IJavaObject instance, Type targetType)
        {
            var generic = javaCastMethod.MakeGenericMethod(targetType);
            try
            {
                return (IJavaObject)generic.Invoke(null, new object[] { instance });
            }
            catch (TargetInvocationException ex)
            {
                ExceptionDispatchInfo.Capture(ex.InnerException).Throw();

                throw; // this is just for the compiler
            }
        }
        
        private static bool IsIEnumerable(Type type)
        {
            return type.IsGenericType &&
                   type.GetGenericTypeDefinition() == typeof(IEnumerable<>);
        }
        
        private static Type GetIEnumerable(Type type)
        {
            // we might be an IEnumerable
            if (IsIEnumerable(type))
            {
                return type;
            }

            // we may inherit from IEnumerable
            return type.GetInterfaces().Where(IsIEnumerable).FirstOrDefault();
        }

        public static Type GetEnumerableElementType(this Type enumerableType)
        {
            // try the easy one, an array
            if (enumerableType.HasElementType)
            {
                return enumerableType.GetElementType();
            }

            // we may be/inherit from IEnumerable
            var ienumerableType = GetIEnumerable(enumerableType);
            if (ienumerableType != null)
            {
                return ienumerableType.GetGenericArguments()[0];
            }

            // we are not generic, so there is no element type information
            return null;
        }

        public static IEnumerable CreateEnumerable(this Type collectionType, IEnumerable collection)
        {
            IEnumerable enumerable = null;

            var elementType = collectionType.GetEnumerableElementType();
            if (elementType != null)
            {
                Array array = null;
                if (collection != null)
                {
                    array = collection.GetType().IsArray
                        ? (Array)collection
                        : collection.Cast<object>().ToArray();
                }
                var arraySize = array == null ? 0 : array.Length;

                if (collectionType.IsArray)
                {
                    // handle arrays
                    var newArray = Array.CreateInstance(elementType, arraySize);
                    if (arraySize > 0)
                    {
                        Array.Copy(array, newArray, array.Length);
                    }
                    enumerable = newArray;
                }
                else if (collectionType.IsClass &&
                        !collectionType.IsAbstract &&
                        collectionType.GetConstructor(Type.EmptyTypes) != null &&
                        collectionType.GetInterfaces().Any(i => i == typeof(IList)))
                {
                    // we have some type:
                    //   is a class 
                    //   is not abstract 
                    //   has a default constructor
                    //   derives from IList
                    // so we can build this one
                    var instance = (IList)Activator.CreateInstance(collectionType);
                    if (arraySize > 0)
                    {
                        foreach (var item in array)
                        {
                            instance.Add(item);
                        }
                    }
                    enumerable = instance;
                }
                else
                {
                    // handle IEnumerable (we just try List<T>,as it is quite broad)
                    var listType = genericListType.MakeGenericType(elementType);
                    if (collectionType.IsAssignableFrom(listType))
                    {
                        var instance = (IList)Activator.CreateInstance(listType, arraySize);
                        if (arraySize > 0)
                        {
                            foreach (var item in array)
                            {
                                instance.Add(item);
                            }
                        }
                        enumerable = instance;
                    }
                }
            }
            
            return enumerable;
        }

        public static View FindViewById(object source, int resourceId)
        {
            var activity = source as Activity;
            if (activity != null)
            {
                return activity.FindViewById(resourceId);
            }

            var view = source as View;
            if (view != null)
            {
                return view.FindViewById(resourceId);
            }

            var dialog = source as Dialog;
            if (dialog != null)
            {
                return dialog.FindViewById(resourceId);
            }

            return null;
        }
    }
}
