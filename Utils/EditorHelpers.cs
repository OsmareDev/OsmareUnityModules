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
}
