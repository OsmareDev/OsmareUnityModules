using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class CharacterMovement : MonoBehaviour
{
    public enum Controller_Type {
        _2D,
        _3D
    }

    [Header("COMPONENTS")]
    [SerializeField] private Object m_cc;
    [SerializeField] private InputActionReference m_moveInput, m_jumpInput;
    [SerializeField] private Transform m_referenceForControls;
    [SerializeField] private Controller_Type m_type;
    [SerializeField] private bool m_fixedUpdate;

    [Header("HORIZONTAL MOVEMENT")]
    [SerializeField] private float m_acceleration = 30f;
    [SerializeField] private float m_extraSpeedAtTopJump = 3f;
    [SerializeField] private float m_braking = 150f;
    [SerializeField] private float m_maxSpeed = 10f;

    [Header("GRAVITY")]
    [SerializeField] private float m_iniFallingAcceleration = 30;
    [SerializeField] private float m_endFallingAcceleration = 70;
    [SerializeField] private float m_maxFallingSpeed = 50;
    [SerializeField] private float m_brakeAscendingSpeed = 1000;

    [Header("JUMP")]
    [SerializeField] private float m_jumpSpeed = 17;
    [SerializeField] private float m_speedToConsiderApex = 12;
    [SerializeField] private float m_coyoteTime = 0.2f;
    [SerializeField] private float m_jumpBuffer = 0.2f;
    [SerializeField] private float m_airborneBrake = 15f;

    private bool m_coyoteReady = false;
    private float m_lastTimeGrounded, m_lastTimeJumped;
    private bool CanUseCoyote => m_coyoteReady && !EditorHelpers.CallProperty<bool>(m_cc, "isGrounded") && m_lastTimeGrounded + m_coyoteTime > Time.time;
    private bool HasBufferedJump => EditorHelpers.CallProperty<bool>(m_cc, "isGrounded") && m_lastTimeJumped + m_jumpBuffer > Time.time;

    private Vector3 m_moveDirection;
    private bool m_jumpedThisFrame, m_jumpValue;

    private Vector3 m_currentSpeed, m_currentHorizontal;
    private bool m_endedJumpEarly = false;
    private float m_completedJump;

    public void FixedUpdate() {
        if (m_fixedUpdate) {
            HorizontalSpeed();
            VerticalSpeed();
            Move();
        }
    }

    public void Update() {
        PlayerInput();
        ResetExtras();
        
        if (!m_fixedUpdate) {
            HorizontalSpeed();
            VerticalSpeed();
            Move();
        }
    }

    public void giveDirection(Vector3 direction) => m_moveDirection = direction;
    public void jump(bool jumpValue) {
        m_jumpedThisFrame = (jumpValue && !m_jumpValue);
        m_jumpValue = jumpValue;
    } 

    private void PlayerInput() {
        if (m_moveInput != null) {
            m_moveDirection = new Vector3(m_moveInput.action.ReadValue<Vector2>().x, 0, m_moveInput.action.ReadValue<Vector2>().y * (int)m_type);
            m_moveDirection = m_moveDirection.normalized;
        }

        if (m_jumpInput != null) {
            m_jumpedThisFrame = m_jumpInput.action.WasPerformedThisFrame();
            m_jumpValue = (m_jumpInput.action.ReadValue<float>() < 0.5f);
        }

        if (!EditorHelpers.CallProperty<bool>(m_cc, "isGrounded") && m_jumpValue && !m_endedJumpEarly && m_currentSpeed.y > 0) {
            m_endedJumpEarly = true;
        }
    }

    private void ResetExtras() {
        if (EditorHelpers.CallProperty<bool>(m_cc, "isGrounded")) {
            m_lastTimeGrounded = Time.time;
            m_coyoteReady = true;
        }

        if (m_jumpedThisFrame) m_lastTimeJumped = Time.time;
    }

    private void HorizontalSpeed() {
        if (m_moveDirection != Vector3.zero) {
            // If they have opposite signs, brake is used instead of acceleration
            if (Vector3.Dot(m_moveDirection, new Vector3(m_currentSpeed.x, 0, m_currentSpeed.z)) < 0) m_currentHorizontal += m_moveDirection * m_braking * ((m_fixedUpdate) ? Time.fixedDeltaTime : Time.deltaTime);
            else m_currentHorizontal += m_moveDirection * m_acceleration * ((m_fixedUpdate) ? Time.fixedDeltaTime : Time.deltaTime);

            m_currentHorizontal = Vector3.ClampMagnitude(m_currentHorizontal, m_maxSpeed);

            Vector3 apexBonus = m_moveDirection * m_extraSpeedAtTopJump * m_completedJump;
            m_currentHorizontal += apexBonus;
        }
        else {
            if (EditorHelpers.CallProperty<bool>(m_cc, "isGrounded")) m_currentHorizontal = Vector3.MoveTowards(m_currentHorizontal, Vector3.zero, m_braking * ((m_fixedUpdate) ? Time.fixedDeltaTime : Time.deltaTime));
            else m_currentHorizontal = Vector3.MoveTowards(m_currentHorizontal, Vector3.zero, m_airborneBrake * ((m_fixedUpdate) ? Time.fixedDeltaTime : Time.deltaTime));
        }

        if ((EditorHelpers.CallProperty<CollisionFlags>(m_cc, "collisionFlags") & CollisionFlags.Sides) != 0) m_currentHorizontal = Vector3.zero;

        m_currentSpeed.x = m_currentHorizontal.x;
        m_currentSpeed.z = m_currentHorizontal.z;
    }

    private void VerticalSpeed() {
        float fallSpeed;

        if (EditorHelpers.CallProperty<bool>(m_cc, "isGrounded")) {
            m_completedJump = m_currentSpeed.y = 0;
        }

        //else {
            m_completedJump = Mathf.InverseLerp(m_speedToConsiderApex, 0, Mathf.Abs(m_currentSpeed.y));
            fallSpeed = Mathf.Lerp(m_iniFallingAcceleration, m_endFallingAcceleration, m_completedJump);

            // GRAVITY
            if (m_endedJumpEarly && m_currentSpeed.y > 0)  fallSpeed = m_brakeAscendingSpeed;

            m_currentSpeed.y -= fallSpeed * ((m_fixedUpdate) ? Time.fixedDeltaTime : Time.deltaTime);

            if (m_currentSpeed.y < -Mathf.Abs(m_maxFallingSpeed)) m_currentSpeed.y = -Mathf.Abs(m_maxFallingSpeed);
        //}

        if ((m_jumpedThisFrame && CanUseCoyote) || HasBufferedJump) {
            m_currentSpeed.y = m_jumpSpeed;
            m_endedJumpEarly = false;
            m_coyoteReady = false;
            m_lastTimeGrounded = 0;
        }

        // If it hits the roof it loses all speed
        if ((EditorHelpers.CallProperty<CollisionFlags>(m_cc, "collisionFlags") & CollisionFlags.Above) != 0) {
            if (m_currentSpeed.y > 0) m_currentSpeed.y = 0;
        }
    }

    private void Move() {
        Vector3 cameraRight = m_referenceForControls.right;
        Vector3 cameraForward = m_referenceForControls.forward;
        Vector3 cameraUp = m_referenceForControls.up;

        //m_cc.RepositionToColider();

        /*
        m_cc.Move(cameraRight * m_currentSpeed.x * Time.fixedDeltaTime);
        m_cc.Move(cameraForward * m_currentSpeed.z * Time.fixedDeltaTime);
        m_cc.Move(cameraUp * m_currentSpeed.y * Time.fixedDeltaTime);
        */

        Vector3 finalMove = cameraRight * m_currentSpeed.x * ((m_fixedUpdate) ? Time.fixedDeltaTime : Time.deltaTime);
        finalMove += cameraForward * m_currentSpeed.z * ((m_fixedUpdate) ? Time.fixedDeltaTime : Time.deltaTime);
        finalMove += cameraUp * m_currentSpeed.y * ((m_fixedUpdate) ? Time.fixedDeltaTime : Time.deltaTime);

        EditorHelpers.CallMethod(m_cc, "Move", finalMove);

        // Debug.Log(m_cc.isGrounded);
        // Debug.Log(m_cc.collisionFlags);
    }
}

