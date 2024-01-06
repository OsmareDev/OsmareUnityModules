using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using System.Threading.Tasks;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class InterestScreenPoint : MonoBehaviour
{
    [SerializeField] private List<Transform> m_points;
    private IInteractable m_currentPoint;
    [SerializeField] private Transform m_visualizator;
    [SerializeField] private RectTransform m_canvas;
    [SerializeField] private float m_range = 20f;
    [SerializeField] private Camera m_camera;


    [SerializeField] private Image m_selectorInCanvas;
    [SerializeField] private float m_refreshSeconds = 0.1f;
    private bool m_running = true, m_inside = false;

    private TimedTask m_task;

    public void Start() {
        m_task = new TimedTask(ShowPoint);
        StartTask();
    }
    public void StartTask() {
        m_task.StartSweep(m_refreshSeconds);
    }
    public void StopTask() {
        m_task.StopSweep();
    }
    public void Interact() {
        if (m_currentPoint != null && m_running && m_inside) m_currentPoint.Interact();
    }
    public void SetPoint(Transform t) => m_points.Add(t);

    private void ShowPoint() {
        m_selectorInCanvas.enabled = false;
        m_inside = false;

        Transform m_nearestPointToShow = m_points.OrderBy(tr => Vector3.Distance(tr.position, m_visualizator.position)).Take(1).ToList()[0];
        Vector3 direction = m_nearestPointToShow.position - m_visualizator.position;
        
        // if the point is backwards dont calculate the position in screen, it may lead to bad results
        if (Vector3.Dot(direction, m_camera.transform.forward) < 0) return;

        RaycastHit hit;

        if (m_visualizator == null) return;

        if (Physics.Raycast(m_visualizator.position, direction.normalized, out hit, m_range) && hit.transform == m_nearestPointToShow) {
            hit.collider.TryGetComponent<IInteractable>(out m_currentPoint);

            // mostrar
            Vector2 screenPos = RectTransformUtility.WorldToScreenPoint(m_camera, m_nearestPointToShow.position);
            Debug.Log(screenPos);
            if (RectTransformUtility.RectangleContainsScreenPoint(m_canvas, screenPos)) {
                m_selectorInCanvas.rectTransform.position = screenPos;
                m_selectorInCanvas.enabled = true;
                m_inside = true;
            }
        }
    }
}

#region Editor
#if UNITY_EDITOR
[CustomEditor(typeof(InterestScreenPoint))]
class InterestScreenPointEditor : Editor {

    SerializedProperty m_points, m_visualizator, m_canvas, m_range, m_camera, m_selectorInCanvas, m_refreshSeconds;

    private void OnEnable() {
        m_points = serializedObject.FindProperty("m_points");
        m_visualizator = serializedObject.FindProperty("m_visualizator");
        m_canvas = serializedObject.FindProperty("m_canvas");
        m_range = serializedObject.FindProperty("m_range");
        m_camera = serializedObject.FindProperty("m_camera");
        m_selectorInCanvas = serializedObject.FindProperty("m_selectorInCanvas");
        m_refreshSeconds = serializedObject.FindProperty("m_refreshSeconds");
    }

    public override void OnInspectorGUI() {
        InterestScreenPoint script = (InterestScreenPoint)target;
        serializedObject.Update();

        EditorGUILayout.PropertyField(m_points, new GUIContent("Points of interest"), true);
        if (m_points.arraySize == 0) {
            EditorGUILayout.HelpBox("There is no Points of interest to show", MessageType.Warning);
        }

        EditorGUILayout.PropertyField(m_visualizator, new GUIContent("Transform of the visualizer"), true);
        if (m_visualizator.objectReferenceValue == null) {
            EditorGUILayout.HelpBox("There is no visualizer attached", MessageType.Error);
            serializedObject.ApplyModifiedProperties();
            return;
        }

        EditorGUILayout.PropertyField(m_canvas, new GUIContent("Canvas"), true);
        if (m_canvas.objectReferenceValue == null) {
            EditorGUILayout.HelpBox("There is no canvas attached", MessageType.Error);
            serializedObject.ApplyModifiedProperties();
            return;
        }

        EditorGUILayout.PropertyField(m_camera, new GUIContent("Camera"), true);
        if (m_camera.objectReferenceValue == null) {
            EditorGUILayout.HelpBox("There is no camera attached", MessageType.Error);
            serializedObject.ApplyModifiedProperties();
            return;
        }

        EditorGUILayout.PropertyField(m_selectorInCanvas, new GUIContent("Image Pointer"), true);
        if (m_selectorInCanvas.objectReferenceValue == null) {
            EditorGUILayout.HelpBox("There is no image", MessageType.Error);
            serializedObject.ApplyModifiedProperties();
            return;
        }

        EditorGUILayout.PropertyField(m_range, new GUIContent("Range to show points"), true);
        if (m_range.floatValue < 0.0f) m_range.floatValue = 0.0f;
        EditorGUILayout.PropertyField(m_refreshSeconds, new GUIContent("Refresh Seconds"), true);
        if (m_refreshSeconds.floatValue <= 0.0f) m_refreshSeconds.floatValue = float.MinValue;

        serializedObject.ApplyModifiedProperties();
    }
}
#endif
#endregion