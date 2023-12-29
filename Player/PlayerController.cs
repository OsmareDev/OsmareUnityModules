using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class PlayerController : MonoBehaviour
{

    [SerializeField] private Object m_collisionController; //IMoveable
    [SerializeField] private Object m_playerInput; //IInputManager
    [SerializeField] private AimController2D m_aim;
    [SerializeField] private ShootController m_shoot;
    [SerializeField] private float m_velocity = 5f;

    [SerializeField] private float m_cameraVelocity = 20f;
    [field: SerializeField] public bool CameraControl {get; set;} = false;


    private Vector2 m_movementDirection;

    void Update()
    {
        GatherInput();
        CheckCameraControl();
        //TODO : hacer para llamar a la funcion pedida en el editor en vez de la interfaz
        ((IMoveable)m_collisionController).Move(m_movementDirection * (m_velocity * Time.deltaTime));

        m_aim.SetAimDirection(((IInputManager)m_playerInput).GetLookDirection(), m_movementDirection);
        if (((IInputManager)m_playerInput).ShootedThisFrame()) m_shoot.Shoot();
    }

    private void GatherInput() {
        m_movementDirection = ((IInputManager)m_playerInput).GetMoveDirection();
    }

    private void CheckCameraControl() {
        if (CameraControl) {
            float originalDistance = Camera.main.transform.position.z;
            Vector2 newPos = Vector2.MoveTowards((Vector2)Camera.main.transform.position, (Vector2)transform.position, m_cameraVelocity * Time.deltaTime);
            Camera.main.transform.position = new Vector3(newPos.x, newPos.y, originalDistance);
        }
    }
}

#region Editor
#if UNITY_EDITOR
[CustomEditor(typeof(PlayerController))]
class PlayerControllerEditor : Editor {
    SerializedProperty m_collisionController, m_playerInput, m_velocity, m_cameraVelocity, cameraControl;
    SerializedProperty m_aim, m_shoot;
    

    private void OnEnable() {
        m_collisionController = serializedObject.FindProperty("m_collisionController");
        m_playerInput = serializedObject.FindProperty("m_playerInput");
        m_velocity = serializedObject.FindProperty("m_velocity");
        m_cameraVelocity = serializedObject.FindProperty("m_collisionController");
        cameraControl = serializedObject.FindProperty("<CameraControl>k__BackingField");
        m_aim = serializedObject.FindProperty("m_aim");
        m_shoot = serializedObject.FindProperty("m_shoot");
    }

    public override void OnInspectorGUI() {
        PlayerController script = (PlayerController)target;
        serializedObject.Update();

        EditorHelpers.CollectAnyThingWithTheFunction(ref m_collisionController, "Move", "Collision Controller ");

        if (m_collisionController.objectReferenceValue == null) {
            EditorGUILayout.HelpBox("This script needs a collision handling class that implements from IMoveable", MessageType.Error);
            serializedObject.ApplyModifiedProperties();
            return;
        }

        EditorHelpers.CollectInterface<IInputManager>(ref m_playerInput, "Input Manager ");

        if (m_playerInput.objectReferenceValue == null) EditorGUILayout.HelpBox("There is no input(IInputManager) attached", MessageType.Warning);
        EditorGUILayout.PropertyField(m_velocity, true);
        EditorGUILayout.PropertyField(cameraControl, true);

        // Arreglar
        EditorGUILayout.PropertyField(m_aim, true);
        EditorGUILayout.PropertyField(m_shoot, true);

        serializedObject.ApplyModifiedProperties();
    }
}
#endif
#endregion