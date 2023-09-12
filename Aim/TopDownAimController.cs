using System.Collections.Generic;
using UnityEngine;
using System.Linq;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class TopDownAimController : MonoBehaviour
{
    public enum AimTypeEnum {
        FreeAim,
        AssistedAim,
        AimIn4Directions,
        AimIn8Directions,
        AimInNDirections
    }
    [field: SerializeField] public AimTypeEnum AimType { get; set; }

    [SerializeField] private LayerMask m_enemyLayer;
    [SerializeField] private float m_maxDistanceToDetect = 10f;
    [SerializeField] private bool m_takeWallsIntoAccount = false;
    [SerializeField] private LayerMask m_wallLayer;
    
    [SerializeField] private int m_numberOfDirections = 16;

    [SerializeField] private Object m_playerInput; //IInputManager
    
    [SerializeField] private Transform m_rotationCenter;
    [SerializeField] private float m_rotationVelocity = 500f;

    private Vector2 m_lookDirection, m_moveDirection, m_lastDirection;
    private bool m_shootedThisFrame;

    private void Start() => m_lastDirection = transform.up;

    private void Update() {
        GatherInput();
        ApplyRotation(GetDirectionToLookAt());  
    }

    #region Inputs
    public void NextShootType() => AimType = Helpers.GetNextInEnum<AimTypeEnum>((int)AimType);

    private void GatherInput() {
        m_moveDirection = (m_playerInput)? ((IInputManager)m_playerInput).GetMoveDirection() : default(Vector2);
        m_lookDirection = (m_playerInput)? ((IInputManager)m_playerInput).GetLookDirection() : default(Vector2);
        if (m_playerInput && ((IInputManager)m_playerInput).JumpedThisFrame()) NextShootType();
    }
    #endregion

    #region DirectionCalculation
    private Collider2D GetClosestEnemyInRange() {
        List<Collider2D> enemies = new List<Collider2D>(Physics2D.OverlapCircleAll(m_rotationCenter.position, m_maxDistanceToDetect, m_enemyLayer));
        Collider2D closest = enemies
            .Where( enemy => (!m_takeWallsIntoAccount) || !Physics2D.Raycast(m_rotationCenter.position, (enemy.transform.position - m_rotationCenter.position).normalized, (enemy.transform.position - m_rotationCenter.position).magnitude, m_wallLayer))
            .OrderBy( enemy => Vector2.Distance(enemy.transform.position, m_rotationCenter.position))
            .FirstOrDefault();
        return closest;
    }

    private Vector2 GetDirectionToLookAt() {
        Collider2D closest = GetClosestEnemyInRange();

        // The default action is aiming in the moving direction, if is not moving then the direction from the last time
        Vector2 directionToAimFor = (m_moveDirection != Vector2.zero) ? m_moveDirection : m_lastDirection;
        switch(AimType) {
            case AimTypeEnum.FreeAim:
            if (Input.mousePresent) directionToAimFor = (Helpers.CameraMain.ScreenToWorldPoint(Input.mousePosition) - m_rotationCenter.position).normalized;
            if (m_lookDirection != Vector2.zero) directionToAimFor = m_lookDirection;
            break;

            case AimTypeEnum.AssistedAim:
            if (closest) directionToAimFor = (closest.transform.position - transform.parent.transform.position).normalized;
            break;

            case AimTypeEnum.AimIn4Directions:
            if (m_lookDirection != Vector2.zero) directionToAimFor = m_lookDirection;
            directionToAimFor = ConvertAimInNDirection(directionToAimFor, 4);
            break;

            case AimTypeEnum.AimIn8Directions:
            if (m_lookDirection != Vector2.zero) directionToAimFor = m_lookDirection;
            directionToAimFor = ConvertAimInNDirection(directionToAimFor, 8);
            break;

            case AimTypeEnum.AimInNDirections:
            if (m_lookDirection != Vector2.zero) directionToAimFor = m_lookDirection;
            directionToAimFor = ConvertAimInNDirection(directionToAimFor, m_numberOfDirections);
            break;
        }
        m_lastDirection = directionToAimFor;

        return directionToAimFor;
    } 

    private Vector2 ConvertAimInNDirection(Vector2 aimDirection, int nDivisions) {
        float angle = Vector2.SignedAngle(Vector2.up, aimDirection);

        float divisionAngle = 360f/(float)nDivisions;
        for (float i = -180f; i <= 180f; i += divisionAngle) {
            if (angle <= i + divisionAngle/2 && angle >= i - divisionAngle/2) {
                Debug.Log(i);
                return Quaternion.AngleAxis(i, m_rotationCenter.forward) * m_rotationCenter.up;
            }
        }

        return aimDirection;
    }

    private void ApplyRotation(Vector2 directionToFollow) {
        float angle = (Vector2.SignedAngle(transform.localPosition.normalized, directionToFollow));
        angle = Mathf.MoveTowardsAngle(0, angle, m_rotationVelocity * Time.deltaTime);
        transform.RotateAround(transform.parent.transform.position, transform.forward, angle);
    }
    #endregion

    private void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.parent.transform.position, m_maxDistanceToDetect);
    }
}

