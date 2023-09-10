using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionControllerCircle : ACollisionController
{
    [Header("Debug")]
    [SerializeField] private bool m_showRays = false;
    [SerializeField] private float m_durationOfRays = 2f;
    
    [Header("Collision")]
    [SerializeField] private CircleCollider2D m_collider;
    [SerializeField] private LayerMask m_collisionLayer;
    [SerializeField] private float m_skinWidth = 0.001f;
    [SerializeField] private int n_maxIterations = 2;

    private Bounds m_bounds;

    public override void Move(Vector2 v) {
        RecalculateBounds();
        Vector2 finalMovement = CheckCollisions(v, Vector2.zero, 1);
        if (m_showRays) Debug.DrawLine(transform.position, transform.position + (Vector3)v, Color.magenta, m_durationOfRays);
        transform.position += (Vector3)finalMovement;
    }

    Vector2 CheckCollisions(Vector2 movement, Vector2 position, int iteration) {
        if (iteration > n_maxIterations) return Vector2.zero;

        float rayLenght = movement.magnitude + 2*m_skinWidth;
        RaycastHit2D hit = Physics2D.CircleCast(m_bounds.center, m_bounds.extents.x, movement.normalized, rayLenght, m_collisionLayer);

        if (hit) {
            float separation = realDistaceToWall(hit.normal, movement.normalized);
            Vector2 acumulatedPosition = movement.normalized * (hit.distance - separation);
            movement -= acumulatedPosition;

            movement = ProjectOnPlane(hit.normal, movement).normalized * movement.magnitude;
            return position + CheckCollisions(movement, position + acumulatedPosition, iteration+1);
        }

        return movement;
    }

    Vector2 ProjectOnPlane(Vector2 planeNormal, Vector2 vector) {
        float alignment = Vector2.Dot(planeNormal, vector);
        Vector2 projectedVector = vector - (planeNormal * alignment);
        return projectedVector;
    }

    float realDistaceToWall(Vector2 normal, Vector2 direction) {
        float angleWithWall = Vector2.Angle(-normal, direction);
        float converter = Mathf.Cos(angleWithWall * Mathf.Deg2Rad);
        if (Mathf.Abs(converter) < 0.1) converter = 0.1f * Mathf.Sign(converter);
        float distanceToWall = m_skinWidth / converter;
        return distanceToWall;
    }

    void RecalculateBounds() {
        m_bounds = m_collider.bounds;
        m_bounds.Expand(-2 * m_skinWidth);
    }
}
