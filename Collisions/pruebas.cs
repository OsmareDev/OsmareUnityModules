using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class pruebas : MonoBehaviour
{
    [SerializeField] private float speed = 10f;
    [SerializeField] private LayerMask collisionLayers;
    private BoxCollider2D playerCollider;
    private RaycastHit2D hit;
    private Vector2 movement;

    void Start()
    {
        playerCollider = GetComponent<BoxCollider2D>();
    }


    void Update()
    {
        float horizontalMovement = Input.GetAxisRaw("Horizontal");
        float verticalMovement = Input.GetAxisRaw("Vertical");

        movement = new Vector2(horizontalMovement, verticalMovement).normalized;
        Debug.Log(movement);
        Vector2 semen = movement * speed * Time.deltaTime;

        

        hit = Physics2D.BoxCast(transform.position, playerCollider.size, 0, new Vector2(0, movement.y).normalized, semen.y, collisionLayers);
        if ( !hit ) transform.Translate(0, semen.y, 0);
        hit = Physics2D.BoxCast(transform.position, playerCollider.size, 0, new Vector2(movement.x, 0).normalized, semen.x, collisionLayers);
        if ( !hit ) transform.Translate(semen.x, 0, 0);
    }

    Vector2 ProjectOnPlane(Vector2 planeNormal, Vector2 vector) {
        float alignment = Vector2.Dot(planeNormal, vector);
        Vector2 projectedVector = vector - (planeNormal * alignment);
        return projectedVector;
    }
}
