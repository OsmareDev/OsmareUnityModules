using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class General2DCollisionController : MonoBehaviour
{
    // Debug Settings
    [SerializeField] private bool m_showRays;
    [SerializeField] private float m_durationOfRays = 2f;

    // Collision Settings
    [SerializeField] private Collider2D m_collider;
    private Vector2 m_colliderSize;
    private Func<Vector2, Vector2, float, RaycastHit2D> m_collisionFunction;
    [SerializeField] private float m_skinWidth = 0.01f;
    [SerializeField] private LayerMask m_collisionLayer;
    [SerializeField] private int m_maxIterations = 3;
    [Range(0f, 1f)][SerializeField] private float m_alignmentToSlide = 0.96f;

    // Gravity Settings
    [field: SerializeField] public bool TakeIntoAccountGravity { get; set; } = false;
    [field: SerializeField] public Transform BodyAxis { get; set; }

    public void Start() => LoadCollisionDetection();

    private Vector2 m_origDirection;
    public void Move(Vector2 v) {
        m_origDirection = v.normalized;
        Vector2 finalMovement = CheckCollisions(v);
        if (m_showRays) Debug.DrawLine(transform.position, transform.position + (Vector3)v, Color.magenta, m_durationOfRays);
        transform.position += (Vector3)finalMovement;
    }

    #region CollisionManagement
    Vector2 CheckCollisions(Vector2 movement, Vector2 displacement = new Vector2(), int iteration = 1) {
        if (iteration > m_maxIterations || movement.magnitude == 0) return Vector2.zero;

        float rayLenght = movement.magnitude + 2*m_skinWidth;
        RaycastHit2D hit = m_collisionFunction(displacement, movement.normalized, rayLenght);
        if (hit) {
            float separation = RealDistaceToWall(hit.normal, movement.normalized);
            Vector2 iterationDisplacement = movement.normalized * (hit.distance - separation);
            movement -= movement.normalized * (hit.distance - separation);
            
            movement = ProjectOnPlane(hit.normal, movement).normalized * movement.magnitude;
            return iterationDisplacement + CheckCollisions(movement, iterationDisplacement + displacement, iteration+1);
        } else {
            return movement;
        }
    }
    #endregion

    #region CollisionHelpers
    void LoadCollisionDetection() {
        if (m_collider is CircleCollider2D) m_collisionFunction = CircleCollision;
        else if (m_collider is BoxCollider2D) m_collisionFunction = BoxCollision;
        else if (m_collider is CapsuleCollider2D) m_collisionFunction = CapsuleCollision;
        else {
            Debug.LogError("El collider introducido no es compatible");
            Debug.Break();
        }
    }

    float RealDistaceToWall(Vector2 normal, Vector2 direction) {
        float angleWithWall = Vector2.Angle(-normal.normalized, direction.normalized);
        float converter = Mathf.Cos(angleWithWall * Mathf.Deg2Rad);
        if (Mathf.Abs(converter) < 0.1) converter = 0.1f * Mathf.Sign(converter);
        float distanceToWall = m_skinWidth / converter;
        return distanceToWall;
    }

    Vector2 ProjectOnPlane(Vector2 planeNormal, Vector2 vector) {
        if (Vector2.Dot(vector.normalized, -planeNormal) > m_alignmentToSlide) return Vector2.zero; // If it does not exceed the threshold to slide, then it doesn't move
        if (Vector2.Dot(m_origDirection, -planeNormal) < (1 - m_alignmentToSlide)) return Vector2.zero; // We need to take into account the original direction too
        float alignment = Vector2.Dot(planeNormal, vector);
        Vector2 projectedVector = vector - planeNormal * alignment;
        return projectedVector;
    }

    RaycastHit2D CircleCollision(Vector2 acumulatedPosition, Vector2 direction, float rayLenght) => Physics2D.CircleCast(m_collider.bounds.center + (Vector3)acumulatedPosition, ((CircleCollider2D)m_collider).radius, direction, rayLenght, m_collisionLayer);
    RaycastHit2D BoxCollision(Vector2 acumulatedPosition, Vector2 direction, float rayLenght) => Physics2D.BoxCast(m_collider.bounds.center + (Vector3)acumulatedPosition, ((BoxCollider2D)m_collider).size, transform.eulerAngles.z, direction, rayLenght, m_collisionLayer);
    RaycastHit2D CapsuleCollision(Vector2 acumulatedPosition, Vector2 direction, float rayLenght) => Physics2D.CapsuleCast(m_collider.bounds.center + (Vector3)acumulatedPosition, ((CapsuleCollider2D)m_collider).size, ((CapsuleCollider2D)m_collider).direction, transform.eulerAngles.z, direction, rayLenght, m_collisionLayer);
    #endregion
}

