using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class FadeWithMaterial : MonoBehaviour, IFadeObject
{
    [SerializeField] private float m_fadeDuration = 0.3f;
    [SerializeField] private float m_transitionDuration = 0.3f;
    [SerializeField] private MeshRenderer m_material;

    [SerializeField] private float m_originalFadeValue = 1f;
    [SerializeField] private float m_targetFade = 0.1f;

    private float m_fadeLeft = 0;
    private Task m_task;

    public void Awake() => m_task = Task.FromResult(0);

    public void Fade() {
        m_fadeLeft = m_fadeDuration;

        if (m_task.IsCompleted) {
            m_task = FadeMaterial();
        }
    }

    private async Task FadeMaterial() {   
        await TransitionMaterial(m_targetFade, m_transitionDuration);

        while (m_fadeLeft > 0) {
            m_fadeLeft -= Time.deltaTime;
            await Task.Yield();
        }

        await TransitionMaterial(m_originalFadeValue, m_transitionDuration);
    }

    private async Task TransitionMaterial(float target, float time) {
        Color color = m_material.material.color;
        float iniValue = color.a;

        for (float t = 0; t < time; t += Time.deltaTime) {
            float nTime = t/time;
            float final = Mathf.Lerp(iniValue, target, nTime);

            color.a = final;
            m_material.material.color = color;
            await Task.Yield();
        }
        color.a = target;
        m_material.material.color = color;
    }
}

#region Editor
#if UNITY_EDITOR
[CustomEditor(typeof(FadeWithMaterial))]
class FadeWithMaterialEditor : Editor {

    SerializedProperty m_fadeDuration, m_transitionDuration, m_material, m_originalFadeValue, m_targetFade;

    private void OnEnable() {
        m_fadeDuration = serializedObject.FindProperty("m_fadeDuration");
        m_transitionDuration = serializedObject.FindProperty("m_transitionDuration");
        m_material = serializedObject.FindProperty("m_material");
        m_originalFadeValue = serializedObject.FindProperty("m_originalFadeValue");
        m_targetFade = serializedObject.FindProperty("m_targetFade");
    }

    public override void OnInspectorGUI() {
        FadeWithMaterial script = (FadeWithMaterial)target;
        serializedObject.Update();

        EditorGUILayout.PropertyField(m_fadeDuration, new GUIContent("Duration of the fading"), true);
        if (m_fadeDuration.floatValue < 0) m_fadeDuration.floatValue = 0;
        EditorGUILayout.PropertyField(m_transitionDuration, new GUIContent("Duration of the effect"), true);
        if (m_transitionDuration.floatValue < 0) m_transitionDuration.floatValue = 0;
        EditorGUILayout.PropertyField(m_material, new GUIContent("Mesh Renderer of the material"), true);
        if (m_material.objectReferenceValue == null) {
            EditorGUILayout.HelpBox("There is no Mesh Renderer", MessageType.Error);
            serializedObject.ApplyModifiedProperties();
            return;
        }
        EditorGUILayout.PropertyField(m_originalFadeValue, new GUIContent("Original Fade Value"), true);
        if (m_originalFadeValue.floatValue < 0) m_originalFadeValue.floatValue = 0;
        if (m_originalFadeValue.floatValue > 1) m_originalFadeValue.floatValue = 1;
        EditorGUILayout.PropertyField(m_targetFade, new GUIContent("Target Fade Value"), true);
        if (m_targetFade.floatValue < 0) m_targetFade.floatValue = 0;
        if (m_targetFade.floatValue > 1) m_targetFade.floatValue = 1;

        serializedObject.ApplyModifiedProperties();
    }
}
#endif
#endregion