#region Editor
#if UNITY_EDITOR
[CustomEditor(typeof(TopDownAimController))]
class TopDownAimControllerEditor : Editor {
    SerializedProperty m_enemyLayer, m_maxDistanceToDetect, m_playerInput, m_rotationCenter, m_rotationVelocity, m_takeWallsIntoAccount, m_wallLayer, m_numberOfDirections;

    private void OnEnable() {
        m_enemyLayer = serializedObject.FindProperty("m_enemyLayer");
        m_maxDistanceToDetect = serializedObject.FindProperty("m_maxDistanceToDetect");
        m_playerInput = serializedObject.FindProperty("m_playerInput");
        m_rotationCenter = serializedObject.FindProperty("m_rotationCenter");
        m_rotationVelocity = serializedObject.FindProperty("m_rotationVelocity");
        m_takeWallsIntoAccount = serializedObject.FindProperty("m_takeWallsIntoAccount");
        m_wallLayer = serializedObject.FindProperty("m_wallLayer");
        m_numberOfDirections = serializedObject.FindProperty("m_numberOfDirections");
    }
    
    public override void OnInspectorGUI() {
        TopDownAimController script = (TopDownAimController)target;
        serializedObject.Update();

        EditorHelpers.CollectInterface<IInputManager>(ref m_playerInput, "Input Manager ");
        if (m_playerInput.objectReferenceValue == null) {
            EditorGUILayout.HelpBox("if there is no playerInput only the free aim mode with mouse will work", MessageType.Warning);
        }

        script.AimType = (TopDownAimController.AimTypeEnum)EditorGUILayout.EnumPopup("Aim Type :", script.AimType);
        if (m_playerInput.objectReferenceValue == null) {
            script.AimType = TopDownAimController.AimTypeEnum.FreeAim;
        }

        if (script.AimType == TopDownAimController.AimTypeEnum.AssistedAim) {
            EditorGUI.indentLevel++;
            EditorGUILayout.PropertyField(m_enemyLayer, true);
            if (m_enemyLayer.intValue == 0) EditorGUILayout.HelpBox("There is no Enemy Layer selected", MessageType.Warning);
            EditorGUILayout.PropertyField(m_maxDistanceToDetect, true);
            EditorGUILayout.PropertyField(m_takeWallsIntoAccount, true);
            if (m_takeWallsIntoAccount.boolValue) {
                EditorGUI.indentLevel++;
                EditorGUILayout.PropertyField(m_wallLayer, true);
                if (m_wallLayer.intValue == 0) EditorGUILayout.HelpBox("There is no Wall Layer selected", MessageType.Warning);
                EditorGUI.indentLevel--;
            }
            EditorGUI.indentLevel--;
        }

        if (script.AimType == TopDownAimController.AimTypeEnum.AimInNDirections) {
            EditorGUI.indentLevel++;
            EditorGUILayout.PropertyField(m_numberOfDirections, true);
            if (m_numberOfDirections.intValue < 2) m_numberOfDirections.intValue = 2;
            EditorGUI.indentLevel--;
        }

        EditorGUILayout.PropertyField(m_rotationCenter, true);
        if (m_rotationCenter.objectReferenceValue == null) m_rotationCenter.objectReferenceValue = script.transform;
        EditorGUILayout.PropertyField(m_rotationVelocity, true);

        if (GUI.changed) EditorUtility.SetDirty(target);
        serializedObject.ApplyModifiedProperties();
    }
}
#endif
#endregion