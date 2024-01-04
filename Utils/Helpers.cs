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

    public static void DoNothing() {}

    public static Vector3 QuadraticLerp(Vector3 a, Vector3 b, Vector3 c, float t) {
        Vector3 ab = Vector3.Lerp(a, b, t);
        Vector3 bc = Vector3.Lerp(b, c, t);

        return Vector3.Lerp(ab, bc, t);
    }

    public static Vector3 CubicLerp(Vector3 a, Vector3 b, Vector3 c, Vector3 d, float t) {
        Vector3 ab_bc = QuadraticLerp(a, b, c, t);
        Vector3 bc_cd = QuadraticLerp(b, c, d, t);

        return Vector3.Lerp(ab_bc, bc_cd, t);
    }
}
