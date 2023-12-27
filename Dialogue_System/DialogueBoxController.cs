using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using System.Linq;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class DialogueBoxController : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] private TMP_Text m_text;

    [SerializeField] private bool m_useTypeEffect;
    [SerializeField] private Object m_typeEffect;

    private BasicNode m_currentNode;

    [SerializeField] private GameObject m_optionsGO;
    [SerializeField] private GameObject m_optionPrefab;

    [SerializeField] private Image m_image;
    [SerializeField] private List<InputActionReference> m_actionsToInteract;

    public void NextDialogue(BasicNode node) {

        if (node == null) {
            StopDialogue();
            return;
        }

        TryToFinishTyping();
        m_currentNode = node;
        DeleteOptions();

        if (m_text != null) m_text.text = node.text;
        if (m_image != null && node.characterImage != null) m_image.sprite = node.characterImage;

        // check if there are options
        if (node is ChoiceNode && m_optionsGO != null && m_optionPrefab != null) {
            ChoiceNode currentNode = (ChoiceNode)node;
            m_optionsGO.SetActive(true);
            
            for (int i = 0; i < currentNode.choices.Length; ++i) {
                GameObject go = Instantiate(m_optionPrefab, m_optionsGO.transform);
                go.GetComponentInChildren<TMP_Text>().text = currentNode.choices[i].choice;

                BasicNode nextNode = currentNode.choices[i].answer;
                go.GetComponentInChildren<Button>().onClick.AddListener( () => NextDialogue(nextNode) );
                go.GetComponentInChildren<Button>().onClick.AddListener( () => DeleteOptions() );
            } 
        }

        // check if we need to use a type effect
        if (m_useTypeEffect) ((ITypeEffect)m_typeEffect)?.Begin(node.text);
    }

    private void DeleteOptions() {
        for (int i = m_optionsGO.transform.childCount - 1; i >= 0; i--)
            GameObject.Destroy(m_optionsGO.transform.GetChild(i).gameObject);

        m_optionsGO.SetActive(false);
    }

    private void StopDialogue() {
        DeleteOptions();
        gameObject.SetActive(false);
    }

    private bool TryToFinishTyping() {
        if (m_useTypeEffect && !((ITypeEffect)m_typeEffect).Check()) {
            ((ITypeEffect)m_typeEffect)?.Finish();
            return true;
        }

        return false;
    } 

    public void UpdateDialogue() {
        if (TryToFinishTyping()) return;

        if (m_currentNode is DialogueNode) {
            NextDialogue(((DialogueNode)m_currentNode).nextNode);
        }
    }

    public void OnPointerDown(PointerEventData eventData) {
        UpdateDialogue();
    }

    public void Update() {
        if (m_actionsToInteract.Count( action => action.action.triggered) > 0) {
            UpdateDialogue();
        }
    }
}

#region Editor
#if UNITY_EDITOR
[CustomEditor(typeof(DialogueBoxController))]
class DialogueBoxControllerEditor : Editor {

    SerializedProperty m_text, m_useTypeEffect, m_typeEffect, m_optionsGO, m_optionPrefab, m_image, m_actionsToInteract;

    private void OnEnable() {
        m_text = serializedObject.FindProperty("m_text");

        m_useTypeEffect = serializedObject.FindProperty("m_useTypeEffect");
        m_typeEffect = serializedObject.FindProperty("m_typeEffect");

        m_optionsGO = serializedObject.FindProperty("m_optionsGO");
        m_optionPrefab = serializedObject.FindProperty("m_optionPrefab");
        m_image = serializedObject.FindProperty("m_image");

        m_actionsToInteract = serializedObject.FindProperty("m_actionsToInteract");
    }

    public override void OnInspectorGUI() {
        DialogueBoxController script = (DialogueBoxController)target;
        serializedObject.Update();

        EditorGUILayout.PropertyField(m_text, new GUIContent("Text to display dialogue"), true);
        if (m_text.objectReferenceValue == null) {
            EditorGUILayout.HelpBox("There is no Text attached", MessageType.Error);

            serializedObject.ApplyModifiedProperties();
            return;
        }

        EditorGUILayout.PropertyField(m_useTypeEffect, new GUIContent("Use a type effect"), true);
        if (m_useTypeEffect.boolValue) {
            EditorGUI.indentLevel++;
            EditorHelpers.CollectInterface<ITypeEffect>(ref m_typeEffect, "Type effect");
            EditorGUI.indentLevel--;
        }

        EditorGUILayout.PropertyField(m_optionsGO, new GUIContent("Options Box"), true);
        if (m_optionsGO.objectReferenceValue != null) {
            EditorGUI.indentLevel++;
            EditorGUILayout.PropertyField(m_optionPrefab, new GUIContent("Button prefab for options"), true);
            EditorGUI.indentLevel--;
        } else {
            EditorGUILayout.HelpBox("There is no box for the options", MessageType.Warning);
        }

        EditorGUILayout.PropertyField(m_image, new GUIContent("Box for the image"), true);
        if (m_image.objectReferenceValue == null) {
            EditorGUILayout.HelpBox("There is no box for the image", MessageType.Warning);
        }

        EditorGUILayout.PropertyField(m_actionsToInteract, new GUIContent("Actions to update text"), true);
        if (m_actionsToInteract.arraySize == 0) {
            EditorGUILayout.HelpBox("The click will be the only thing that will trigger te text update", MessageType.Warning);
        }

        serializedObject.ApplyModifiedProperties();
    }
}
#endif
#endregion