#region Editor
#if UNITY_EDITOR
[CustomEditor(typeof(CharacterMovement))]
class CharacterMovementEditor : Editor {

    // [SerializeField] private CapsuleCollisionController m_cc;
    
    // Components properties
    SerializedProperty m_fixedUpdate, m_type, m_referenceForControls, m_jumpInput, m_moveInput, m_cc;
    // Horizontal movement properties
    SerializedProperty m_maxSpeed, m_braking, m_extraSpeedAtTopJump, m_acceleration;
    // Gravity properties
    SerializedProperty m_brakeAscendingSpeed, m_maxFallingSpeed, m_endFallingAcceleration, m_iniFallingAcceleration;
    // Jump properties
    SerializedProperty m_airborneBrake, m_jumpBuffer, m_coyoteTime, m_speedToConsiderApex, m_jumpSpeed;

    bool showComponents, showHorizontal, showGravity, showJump;

    private void OnEnable() {
        m_fixedUpdate = serializedObject.FindProperty("m_fixedUpdate");
        m_type = serializedObject.FindProperty("m_type");
        m_referenceForControls = serializedObject.FindProperty("m_referenceForControls");
        m_jumpInput = serializedObject.FindProperty("m_jumpInput");
        m_moveInput = serializedObject.FindProperty("m_moveInput");
        m_cc = serializedObject.FindProperty("m_cc");

        m_maxSpeed = serializedObject.FindProperty("m_maxSpeed");
        m_braking = serializedObject.FindProperty("m_braking");
        m_extraSpeedAtTopJump = serializedObject.FindProperty("m_extraSpeedAtTopJump");
        m_acceleration = serializedObject.FindProperty("m_acceleration");

        m_brakeAscendingSpeed = serializedObject.FindProperty("m_brakeAscendingSpeed");
        m_maxFallingSpeed = serializedObject.FindProperty("m_maxFallingSpeed");
        m_endFallingAcceleration = serializedObject.FindProperty("m_endFallingAcceleration");
        m_iniFallingAcceleration = serializedObject.FindProperty("m_iniFallingAcceleration");

        m_airborneBrake = serializedObject.FindProperty("m_airborneBrake");
        m_jumpBuffer = serializedObject.FindProperty("m_jumpBuffer");
        m_coyoteTime = serializedObject.FindProperty("m_coyoteTime");
        m_speedToConsiderApex = serializedObject.FindProperty("m_speedToConsiderApex");
        m_jumpSpeed = serializedObject.FindProperty("m_jumpSpeed");

        showComponents = EditorPrefs.GetBool("Editor_Osmare_CharacterMovement_showComponents", true);
        showHorizontal = EditorPrefs.GetBool("Editor_Osmare_CharacterMovement_showHorizontal", true);
        showGravity = EditorPrefs.GetBool("Editor_Osmare_CharacterMovement_showGravity", true);
        showJump = EditorPrefs.GetBool("Editor_Osmare_CharacterMovement_showJump", true);
    }

