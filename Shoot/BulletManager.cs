using System;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class BulletManager : MonoBehaviour
{
    [SerializeField] private int m_defaultNumberObjects = 10;
    [SerializeField] private int m_maxNumberObjects = 100;
    [SerializeField] private float m_timeUntilDestroy = 5;
    private TimedObjectPool<BaseBullet> m_pool;

    private void Start() { 
        m_pool = new TimedObjectPool<BaseBullet>(m_defaultNumberObjects, m_maxNumberObjects, m_timeUntilDestroy);
        m_pool.StartSweep(0.1f);
    }

    public T GetBullet<T>(T prefab, float timeToLive) where T : BaseBullet {
        T bullet = m_pool.GetElement(prefab, timeToLive);
        if (bullet) bullet.m_deactivationFunction = ReleaseBullet;
        return bullet;
    }

    public void ReleaseBullet(BaseBullet bullet) => m_pool.TryReleaseElement(bullet);
}

#region Editor
#if UNITY_EDITOR
[CustomEditor(typeof(BulletManager))]
class BulletManagerEditor : Editor {
    SerializedProperty m_defaultNumberObjects, m_maxNumberObjects, m_timeUntilDestroy;

    private void OnEnable() {
        m_defaultNumberObjects = serializedObject.FindProperty("m_defaultNumberObjects");
        m_maxNumberObjects = serializedObject.FindProperty("m_maxNumberObjects");
        m_timeUntilDestroy = serializedObject.FindProperty("m_timeUntilDestroy");
    }

    public override void OnInspectorGUI() {
        BulletManager script = (BulletManager)target;
        serializedObject.Update();

        EditorGUILayout.PropertyField(m_defaultNumberObjects, true);
        if ( m_defaultNumberObjects.intValue <= 0 ) m_defaultNumberObjects.intValue = 0;

        EditorGUILayout.PropertyField(m_maxNumberObjects, true);
        if ( m_maxNumberObjects.intValue < 0 ) m_maxNumberObjects.intValue = 0;
        if ( m_defaultNumberObjects.intValue > m_maxNumberObjects.intValue ) m_defaultNumberObjects.intValue = m_maxNumberObjects.intValue;

        EditorGUILayout.PropertyField(m_timeUntilDestroy, true);
        if ( m_timeUntilDestroy.floatValue < 0 ) m_timeUntilDestroy.floatValue = 0;

        serializedObject.ApplyModifiedProperties();
    }
}
#endif
#endregion