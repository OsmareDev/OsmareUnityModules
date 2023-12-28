using System.Collections.Generic;
using UnityEngine;
using System.Linq;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class AimController2D : MonoBehaviour
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
    
    [SerializeField] private Transform m_rotationCenter;
    [SerializeField] private float m_rotationVelocity = 500f;

    private Vector2 m_lookDirection, m_lastDirection;
    private bool m_shootedThisFrame;
    private GameObject m_target;

    private void Start() => m_lastDirection = transform.up;

    private void Update() {
        ApplyRotation(GetDirectionToLookAt());  
    }

    #region Inputs
    public void NextShootType() => AimType = Helpers.GetNextInEnum<AimTypeEnum>((int)AimType);

    public void SetAimDirection( Vector2 lookDirection, Vector2 moveDirection = default) => m_lookDirection = (lookDirection == Vector2.zero) ? moveDirection : lookDirection; 
    public void SetTarget(GameObject target) => m_target = target;

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
        // The default action is aiming in the looking direction, if there is none then will be the same as last frame
        Vector2 directionToAimFor = (m_lookDirection != Vector2.zero) ? m_lookDirection : m_lastDirection;
        switch(AimType) {
            case AimTypeEnum.FreeAim:
            break;

            case AimTypeEnum.AssistedAim:
            if (m_target) {
                directionToAimFor = (m_target.transform.position - transform.parent.transform.position).normalized;
                m_target = null;
            } else {
                Collider2D closest = GetClosestEnemyInRange();
                if (closest) directionToAimFor = (closest.transform.position - transform.parent.transform.position).normalized;
            }
            break;

            case AimTypeEnum.AimIn4Directions:
            directionToAimFor = ConvertAimInNDirection(directionToAimFor, 4);
            break;

            case AimTypeEnum.AimIn8Directions:
            directionToAimFor = ConvertAimInNDirection(directionToAimFor, 8);
            break;

            case AimTypeEnum.AimInNDirections:
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
                return Quaternion.AngleAxis(i, m_rotationCenter.forward) * m_rotationCenter.up;
            }
        }

        return aimDirection;
    }

    private void ApplyRotation(Vector2 directionToFollow) {
        float angle = (Vector2.SignedAngle(transform.localPosition.normalized, directionToFollow));
        if (m_rotationVelocity > 0) angle = Mathf.MoveTowardsAngle(0, angle, m_rotationVelocity * Time.deltaTime);
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
[CustomEditor(typeof(AimController2D))]
class AimController2DEditor : Editor {
    SerializedProperty m_enemyLayer, m_maxDistanceToDetect, m_rotationCenter, m_rotationVelocity, m_takeWallsIntoAccount, m_wallLayer, m_numberOfDirections;

    private void OnEnable() {
        m_enemyLayer = serializedObject.FindProperty("m_enemyLayer");
        m_maxDistanceToDetect = serializedObject.FindProperty("m_maxDistanceToDetect");
        m_rotationCenter = serializedObject.FindProperty("m_rotationCenter");
        m_rotationVelocity = serializedObject.FindProperty("m_rotationVelocity");
        m_takeWallsIntoAccount = serializedObject.FindProperty("m_takeWallsIntoAccount");
        m_wallLayer = serializedObject.FindProperty("m_wallLayer");
        m_numberOfDirections = serializedObject.FindProperty("m_numberOfDirections");
    }
    
    public override void OnInspectorGUI() {
        AimController2D script = (AimController2D)target;
        serializedObject.Update();

        script.AimType = (AimController2D.AimTypeEnum)EditorGUILayout.EnumPopup("Aim Type :", script.AimType);

        if (script.AimType == AimController2D.AimTypeEnum.AssistedAim) {
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

        if (script.AimType == AimController2D.AimTypeEnum.AimInNDirections) {
            EditorGUI.indentLevel++;
            EditorGUILayout.PropertyField(m_numberOfDirections, true);
            if (m_numberOfDirections.intValue < 2) m_numberOfDirections.intValue = 2;
            EditorGUI.indentLevel--;
        }

        EditorGUILayout.PropertyField(m_rotationCenter, true);
        if (m_rotationCenter.objectReferenceValue == null) m_rotationCenter.objectReferenceValue = script.transform;
        EditorGUILayout.PropertyField(m_rotationVelocity, true);
        if (m_rotationVelocity.floatValue <= 0) {
            m_rotationVelocity.floatValue = 0;
            EditorGUILayout.HelpBox("At 0 speed the movement will be instantaneous, if you want to lock the rotation...", MessageType.Warning);
        }
        

        if (GUI.changed) EditorUtility.SetDirty(target);
        serializedObject.ApplyModifiedProperties();
    }
}
#endif
#endregion