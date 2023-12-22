using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class DialogueNodeController : MonoBehaviour
{
    [SerializeField] private DialogueBoxController m_dialogueGO;

    [SerializeField] private BasicNode[] m_initialTextNodes;
    [SerializeField] private bool m_useInOrder = false;
    private int m_currentInitialNode = -1; // make posible to load

    public void BeginDialogue() {
        SelectNode();
        
        m_dialogueGO?.gameObject.SetActive(true);
        m_dialogueGO.NextDialogue(m_initialTextNodes[m_currentInitialNode]);
    }

    public void LoadCurrentNode() {
        // ...
    }

    private void SelectNode() {
        int numberOfNodes = m_initialTextNodes.Length;

        if (numberOfNodes < 1) return;

        if (numberOfNodes == 1) {
            m_currentInitialNode = 0;
            return;
        }

        if (m_useInOrder) m_currentInitialNode = (m_currentInitialNode == numberOfNodes-1) ? m_currentInitialNode : m_currentInitialNode + 1;
        else m_currentInitialNode = Random.Range(0, numberOfNodes);
    }
}

#region Editor
#if UNITY_EDITOR
[CustomEditor(typeof(DialogueNodeController))]
class DialogueNodeControllerEditor : Editor {

    SerializedProperty m_dialogueGO, m_initialTextNodes, m_useInOrder;

    private void OnEnable() {
        m_dialogueGO = serializedObject.FindProperty("m_dialogueGO");

        m_initialTextNodes = serializedObject.FindProperty("m_initialTextNodes");
        m_useInOrder = serializedObject.FindProperty("m_useInOrder");
    }

    public override void OnInspectorGUI() {
        DialogueNodeController script = (DialogueNodeController)target;
        serializedObject.Update();
        
        EditorGUILayout.PropertyField(m_dialogueGO, new GUIContent("Dialogue box"), true);
        if (m_dialogueGO.objectReferenceValue == null) {
            EditorGUILayout.HelpBox("There is no Dialogue Box", MessageType.Error);

            serializedObject.ApplyModifiedProperties();
            return;
        }
        
        EditorGUILayout.PropertyField(m_initialTextNodes, new GUIContent("Initial Dialogues"), true);
        if (m_initialTextNodes.arraySize > 1) {
            EditorGUI.indentLevel++;
            EditorGUILayout.PropertyField(m_useInOrder, new GUIContent("Use in order"), true);
            EditorGUI.indentLevel--;
        }

        serializedObject.ApplyModifiedProperties();
    }
}
#endif
#endregion