using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

#if UNITY_EDITOR
using UnityEditor;
#endif

// the CollectibleCatcher name doesnt let me change the editor 
public class CollectibleCatcherController : MonoBehaviour
{
    public enum Dimensions {
        _2D,
        _3D
    }

    [SerializeField] private bool m_useAlternative = false;
    [SerializeField] private float m_radius = 1.0f;
    [SerializeField] private Dimensions m_dimension;

    private Action m_checkFunction;

    #region UnityFunctions
    public void Start() => LoadChecks();
    public void Update() => m_checkFunction();
    private void OnDrawGizmos() {
        if (Selection.activeGameObject != gameObject) return;

        Gizmos.DrawWireSphere(transform.position, m_radius);
    }
    #endregion

    #region Alternative
    private void LoadChecks() {
        if (!m_useAlternative) {
            m_checkFunction = Helpers.DoNothing;
            return;
        }

        if (m_dimension == Dimensions._2D) m_checkFunction = Check2D;
        else m_checkFunction = Check3D;
    }

    private void Check2D() {
        List<Collider2D> colliders = new List<Collider2D>(Physics2D.OverlapCircleAll(transform.position, m_radius));
        colliders.ForEach( collider => {
            if (collider.gameObject.TryGetComponent<ICollectible>(out ICollectible collectable)) collectable.GetCollected(this);
        });
    }

    private void Check3D() {
        List<Collider> colliders = new List<Collider>(Physics.OverlapSphere(transform.position, m_radius));
        colliders.ForEach( collider => {
            if (collider.gameObject.TryGetComponent<ICollectible>(out ICollectible collectable)) collectable.GetCollected(this);
        });
    }
    #endregion


    #region CharacterController
    // This will not function without a character controller
    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.TryGetComponent<ICollectible>(out ICollectible collectable)) collectable.GetCollected(this);
    }
    #endregion

    #region Rigidbody
    // This will not function without a rigidbody
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent<ICollectible>(out ICollectible collectable)) collectable.GetCollected(this);
    }
    #endregion
}

#region Editor
#if UNITY_EDITOR
[CustomEditor(typeof(CollectibleCatcherController))]
class CollectibleCatcherControllerEditor : Editor {

    SerializedProperty m_useAlternative, m_radius, m_dimension;

    private void OnEnable() {
        m_useAlternative = serializedObject.FindProperty("m_useAlternative");

        m_radius = serializedObject.FindProperty("m_radius");
        m_dimension = serializedObject.FindProperty("m_dimension");
    }

    public override void OnInspectorGUI() {
        CollectibleCatcherController script = (CollectibleCatcherController)target;
        serializedObject.Update();

        EditorGUILayout.PropertyField(m_useAlternative, new GUIContent("Use Alternative Check Method"), true);
        if (m_useAlternative.boolValue) {
            EditorGUI.indentLevel++;
            EditorGUILayout.PropertyField(m_dimension, new GUIContent("Dimensions"), true);
            EditorGUILayout.PropertyField(m_radius, new GUIContent("Radius"), true);
            EditorGUI.indentLevel--;
        }

        serializedObject.ApplyModifiedProperties();
    }
}
#endif
#endregion