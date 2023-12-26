using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class JoyStickController : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    [SerializeField] private RectTransform m_body;
    [SerializeField] private GameObject m_cursor_ext;
    [SerializeField] private GameObject m_cursor_int;
    [SerializeField] private int m_tamCursor = 25;
    
    private Vector2 m_posClick;
    private Vector2 m_direction;

    public void Awake() {
        m_cursor_ext.SetActive(false);
        m_cursor_int.SetActive(false);
        m_direction = Vector2.zero;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 m_posAct = Input.mousePosition;
        m_posAct = (m_posAct - m_posClick);
        if (m_posAct.magnitude > m_tamCursor)  {
            m_cursor_int.transform.position = m_posClick + m_posAct.normalized * m_tamCursor; 
            m_direction = m_posAct.normalized;
        }
        else {
            m_cursor_int.transform.position = m_posClick + m_posAct; 
            m_direction = m_posAct / m_tamCursor;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        m_posClick = Input.mousePosition;

        m_cursor_ext.SetActive(true);
        m_cursor_ext.transform.position = m_posClick;
        m_cursor_int.SetActive(true);
        m_cursor_int.transform.position = m_posClick;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        m_cursor_ext.SetActive(false);
        m_cursor_int.SetActive(false);
        m_direction = new Vector2(0,0);
    }

    public Vector2 GetValue() => m_direction;
}

#region Editor
#if UNITY_EDITOR
[CustomEditor(typeof(JoyStickController))]
class JoyStickControllerEditor : Editor {

    SerializedProperty m_body, m_cursor_ext, m_cursor_int, m_tamCursor;

    private void OnEnable() {
        m_body = serializedObject.FindProperty("m_body");
        m_cursor_ext = serializedObject.FindProperty("m_cursor_ext");
        m_cursor_int = serializedObject.FindProperty("m_cursor_int");
        m_tamCursor = serializedObject.FindProperty("m_tamCursor");
    }

    public override void OnInspectorGUI() {
        JoyStickController script = (JoyStickController)target;
        serializedObject.Update();

        EditorGUILayout.PropertyField(m_body, new GUIContent("TouchScreen Canvas"), true);
        if (m_body.objectReferenceValue == null) {
            EditorGUILayout.HelpBox("There is no TouchScreen", MessageType.Error);

            serializedObject.ApplyModifiedProperties();
            return;
        }
        
        EditorGUI.indentLevel++;
        EditorGUILayout.PropertyField(m_cursor_ext, new GUIContent("Cursor Exterior Piece"), true);
        EditorGUILayout.PropertyField(m_cursor_int, new GUIContent("Cursor Interior Piece"), true);
        EditorGUILayout.PropertyField(m_tamCursor, new GUIContent("Size of the cursor"), true);
        EditorGUI.indentLevel--;

        serializedObject.ApplyModifiedProperties();
    }
}
#endif
#endregion