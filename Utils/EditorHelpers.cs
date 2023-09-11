using System.Reflection;
using UnityEditor;
using UnityEngine;

public class EditorHelpers
{
    public static void CollectInterface<T>(ref SerializedProperty prop, string msg) {
        prop.objectReferenceValue = EditorGUILayout.ObjectField(msg, prop.objectReferenceValue, typeof(Object), true);

        if (prop.objectReferenceValue is T) return;
        else if (prop.objectReferenceValue is GameObject && ((GameObject)prop.objectReferenceValue).TryGetComponent<T>(out T component)) {
            prop.objectReferenceValue = (Object)(object)component;
            return;
        }

        prop.objectReferenceValue = null;
    }

    public static void CollectAnyThingWithTheFunction(ref SerializedProperty prop, string functionName, string msg) {
        prop.objectReferenceValue = EditorGUILayout.ObjectField(msg, prop.objectReferenceValue, typeof(Object), true);

        if (prop.objectReferenceValue is GameObject) {
            Component[] components = ((GameObject)prop.objectReferenceValue).GetComponents<Component>();
            foreach (Component component in components) if (CollectAndCheckMethods(component, functionName)) {
                prop.objectReferenceValue = component;
                return;
            }
        } 
        else if (CollectAndCheckMethods(prop.objectReferenceValue, functionName)) return;

        prop.objectReferenceValue = null;
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
}
