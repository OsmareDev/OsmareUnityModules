using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GunController : MonoBehaviour
{
    [SerializeField] private APlayerInput m_playerInput;
    [SerializeField] private LayerMask m_enemyLayer;
    [SerializeField] private float m_angleVelocity = 10f;
    [SerializeField] private float m_maxDistance = 10f;
    public Vector2 DirectionPlayer {get; set;}
    public bool ShootType {get; set;} = true;

    private void Update() {
        if (m_playerInput.JumpedThisFrame()) ShootType = !ShootType;
        DirectionPlayer = m_playerInput.GetDirection();

        Vector2 directionToFollow = GetDirectionToLookAt();
        if (directionToFollow == Vector2.zero) directionToFollow = (transform.position - transform.parent.transform.position).normalized;

        float angle = (Vector2.SignedAngle(transform.localPosition.normalized, directionToFollow));
        angle = Mathf.MoveTowardsAngle(0, angle, m_angleVelocity * Time.deltaTime);
        transform.RotateAround(transform.parent.transform.position, transform.forward, angle);
    }

    private Collider2D GetClosestEnemy() {
        List<Collider2D> enemies = new List<Collider2D>(Physics2D.OverlapCircleAll(transform.parent.position, m_maxDistance, m_enemyLayer));
        Collider2D closest = enemies.OrderBy( enemy => Vector2.Distance(enemy.transform.position, transform.parent.transform.position)).FirstOrDefault();
        return closest;
    }

    private Vector2 GetDirectionToLookAt() {
        Collider2D closest = GetClosestEnemy();

        Vector2 directionToFollow;
        if (ShootType)
            if (closest != null) {
                directionToFollow = (closest.transform.position - transform.parent.transform.position).normalized;
            } else {
                directionToFollow = DirectionPlayer;
            }
        else directionToFollow = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.parent.position).normalized;

        return directionToFollow;
    } 

    private void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.parent.transform.position, m_maxDistance);
    }
}
