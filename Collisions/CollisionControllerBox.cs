using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionControllerBox : ACollisionController
{
    [Header("Debug")]
    [SerializeField] private bool m_showRays = false;
    [SerializeField] private float m_durationOfRays = 2f;
    
    [Header("Collision")]
    [SerializeField] private BoxCollider2D m_collider;
    [SerializeField] private LayerMask m_collisionLayer;
    [SerializeField] private float m_skinWidth = 0.001f;

    void Awake() {
        //Time.timeScale = 0.01f;
    }

    public override void Move(Vector2 v) {
        CheckCollisions(ref v);
        if (m_showRays) Debug.DrawLine(transform.position, transform.position + (Vector3)v, Color.magenta, m_durationOfRays);
        transform.position += new Vector3(v.x, v.y, 0);
    }

    void CheckPosition(ref Vector2 v) {
        if (Physics2D.OverlapBox(m_collider.bounds.center + (Vector3)v, m_collider.size, 0f, m_collisionLayer)) {
            v = Vector2.zero;
        }
    }

    void CheckCollisions(ref Vector2 v) {
        Vector2 newVelocityToCheck = v;
        Vector2 finalVelocity = Vector2.zero;

        float rayLenght;
        int cont = 0;

        Debug.Log("begin");

        while (newVelocityToCheck != Vector2.zero) {
            Debug.Log("vuelta : " + cont);
            rayLenght = newVelocityToCheck.magnitude + 2*m_skinWidth;
            Debug.Log("rayLenght : " + rayLenght);
            if (finalVelocity.magnitude > 100f) Debug.Break();

            RaycastHit2D hit = Physics2D.BoxCast(m_collider.bounds.center + (Vector3)finalVelocity, m_collider.bounds.size, 0f, newVelocityToCheck.normalized, rayLenght, m_collisionLayer);
            if (hit) {
                if (m_showRays) Debug.DrawRay(hit.point, hit.normal, Color.yellow, m_durationOfRays);
                if (m_showRays) Debug.DrawRay(hit.point, newVelocityToCheck.normalized, Color.black, m_durationOfRays);
                Debug.Log("hit.point : " + hit.point);
                Debug.Log("hit.normal.normalized : " + hit.normal);
                Debug.Log("newVelocityToCheck.normalized : " + newVelocityToCheck.normalized);

                float angleWithWall = Vector2.Angle(-hit.normal.normalized, newVelocityToCheck.normalized);
                Debug.Log("angle : " + angleWithWall);
                float com = Mathf.Cos(angleWithWall * Mathf.Deg2Rad);
                if (Mathf.Abs(com) < 0.1) com = 0.1f;
                float distanceToBe = m_skinWidth / com;
                Debug.Log("distanceToBe : " + distanceToBe);

                finalVelocity += newVelocityToCheck.normalized * (hit.distance - distanceToBe);
                Debug.Log("finalVelocity 1 : " + finalVelocity);
                Debug.Log("finalVelocity 1 mag: " + finalVelocity.magnitude);
                newVelocityToCheck -= newVelocityToCheck.normalized * (hit.distance);
                Debug.Log("newVelocityToCheck 1 : " + newVelocityToCheck);
                Debug.Log("newVelocityToCheck 1 mag: " + newVelocityToCheck.magnitude);

                Vector2 wallNormal = hit.normal;
                Debug.Log("wallNormal : " + wallNormal);
                float alignment = Vector2.Dot(wallNormal, v.normalized);
                Debug.Log("alignment : " + alignment);

                // if (alignment >= -0.9) {
                    float correction = Vector2.Dot(wallNormal, v);
                    Debug.Log("correction : " + correction);
                    newVelocityToCheck = (v - wallNormal * correction).normalized * v.magnitude;
                    Debug.Log("newVelocityToCheck 2 : " + newVelocityToCheck);
                    Debug.Log("newVelocityToCheck 2 mag: " + newVelocityToCheck.magnitude);
                // } else {
                //     newVelocityToCheck = Vector2.zero;
                //     Debug.Log("newVelocityToCheck 2 : " + newVelocityToCheck);
                //     Debug.DrawRay(hit.point + hit.normal * m_skinWidth, newVelocityToCheck.normalized, Color.red, 1f);
                // }


            } else {
                finalVelocity += newVelocityToCheck;
                Debug.Log("finalVelocity else : " + finalVelocity);
                newVelocityToCheck = Vector2.zero;
                Debug.Log("newVelocityToCheck else : " + finalVelocity);
            }

            if (cont > 10) {
                break;
            }
            cont++;
        }

        v = finalVelocity;
    }
}
