using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class BarController : MonoBehaviour
{
    public enum Bar_Types {
        HORIZONTAL,
        VERTICAL,
        CIRCULAR
    }

    [SerializeField] private Bar_Types m_type;
    [SerializeField] private Image[] m_bars;
 
    [SerializeField] private float m_maxValue = 1.0f;
    [SerializeField] private float m_iniValue = 1.0f;
    private float m_actValue;
    private float m_newValue;
    private Task m_task;
    private float m_distance = 0f;
    [SerializeField] private bool m_animate = false;
    [SerializeField] private float m_transitionTime;
    [SerializeField] private bool m_sprited = false;

    public void Start() {
        switch (m_type) {
            case Bar_Types.HORIZONTAL:
                foreach (Image img in m_bars) {
                    img.type = Image.Type.Filled;
                    img.fillMethod = Image.FillMethod.Horizontal;
                }
            break;

            case Bar_Types.CIRCULAR:
                foreach (Image img in m_bars) {
                    img.type = Image.Type.Filled;
                    img.fillMethod = Image.FillMethod.Radial360;
                }
            break;
            
            case Bar_Types.VERTICAL:
                foreach (Image img in m_bars) {
                    img.type = Image.Type.Filled;
                    img.fillMethod = Image.FillMethod.Vertical;
                }
            break;
        }

        m_actValue = m_newValue = m_iniValue;
        if (m_sprited) {
            foreach (Image img in m_bars) img.fillAmount = 0f;
            float nImg = m_bars.Length * (m_actValue/m_maxValue);
            for (int cont = 0; nImg > 0; nImg--) {
                m_bars[cont].fillAmount = nImg;
                cont++;
            }
        }
        else foreach (Image img in m_bars) img.fillAmount = m_actValue/m_maxValue;
    }

    public void setValue(float value) {
        m_newValue = value;

        if (m_sprited) updateSpritedDisplay();
        else updateDisplay();
    }

    public void updateValue(float value) {
        m_newValue = Mathf.Clamp(m_newValue + value, 0f, m_maxValue);

        if (m_sprited) updateSpritedDisplay();
        else updateDisplay();
    }

    private async Task updateAnimatedDisplay() {
        while(m_actValue != m_newValue) {
            m_actValue = Mathf.MoveTowards(m_actValue, m_newValue, m_distance * Time.deltaTime * (1f/m_transitionTime));
            foreach (Image img in m_bars) img.fillAmount = m_actValue/m_maxValue;
            await Task.Yield();
        }
    }

    private async Task updateAnimatedSpritedDisplay() {
        while(m_actValue != m_newValue) {
            m_actValue = Mathf.MoveTowards(m_actValue, m_newValue, m_distance * Time.deltaTime * (1f/m_transitionTime));
            foreach (Image img in m_bars) img.fillAmount = 0f;
            float nImg = m_bars.Length * (m_actValue/m_maxValue);
            for (int cont = 0; nImg > 0; nImg--) {
                m_bars[cont].fillAmount = nImg;
                cont++;
            }

            await Task.Yield();
        }
    }

    private void updateDisplay() {
        if (m_animate) {
            m_distance = Mathf.Abs(m_actValue - m_newValue);
            if (m_task == null || m_task.IsCompleted) m_task = updateAnimatedDisplay();
        }
        else {
            m_actValue = m_newValue;
            foreach (Image img in m_bars) img.fillAmount = m_actValue/m_maxValue;
        }
    }

    private void updateSpritedDisplay() {
        if (m_animate) {
            m_distance = Mathf.Abs(m_actValue - m_newValue);
            if (m_task == null || m_task.IsCompleted) m_task = updateAnimatedSpritedDisplay();
        }
        else {
            m_actValue = m_newValue;
            foreach (Image img in m_bars) img.fillAmount = 0f;
            float nImg = m_bars.Length * (m_actValue/m_maxValue);
            for (int cont = 0; nImg > 0; nImg--) {
                m_bars[cont].fillAmount = nImg;
                cont++;
            }
        }
    }

    public void ChangeAnimation() => m_animate = !m_animate;
    public void ChangeAnimation(bool val) => m_animate = val;

    public bool IsEmpty() => m_actValue == 0;
}

#region Editor
#if UNITY_EDITOR
[CustomEditor(typeof(BarController))]
class BarControllerEditor : Editor {


    SerializedProperty m_type, m_bars, m_maxValue, m_iniValue, m_animate, m_transitionTime, m_sprited;

    private void OnEnable() {
        m_type = serializedObject.FindProperty("m_type");
        m_bars = serializedObject.FindProperty("m_bars");
        m_maxValue = serializedObject.FindProperty("m_maxValue");
        m_iniValue = serializedObject.FindProperty("m_iniValue");
        m_animate = serializedObject.FindProperty("m_animate");
        m_sprited = serializedObject.FindProperty("m_sprited");
        m_transitionTime = serializedObject.FindProperty("m_transitionTime");
    }

    public override void OnInspectorGUI() {
        BarController script = (BarController)target;
        serializedObject.Update();

        EditorGUILayout.PropertyField(m_type, new GUIContent("Bar Type"), true);
        EditorGUILayout.PropertyField(m_bars, new GUIContent("Images to represent the bar"), true);

        EditorGUILayout.PropertyField(m_maxValue, new GUIContent("Max Value"), true);
        if (m_maxValue.floatValue < 0) m_maxValue.floatValue = 0; 

        EditorGUILayout.PropertyField(m_iniValue, new GUIContent("Initial Value"), true);
        if (m_iniValue.floatValue > m_maxValue.floatValue) m_iniValue.floatValue = m_maxValue.floatValue;
        if (m_iniValue.floatValue < 0.0f) m_iniValue.floatValue = 0;

        EditorGUILayout.PropertyField(m_animate, new GUIContent("Animate Transition"), true);
        
        EditorGUI.indentLevel++;
        if (m_animate.boolValue) {
            EditorGUILayout.PropertyField(m_transitionTime, new GUIContent("Transition time"), true);
            if (m_transitionTime.floatValue < 0.0f) m_transitionTime.floatValue = 0.0f;
        }
        EditorGUI.indentLevel--;

        EditorGUILayout.PropertyField(m_sprited, new GUIContent("Composed Bar"), true);

        serializedObject.ApplyModifiedProperties();
    }
}
#endif
#endregion