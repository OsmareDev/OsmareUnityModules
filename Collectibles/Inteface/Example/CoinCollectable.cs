using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class CoinCollectable : MonoBehaviour, ICollectible
{
    [SerializeField] private TMP_Text m_text; 
    [SerializeField] private float m_secondsToCollect; 

    private bool m_alreadyCounted = false;
    private float m_interpolatedAmount = 0;
    private Vector3 a, b, c;
    private Transform d;

    public void Update() => CheckCollection();

    public void GetCollected(CollectibleCatcherController ccatcher = null)
    {
        if (!m_alreadyCounted) {
            m_alreadyCounted = true;

            GenerateSplinePoints(ccatcher.transform);
        }
    }

    private void CheckCollection() {
        if (m_alreadyCounted) {
            m_interpolatedAmount += 1 * (Time.deltaTime / m_secondsToCollect);
            transform.position = Helpers.CubicLerp(a, b, c, d.position, m_interpolatedAmount);
            if (m_interpolatedAmount > 1) {
                if (m_text != null) {
                    int coins = int.Parse(m_text.text);
                    coins++;
                    m_text.text = coins.ToString();
                }
                GameObject.Destroy(gameObject);
            }
        }
    }

    private void GenerateSplinePoints(Transform end) {
        a = transform.position;
        d = end;

        b = a - (d.position - a);
        c = new Vector3(transform.position.x + Random.Range(0.0f, 1.0f), transform.position.y + Random.Range(0.0f, 1.0f), transform.position.z);
    }
}

#region Editor
#if UNITY_EDITOR
[CustomEditor(typeof(CoinCollectable))]
class CoinCollectableEditor : Editor {

    SerializedProperty m_text, m_secondsToCollect;

    private void OnEnable() {
        m_text = serializedObject.FindProperty("m_text");
        m_secondsToCollect = serializedObject.FindProperty("m_secondsToCollect");
    }

    public override void OnInspectorGUI() {
        CoinCollectable script = (CoinCollectable)target;
        serializedObject.Update();

        EditorGUILayout.PropertyField(m_text, new GUIContent("Text to Update"), true);
        EditorGUILayout.PropertyField(m_secondsToCollect, new GUIContent("Seconds to collect"), true);

        serializedObject.ApplyModifiedProperties();
    }
}
#endif
#endregion