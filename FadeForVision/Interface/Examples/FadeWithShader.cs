using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class FadeWithShader : MonoBehaviour, IFadeObject
{
    [SerializeField] private float _fadeDuration = 0.3f;
    [SerializeField] private float _transitionDuration = 0.3f;
    [SerializeField] private MeshRenderer _material;

    [SerializeField] private float _originalFadeValue = 1f;
    [SerializeField] private float _targetFade = 0.1f;

    private float _fadeLeft = 0;
    private Task _task;

    public void Awake() => _task = Task.FromResult(0);

    public void Fade() {
        _fadeLeft = _fadeDuration;

        if (_task.IsCompleted) {
            _task = FadeShader();
        }
    }

    private async Task FadeShader() {
        await TransitionShader(_targetFade, _transitionDuration);

        while (_fadeLeft > 0) {
            _fadeLeft -= Time.deltaTime;
            await Task.Yield();
        }

        await TransitionShader(_originalFadeValue, _transitionDuration);
    }

    private async Task TransitionShader(float target, float time) {
        float iniValue = _material.material.GetFloat("_Transparency");

        for (float t = 0; t < time; t += Time.deltaTime) {
            float nTime = t/time;
            float final = Mathf.Lerp(iniValue, target, nTime);

            _material.material.SetFloat("_Transparency", final);
            await Task.Yield();
        }
        _material.material.SetFloat("_Transparency", target);
    }
}

#region Editor
#if UNITY_EDITOR
[CustomEditor(typeof(FadeWithShader))]
class FadeWithShaderEditor : Editor {

    SerializedProperty m_fadeDuration, m_transitionDuration, m_material, m_originalFadeValue, m_targetFade;

    private void OnEnable() {
        m_fadeDuration = serializedObject.FindProperty("m_fadeDuration");
        m_transitionDuration = serializedObject.FindProperty("m_transitionDuration");
        m_material = serializedObject.FindProperty("m_material");
        m_originalFadeValue = serializedObject.FindProperty("m_originalFadeValue");
        m_targetFade = serializedObject.FindProperty("m_targetFade");
    }

    public override void OnInspectorGUI() {
        FadeWithShader script = (FadeWithShader)target;
        serializedObject.Update();

        EditorGUILayout.PropertyField(m_fadeDuration, new GUIContent("Duration of the fading"), true);
        if (m_fadeDuration.floatValue < 0) m_fadeDuration.floatValue = 0;
        EditorGUILayout.PropertyField(m_transitionDuration, new GUIContent("Duration of the effect"), true);
        if (m_transitionDuration.floatValue < 0) m_transitionDuration.floatValue = 0;
        EditorGUILayout.PropertyField(m_material, new GUIContent("Mesh Renderer of the shader"), true);
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