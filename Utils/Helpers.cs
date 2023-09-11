using System;
using UnityEngine;

public class Helpers
{
    public static T GetNextInEnum<T>(int value) where T : Enum {
        int nextValue = (int)(object)value + 1;
        if (!Enum.IsDefined(typeof(T), nextValue)) nextValue = 0;
        return (T)(object)nextValue;
    } 

    private static Camera m_camera;
    public static Camera CameraMain {
        get {
            if (m_camera == null) m_camera = Camera.main;
            return m_camera;
        }
    }
}
