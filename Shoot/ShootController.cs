using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class ShootController : MonoBehaviour
{
    [SerializeField] private Transform m_directionToShoot;
    [SerializeField] private AxisDirection m_axisDirection;

    [SerializeField] private GameObject m_bulletPrefab;
    [SerializeField] private Object m_inputManager; //IInputManager

    private Vector2 m_direction;

    public void Update() {
        if (((IInputManager)m_inputManager).ShootedThisFrame()) Shoot();
    }

    private void LoadDirection() {
        switch(m_axisDirection) {
            default:
            case AxisDirection.Up:
            m_direction = m_directionToShoot.up;
            break;

            case AxisDirection.Down:
            m_direction = -m_directionToShoot.up;
            break;

            case AxisDirection.Right:
            m_direction = m_directionToShoot.right;
            break;

            case AxisDirection.Left:
            m_direction = -m_directionToShoot.right;
            break;

            case AxisDirection.Forward:
            m_direction = m_directionToShoot.forward;
            break;

            case AxisDirection.Backwards:
            m_direction = -m_directionToShoot.forward;
            break;
        }
    }

    public void Shoot() {
        LoadDirection();
        // hacer logica de disparo
    }
}

#region Editor
#if UNITY_EDITOR
[CustomEditor(typeof(ShootController))]
class ShootControllerEditor : Editor {
    SerializedProperty m_directionToShoot, m_axisDirection, m_bulletPrefab, m_inputManager;

    private void OnEnable() {
        m_axisDirection = serializedObject.FindProperty("m_axisDirection");
        m_directionToShoot = serializedObject.FindProperty("m_directionToShoot");
        m_bulletPrefab = serializedObject.FindProperty("m_bulletPrefab");
        m_inputManager = serializedObject.FindProperty("m_inputManager");
    }

    public override void OnInspectorGUI() {
        ShootController script = (ShootController)target;
        serializedObject.Update();

        EditorGUILayout.PropertyField(m_directionToShoot, true);
        if (m_directionToShoot.objectReferenceValue == null) m_directionToShoot.objectReferenceValue = script.transform;
        
        EditorGUILayout.PropertyField(m_axisDirection, true);
        
        EditorGUILayout.PropertyField(m_bulletPrefab, true);
        EditorHelpers.CollectInterface<IInputManager>(ref m_inputManager, "Input Manager ");

        serializedObject.ApplyModifiedProperties();
    }
}
#endif
#endregion