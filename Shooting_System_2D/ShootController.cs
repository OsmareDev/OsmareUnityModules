using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class ShootController : MonoBehaviour
{
    [SerializeField] private Transform m_directionToShoot;
    [SerializeField] private Transform m_launchPosition; //
    [SerializeField] private AxisDirection m_axisDirection;

    [SerializeField] private BulletManager m_bulletManager;
    [SerializeField] private BaseBullet m_ammoType;

    // [SerializeField] private float m_shootAngle = 0;
    // [SerializeField] private int m_nBulletsPerShoot = 1;
    // [SerializeField] private bool m_randomPositionInRange = false;

    [SerializeField] private WeaponStats m_stats;

    private Vector2 m_direction;
    private float m_lastBulletTime = 0;

    #region UnityFunctions

    private void OnDrawGizmos() {
        Gizmos.color = Color.green;
        float beginAngle = -(m_stats.shootAngle / 2f);

        if (!Application.isPlaying) LoadDirection();
        // draw the range of shooting
        Gizmos.DrawRay(transform.position, Quaternion.AngleAxis(beginAngle, -m_directionToShoot.forward) * m_direction * 2);
        Gizmos.DrawRay(transform.position, Quaternion.AngleAxis(-beginAngle, -m_directionToShoot.forward) * m_direction * 2);
    }
    #endregion

    #region DirectionCalculation
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

    private Vector2 CalculateDirection(float beginAngle, float distributionAngle, int iteration) {
        float finalAngle = beginAngle;
        if (m_stats.randomPositionInRange) {
            finalAngle += (float)(new System.Random().NextDouble() * m_stats.shootAngle);
        } else {
            finalAngle += (distributionAngle/2f) + (iteration) * distributionAngle;
        }
        
        // TODO: make it possible for you to choose the angle in which it is rotated
        return Quaternion.AngleAxis(finalAngle, -m_directionToShoot.forward) * m_direction;
    }
    #endregion

    public void Shoot() {
        if (m_lastBulletTime > Time.time) return;
        m_lastBulletTime = Time.time + (1f / m_stats.fireRate);

        LoadDirection();
        
        float beginAngle = -(m_stats.shootAngle / 2f);
        float distributedAngle = (m_stats.shootAngle / (float)m_stats.nBulletsPerShoot);

        for (int i = 0; i < m_stats.nBulletsPerShoot; ++i) {
            Vector2 direction = CalculateDirection(beginAngle, distributedAngle, i);

            float timeToLive = m_stats.range / m_stats.bulletSpeed;
            BaseBullet go = m_bulletManager.GetBullet(m_ammoType, timeToLive);
            go?.Shooted(m_launchPosition.position, direction, m_stats);
        }
    }
}

#region Editor
#if UNITY_EDITOR
[CustomEditor(typeof(ShootController))]
class ShootControllerEditor : Editor {
    SerializedProperty m_directionToShoot, m_axisDirection, m_bulletManager, m_ammoType, m_launchPosition, m_shootAngle, 
        m_nBulletsPerShoot, m_randomPositionInRange, m_stats;

    private void OnEnable() {
        m_axisDirection = serializedObject.FindProperty("m_axisDirection");
        m_directionToShoot = serializedObject.FindProperty("m_directionToShoot");
        m_bulletManager = serializedObject.FindProperty("m_bulletManager");
        m_ammoType = serializedObject.FindProperty("m_ammoType");
        m_launchPosition = serializedObject.FindProperty("m_launchPosition");
        m_shootAngle = serializedObject.FindProperty("m_shootAngle");
        m_nBulletsPerShoot = serializedObject.FindProperty("m_nBulletsPerShoot");
        m_randomPositionInRange = serializedObject.FindProperty("m_randomPositionInRange");
        m_stats = serializedObject.FindProperty("m_stats");
    }

    public override void OnInspectorGUI() {
        ShootController script = (ShootController)target;
        serializedObject.Update();

        EditorGUILayout.PropertyField(m_stats, true);
        EditorGUILayout.Space();
        EditorGUILayout.PropertyField(m_directionToShoot, true);
        if (m_directionToShoot.objectReferenceValue == null) m_directionToShoot.objectReferenceValue = script.transform;
        EditorGUILayout.PropertyField(m_launchPosition, true);
        if (m_launchPosition.objectReferenceValue == null) m_launchPosition.objectReferenceValue = script.transform;
        
        EditorGUILayout.PropertyField(m_axisDirection, true);
        
        EditorGUILayout.PropertyField(m_bulletManager, true);
        EditorGUILayout.PropertyField(m_ammoType, true);

        serializedObject.ApplyModifiedProperties();
    }
}
#endif
#endregion