﻿using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;

namespace ChromeLauncher.CommandLineApplication
{
    internal static class UtilsServices
    {
        public static string FormatWith(this string format, params object[] args)
        {
            return string.Format(format, args);
        }

        public static T GetAttribute<T>(this MethodInfo method) where T : Attribute
        {
            var att = Attribute.GetCustomAttribute(method, typeof(T));

            return (T)att;
        }

        public static T GetAttribute<T>(this Type type) where T : Attribute
        {
            var att = Attribute.GetCustomAttribute(type, typeof(T));

            return (T)att;
        }

        public static T GetAttribute<T>(this ParameterInfo parameter) where T : Attribute
        {
            var att = Attribute.GetCustomAttribute(parameter, typeof(T));

            return (T)att;
        }

        public static IEnumerable<T> GetAttributes<T>(this ParameterInfo parameter) where T : Attribute
        {
            var atts = Attribute.GetCustomAttributes(parameter, typeof(T)).Cast<T>();

            return atts;
        }

        public static IEnumerable<T> GetAttributes<T>(this PropertyInfo property) where T : Attribute
        {
            var atts = Attribute.GetCustomAttributes(property, typeof(T)).Cast<T>();

            return atts;
        }

        public static IEnumerable<T> GetInterfaceAttributes<T>(this MethodInfo method)
        {
            return method.GetCustomAttributes(true).
                Where(a => a.GetType().GetInterfaces().Contains(typeof(T))).
                Cast<T>();
        }

        public static IEnumerable<T> GetAttributes<T>(this Type type) where T : Attribute
        {
            var atts = Attribute.GetCustomAttributes(type, typeof(T)).Cast<T>();

            return atts;
        }

        public static bool HasAttribute<T>(this MethodInfo method) where T : Attribute
        {
            return Attribute.IsDefined(method, typeof(T));
        }

        public static bool HasAttribute<T>(this Type type) where T : Attribute
        {
            return Attribute.IsDefined(type, typeof(T));
        }

        public static bool HasAttribute<T>(this ParameterInfo parameter) where T : Attribute
        {
            return Attribute.IsDefined(parameter, typeof(T));
        }

        public static IEnumerable<MethodInfo> GetMethodsWith<T>(this Type type) where T : Attribute
        {
            var methods = GetAllMethods(type).
                Where(m => m.HasAttribute<T>());

            return methods;
        }

        public static IEnumerable<MethodInfo> GetAllMethods(this Type type)
        {
            var methods = type.GetMethods(
                BindingFlags.Public |
                BindingFlags.NonPublic |
                BindingFlags.Instance |
                BindingFlags.Static |
                BindingFlags.FlattenHierarchy);

            return methods;
        }

        public static bool None<T>(this IEnumerable<T> collection)
        {
            return collection == null || !collection.Any();
        }

        public static string StringJoin(this IEnumerable<string> strings, string separator)
        {
            return string.Join(separator, strings.ToArray());
        }

        public static IEnumerable<string> SplitBy(this string str, string separator)
        {
            return str.Split(new[] { separator }, StringSplitOptions.RemoveEmptyEntries);
        }

        public static IEnumerable<string> CommaSplit(this string str)
        {
            return SplitBy(str, ",");
        }

        public static void Each<T>(this IEnumerable<T> collection, Action<T, int> action)
        {
            var index = 0;

            foreach (var item in collection)
            {
                action(item, index);

                index++;
            }
        }

        public static string ToSafeString(this object obj, string nullValue)
        {
            return obj == null ? nullValue : obj.ToString();
        }

        public static bool StartsWith(this string str, IEnumerable<string> values)
        {
            return values.Any(v => str.StartsWith(v));
        }

        public static bool Contains(this string str, IEnumerable<string> values)
        {
            return values.Any(v => str.Contains(v));
        }

        public static string GetGenericTypeName(this Type type)
        {
            if (!type.IsGenericType)
            {
                return type.Name;
            }

            var genericTypeName = type.GetGenericTypeDefinition().Name;

            genericTypeName = genericTypeName.Remove(genericTypeName.IndexOf('`'));

            var genericArgs = type.GetGenericArguments().
                Select(a => GetGenericTypeName(a)).
                StringJoin(",");

            return "{0}<{1}>".FormatWith(genericTypeName, genericArgs);
        }
    }
}
