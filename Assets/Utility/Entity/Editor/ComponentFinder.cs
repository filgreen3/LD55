using System;
using System.Collections.Generic;
using System.Reflection;

using UnityEngine;

public static class ComponentFinder
{
    public static Type GetTypeFromAssembley(string typeName, Assembly assembly)
    {
        var types = assembly.GetTypes();
        var type = (Type)null;
        foreach (var item in types)
        {
            if (item.Name == typeName)
            {
                type = item;
                break;
            }
        }
        return type;
    }


    public static Type[] GetSystemTypes(Assembly assembly)
    {
        var result = new List<Type>();
        var types = assembly.GetTypes();
        foreach (var type in types)
        {
            if (isSystemAcceptable(type))
            {
                result.Add(type);
            }
        }
        return result.ToArray();
    }

    public static bool isSystemAcceptable(Type type, Entity entity = null) =>
        type != null
        && type.IsClass
        && !type.IsAbstract
        && !typeof(MonoBehaviour).IsAssignableFrom(type)
        && (entity == null || entity.ComponentType.IsAssignableFrom(type))
        && (entity == null || entity.AllowDuplication || !entity.HasEntityComponent(type));

    public static string[] GetSystems(Assembly assembly, Entity entity)
    {
        var result = new List<string>();
        var types = assembly.GetTypes();
        foreach (var type in types)
        {
            if (isSystemAcceptable(type, entity))
            {
                result.Add(type.Name);
            }
        }
        return result.ToArray();
    }

    public static string[] GetAssembles(Assembly[] assembles)
    {
        var result = new List<string>();
        foreach (var assembly in assembles)
        {
            result.Add(assembly.GetName().Name);
        }
        return result.ToArray();
    }
}