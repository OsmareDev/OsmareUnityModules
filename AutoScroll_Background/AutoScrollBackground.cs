using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class AutoScrollBackground : MonoBehaviour
{
    [SerializeField] private RawImage m_img;

    [Tooltip("Allows the scroll to be done completely vertically despite the rotation or horizontally if the option is activated.")]
    [SerializeField] private bool m_counterRotation;

    [Tooltip("Allows scrolling to be done horizontally, by default vertical movement is sought.")]
    [SerializeField] private bool m_horizontal = false;

    [Tooltip("speed at which scrolls the image.")]
    [SerializeField] private float m_speed;

    [Tooltip("speed at which rotates the image.")]
    [SerializeField] private float m_rotationSpeed;

    void Update()
    {
        if (m_img == null) {
            Debug.LogWarning("No hay ninguna imagen configurada para el scroll");
            return;
        }

        Vector2 amount;
        m_img.gameObject.transform.eulerAngles = m_img.gameObject.transform.eulerAngles + new Vector3(0f, 0f, m_rotationSpeed) * Time.deltaTime;
        
        if (m_horizontal) {
            amount = new Vector2(m_speed, 0f);

            if (m_counterRotation) {
                amount = Quaternion.Euler(0f, 0f, -m_img.gameObject.transform.rotation.eulerAngles.z) * Vector2.right * m_speed;
            }
        } else {
            amount = new Vector2(0f, m_speed);

            if (m_counterRotation) {
                amount = Quaternion.Euler(0f, 0f, -m_img.gameObject.transform.rotation.eulerAngles.z) * Vector2.up * m_speed;
            }
        }

        m_img.uvRect = new Rect(m_img.uvRect.position + amount * Time.deltaTime, m_img.uvRect.size);
    }
}

#region Editor
#if UNITY_EDITOR
[CustomEditor(typeof(AutoScrollBackground))]
class AutoScrollBackgroundEditor : Editor {

    SerializedProperty m_counterRotation, m_horizontal, m_img, m_speed, m_rotationSpeed;

    private void OnEnable() {
        m_img = serializedObject.FindProperty("m_img");

        m_counterRotation = serializedObject.FindProperty("m_counterRotation");
        m_horizontal = serializedObject.FindProperty("m_horizontal");

        m_speed = serializedObject.FindProperty("m_speed");
        m_rotationSpeed = serializedObject.FindProperty("m_rotationSpeed");
    }

    public override void OnInspectorGUI() {
        AutoScrollBackground script = (AutoScrollBackground)target;
        serializedObject.Update();

        EditorGUILayout.PropertyField(m_img, new GUIContent("Raw Image Component"), true);
        if (m_img.objectReferenceValue == null) {
            EditorGUILayout.HelpBox("There is no Raw Image to scroll", MessageType.Error);

            serializedObject.ApplyModifiedProperties();
            return;
        }

        EditorGUI.indentLevel++;

        EditorGUILayout.PropertyField(m_counterRotation, new GUIContent("Undo Rotation"), true);
        EditorGUILayout.PropertyField(m_horizontal, true);

        EditorGUILayout.PropertyField(m_speed, new GUIContent("Scroll Speed"), true);
        EditorGUILayout.PropertyField(m_rotationSpeed, true);

        EditorGUI.indentLevel--;

        serializedObject.ApplyModifiedProperties();
    }
}
#endif
#endregion