using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class CameraObstacleCheck : MonoBehaviour
{
    [SerializeField] private Transform m_player;
    [SerializeField] private Camera m_camera;
    [SerializeField] private LayerMask m_layersToCheck;

    public void Update() {

        Vector3 direction = m_player.position - transform.position;
        float distance = direction.magnitude;
        direction.Normalize();
        Vector3 position = transform.position + direction * m_camera.nearClipPlane;

        List<RaycastHit> hits = new List<RaycastHit>(Physics.RaycastAll(position, direction, distance, m_layersToCheck));
        hits.ForEach( hit => {
            if (hit.transform.TryGetComponent<IFadeObject>(out IFadeObject fo)) fo.Fade();
        });
        // if (Physics.RaycastAll(position, direction, out hit))
        // {
        //     if (hit.transform.TryGetComponent<FadeObject>(out FadeObject fo)) fo.Fade();
        // }
    }
}

#region Editor
#if UNITY_EDITOR
[CustomEditor(typeof(CameraObstacleCheck))]
class CameraObstacleCheckEditor : Editor {

    SerializedProperty m_player, m_camera, m_layersToCheck;

    private void OnEnable() {
        m_player = serializedObject.FindProperty("m_player");
        m_camera = serializedObject.FindProperty("m_camera");
        m_layersToCheck = serializedObject.FindProperty("m_layersToCheck");
    }

    public override void OnInspectorGUI() {
        CameraObstacleCheck script = (CameraObstacleCheck)target;
        serializedObject.Update();

        EditorGUILayout.PropertyField(m_player, new GUIContent("Object to check visibility"), true);
        if (m_player.objectReferenceValue == null) {
            EditorGUILayout.HelpBox("There is no Object Selected", MessageType.Error);
            serializedObject.ApplyModifiedProperties();
            return;
        }
        EditorGUILayout.PropertyField(m_camera, new GUIContent("Camera"), true);
        if (m_camera.objectReferenceValue == null) {
            EditorGUILayout.HelpBox("There is no Camera Selected", MessageType.Error);
            serializedObject.ApplyModifiedProperties();
            return;
        }
        EditorGUILayout.PropertyField(m_layersToCheck, new GUIContent("Layers To Check"), true);

        serializedObject.ApplyModifiedProperties();
    }
}
#endif
#endregion