#region Editor
#if UNITY_EDITOR
[CustomEditor(typeof(General2DCollisionController))]
class General2DCollisionControllerEditor : Editor {
    // Debug properties
    SerializedProperty m_showRays, m_durationOfRays;
    // Collider properties
    SerializedProperty m_collider, m_skinWidth, m_collisionLayer, m_maxIterations, m_alignmentToSlide;
    // Gravity properties
    SerializedProperty takeIntoAccountGravity, bodyAxis;

    bool showDebug, showCollider, showGravity;

    private void OnEnable() {
        m_showRays = serializedObject.FindProperty("m_showRays");
        m_durationOfRays = serializedObject.FindProperty("m_durationOfRays");

        m_collider = serializedObject.FindProperty("m_collider");
        m_skinWidth = serializedObject.FindProperty("m_skinWidth");
        m_collisionLayer = serializedObject.FindProperty("m_collisionLayer");
        m_maxIterations = serializedObject.FindProperty("m_maxIterations");
        m_alignmentToSlide = serializedObject.FindProperty("m_alignmentToSlide");

        takeIntoAccountGravity = serializedObject.FindProperty("<TakeIntoAccountGravity>k__BackingField");
        bodyAxis = serializedObject.FindProperty("<BodyAxis>k__BackingField");

        showDebug = EditorPrefs.GetBool("Editor_showDebug", true);
        showGravity = EditorPrefs.GetBool("Editor_showGravity", true);
        showCollider = EditorPrefs.GetBool("Editor_showCollider", true);
    }

    public override void OnInspectorGUI() {
        General2DCollisionController script = (General2DCollisionController)target;
        serializedObject.Update();
        //EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

        // || Collisions Settings ||
        showCollider = EditorGUILayout.Foldout(showCollider, "Collider Settings", true, EditorStyles.foldoutHeader);
        EditorPrefs.SetBool("Editor_showCollider", showCollider);
        if (showCollider) {
            EditorGUILayout.PropertyField(m_collider, true);
        }

        // ERROR DETECTION
        bool isTheCorrectCollider = (m_collider.objectReferenceValue is BoxCollider2D) || (m_collider.objectReferenceValue is CircleCollider2D) || (m_collider.objectReferenceValue is CapsuleCollider2D);
        if (m_collider.objectReferenceValue == null || !isTheCorrectCollider) {
            if (m_collider.objectReferenceValue)
                EditorGUILayout.HelpBox("The collider should be CircleCollider2D, BoxCollider2D or CapsuleCollider2D", MessageType.Error);
            else
                EditorGUILayout.HelpBox("It seems to be no collider", MessageType.Error);
            
            serializedObject.ApplyModifiedProperties();
            return;
        } 

        if (showCollider) {
            EditorGUI.indentLevel++;
            EditorGUILayout.PropertyField(m_skinWidth, true);
            if (m_skinWidth.floatValue <= 0.02f) m_skinWidth.floatValue = 0.02f;
            
            EditorGUILayout.PropertyField(m_collisionLayer, true);
            
            EditorGUILayout.PropertyField(m_maxIterations, true);
            if (m_maxIterations.intValue < 2) m_maxIterations.intValue = 2; 

            EditorGUILayout.PropertyField(m_alignmentToSlide, true);

            if (m_collisionLayer.intValue == 0) EditorGUILayout.HelpBox("There is no Collision Layer selected", MessageType.Warning);
            
            EditorGUI.indentLevel--;
        }
        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
        // || Colisions Settings ||


        // || Gravity Settings ||
        showGravity = EditorGUILayout.Foldout(showGravity, "Gravity Settings", true, EditorStyles.foldoutHeader);
        EditorPrefs.SetBool("Editor_showGravity", showGravity);
        if (showGravity) {
            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("Take Into Account Gravity", GUILayout.Width(180));
            EditorGUILayout.PropertyField(takeIntoAccountGravity, GUIContent.none);
            EditorGUILayout.EndHorizontal();

            EditorGUI.indentLevel++;
            if (takeIntoAccountGravity.boolValue) {
                EditorGUILayout.PropertyField(bodyAxis, true);
                if (bodyAxis.objectReferenceValue == null) bodyAxis.objectReferenceValue = script.transform;
            
            }
            
            EditorGUI.indentLevel--;
        }
        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
        // || Gravity Settings ||
        

        // || Debug Settings ||
        showDebug = EditorGUILayout.Foldout(showDebug, "Debug Settings", true, EditorStyles.foldoutHeader);
        EditorPrefs.SetBool("Editor_showDebug", showDebug);
        if (showDebug) {
            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("Show Rays", GUILayout.Width(180));
            EditorGUILayout.PropertyField(m_showRays, GUIContent.none);
            EditorGUILayout.EndHorizontal();
            EditorGUI.indentLevel++;

            if (m_showRays.boolValue) {
                EditorGUILayout.PropertyField(m_durationOfRays, true);
                if (m_durationOfRays.floatValue < 0f) m_durationOfRays.floatValue = 0f;
            }
            EditorGUI.indentLevel--;
        }
        // || Debug Settings ||


        serializedObject.ApplyModifiedProperties();
    }
}
#endif
#endregion