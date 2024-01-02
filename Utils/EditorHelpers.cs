using System.Reflection;
using UnityEditor;
using UnityEngine;

public class EditorHelpers
{
    public static void CollectInterface<T>(ref SerializedProperty prop, string msg) {
        Object obj = EditorGUILayout.ObjectField(msg, prop.objectReferenceValue, typeof(Object), true);
        if (obj == null) { prop.objectReferenceValue = null; return; }

        if (obj is T) { prop.objectReferenceValue = obj; return; }
        else if (obj is GameObject && ((GameObject)obj).TryGetComponent<T>(out T component)) {
            prop.objectReferenceValue = (Object)(object)component;
            return;
        }
    }

    public static void CollectAnyThingWithTheFunction(ref SerializedProperty prop, string functionName, string msg) {
        Object obj = EditorGUILayout.ObjectField(msg, prop.objectReferenceValue, typeof(Object), true);
        if (obj == null) { prop.objectReferenceValue = null; return; }

        if (obj is GameObject) {
            Component[] components = ((GameObject)obj).GetComponents<Component>();
            foreach (Component component in components) if (CollectAndCheckMethods(component, functionName)) {
                prop.objectReferenceValue = component;
                return;
            }
        } 
        else if (CollectAndCheckMethods(obj, functionName)) { prop.objectReferenceValue = obj; return; }
    }

    private static bool CollectAndCheckMethods(Object obj, string functionName) {
        if (obj == null) return false;

        MethodInfo[] metodos = obj.GetType().GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static);

        foreach (MethodInfo metodo in metodos)
        {
            if (metodo.Name == functionName)
            {
                return true;
            }
        }

        return false;
    }

    public static object CallMethod(Object obj, string functionName, params object[] parameters) {

        MethodInfo method = obj.GetType().GetMethod(functionName);
        if (method != null) {
            return method.Invoke(obj, parameters);
        }

        return null;
    }
    public static T CallMethod<T>(Object obj, string functionName, params object[] parameters) {

        MethodInfo method = obj.GetType().GetMethod(functionName);
        if (method != null) {
            return (T)method.Invoke(obj, parameters);
        }

        return default;
    }

    public static object CallProperty(Object obj, string propertyName) {
        PropertyInfo property = obj.GetType().GetProperty(propertyName);
        if (property != null) {
            return property.GetGetMethod().Invoke(obj, null);
        }

        return null;
    }
    public static T CallProperty<T>(Object obj, string propertyName) {
        PropertyInfo property = obj.GetType().GetProperty(propertyName);
        if (property != null) {
            return (T)property.GetGetMethod().Invoke(obj, null);
        }

        return default;
    }
}