    public override void OnInspectorGUI() {
        CharacterMovement script = (CharacterMovement)target;
        serializedObject.Update();
        //EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

        // || Components Settings ||
        showComponents = EditorGUILayout.Foldout(showComponents, "Components Settings", true, EditorStyles.foldoutHeader);
        EditorPrefs.SetBool("Editor_Osmare_CharacterMovement_showComponents", showComponents);
        if (showComponents) {
            EditorHelpers.CollectAnyThingWithTheFunction(ref m_cc, "Move", "Script with Move function");
        }
        
        if (m_cc.objectReferenceValue == null) {
            EditorGUILayout.HelpBox("There is no collision controller (script with move function)", MessageType.Error);
            serializedObject.ApplyModifiedProperties();
            return;
        }

        if (showComponents) {
            EditorGUILayout.PropertyField(m_referenceForControls, new GUIContent("Reference For Controls"), true);
            EditorGUILayout.PropertyField(m_type, new GUIContent("Dimensions"), true);
            EditorGUILayout.PropertyField(m_fixedUpdate, new GUIContent("Use Fixed Update"), true);
            EditorGUILayout.PropertyField(m_jumpInput, new GUIContent("Jump Input"), true);
            EditorGUILayout.PropertyField(m_moveInput, new GUIContent("Move Input"), true);
        }

        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
        // || Components Settings ||


        // || Horizontal Movement Settings ||
        showHorizontal = EditorGUILayout.Foldout(showHorizontal, "Horizontal Movement Settings", true, EditorStyles.foldoutHeader);
        EditorPrefs.SetBool("Editor_Osmare_CharacterMovement_showHorizontal", showHorizontal);
        if (showHorizontal) {
            EditorGUILayout.PropertyField(m_acceleration, new GUIContent("Horizontal Acceleration"), true);
            EditorGUILayout.PropertyField(m_maxSpeed, new GUIContent("Maximum Speed"), true);
            EditorGUILayout.PropertyField(m_braking, new GUIContent("Ground Braking Acceleration"), true);
            EditorGUILayout.PropertyField(m_airborneBrake, new GUIContent("Airborne Brake Acceleration"), true);
            EditorGUILayout.PropertyField(m_extraSpeedAtTopJump, new GUIContent("Extra Speed At Jump Apex"), true);
        }
        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
        // || Horizontal Movement Settings ||


        // || Gravity Settings ||
        showGravity = EditorGUILayout.Foldout(showGravity, "Gravity Settings", true, EditorStyles.foldoutHeader);
        EditorPrefs.SetBool("Editor_Osmare_CharacterMovement_showGravity", showGravity);
        if (showGravity) {
            EditorGUILayout.PropertyField(m_iniFallingAcceleration, new GUIContent("Initial Falling Acceleration"), true);
            EditorGUILayout.PropertyField(m_endFallingAcceleration, new GUIContent("End Falling Acceleration"), true);
            EditorGUILayout.PropertyField(m_maxFallingSpeed, new GUIContent("Maximun Falling Speed"), true);
            EditorGUILayout.PropertyField(m_brakeAscendingSpeed, new GUIContent("Brake Ascending Speed"), true);
        }
        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
        // || Gravity Settings ||


        // || Jump Settings ||
        showJump = EditorGUILayout.Foldout(showJump, "Jump Settings", true, EditorStyles.foldoutHeader);
        EditorPrefs.SetBool("Editor_Osmare_CharacterMovement_showJump", showJump);
        if (showJump) {
            EditorGUILayout.PropertyField(m_jumpSpeed, new GUIContent("Jump Speed"), true);
            EditorGUILayout.PropertyField(m_jumpBuffer, new GUIContent("Time buffer for jump"), true);
            EditorGUILayout.PropertyField(m_coyoteTime, new GUIContent("Coyote Time"), true);
            EditorGUILayout.PropertyField(m_speedToConsiderApex, new GUIContent("Speed to consider the apex jump"), true);
        }
        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
        // || Jump Settings ||

        serializedObject.ApplyModifiedProperties();
    }
}
#endif
#endregion