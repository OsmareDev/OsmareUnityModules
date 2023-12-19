using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;
using TMPro;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class GameSceneManager : Singleton<GameSceneManager>
{
    [SerializeField] private GameObject m_loadingCanvas;
    [SerializeField] private GameObject[] m_deactivateObjects;
    [SerializeField] private BarController m_progresBar;
    [SerializeField] private TMP_Text m_text;

    public async void LoadScene(string scene) {
        AsyncOperation op = SceneManager.LoadSceneAsync(scene);
        op.allowSceneActivation = false;

        m_loadingCanvas.SetActive(true);
        foreach (GameObject go in m_deactivateObjects) go.SetActive(false);

        m_progresBar?.setValue(0);

        if (m_text) m_text.text = scene;
        AudioManager.Instance.StopMusic();

        // unity takes the charge up to 90% only
        do {
            m_progresBar?.setValue(op.progress); 

            // the delays are set to be able to see the bars in motion, because the majority of the scene load instantly
            // await Task.Yield();
            await Task.Delay(200);
        } while (op.progress < 0.9f);

        m_progresBar?.setValue(op.progress); 

        // to see the load only
        await Task.Delay(1000);

        op.allowSceneActivation = true;
        m_loadingCanvas.SetActive(false);
    }
}

#region Editor
#if UNITY_EDITOR
[CustomEditor(typeof(GameSceneManager))]
class GameSceneManagerEditor : Editor {

    SerializedProperty m_loadingCanvas, m_deactivateObjects, m_progresBar, m_text;

    private void OnEnable() {
        m_loadingCanvas = serializedObject.FindProperty("m_loadingCanvas");
        m_deactivateObjects = serializedObject.FindProperty("m_deactivateObjects");
        m_progresBar = serializedObject.FindProperty("m_progresBar");
        m_text = serializedObject.FindProperty("m_text");
    }

    public override void OnInspectorGUI() {
        GameSceneManager script = (GameSceneManager)target;
        serializedObject.Update();

        EditorGUILayout.PropertyField(m_loadingCanvas, new GUIContent("Loading Screen Canvas"), true);
        if (m_loadingCanvas.objectReferenceValue == null) {
            EditorGUILayout.HelpBox("There is no Canvas attached", MessageType.Warning);
        }

        EditorGUI.indentLevel++;

        EditorGUILayout.PropertyField(m_deactivateObjects, new GUIContent("Objects to deactivate"), true);
        EditorGUILayout.PropertyField(m_progresBar, true);
        EditorGUILayout.PropertyField(m_text, true);

        EditorGUI.indentLevel--;

        serializedObject.ApplyModifiedProperties();
    }
}
#endif
#endregion
