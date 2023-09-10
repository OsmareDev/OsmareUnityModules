using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private APlayerInput m_playerInput;
    [SerializeField, SerializeReference] private General2DCollisionController m_cc;
    [SerializeField] private float m_velocity = 5f;

    [SerializeField] private float m_cameraVelocity = 20f;
    [field: SerializeField] public bool CameraControl {get; set;} = false;


    private Vector2 m_movementDirection;

    void Update()
    {
        GatherInput();
        CheckCameraControl();
        m_cc.Move(m_movementDirection * (m_velocity * Time.deltaTime));
    }

    private void GatherInput() {
        m_movementDirection = m_playerInput.GetDirection();
    }

    private void CheckCameraControl() {
        if (CameraControl) {
            float originalDistance = Camera.main.transform.position.z;
            Vector2 newPos = Vector2.MoveTowards((Vector2)Camera.main.transform.position, (Vector2)transform.position, m_cameraVelocity * Time.deltaTime);
            Camera.main.transform.position = new Vector3(newPos.x, newPos.y, originalDistance);
        }
    }
}
