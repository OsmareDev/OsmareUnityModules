using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletExample1 : BaseBullet
{
    [field: SerializeField] public override int TimesAlive {get; set;}
    [SerializeField] CircleCollider2D m_collider;
    [SerializeField] LayerMask m_collideLayer;
    [SerializeField] LayerMask m_enemyLayer;
    [SerializeField] float m_detectionRadius = 10f;

    new private void Update() {
        
        if (Physics2D.OverlapCircle(m_collider.bounds.center, m_collider.bounds.extents.x, m_enemyLayer)) {
            m_deactivationFunction(this);
        }

        Vector3 move = m_dir * m_speed * Time.deltaTime;
        RaycastHit2D hit = Physics2D.CircleCast(m_collider.bounds.center, m_collider.bounds.extents.x, m_dir, move.magnitude + 0.01f, m_collideLayer);
        if (hit) {
            move = m_dir * (hit.distance - 0.01f);
            m_dir = m_dir - 2 * (Vector3.Dot(m_dir, hit.normal) * (Vector3)hit.normal);


            Collider2D enemy = Physics2D.OverlapCircle(m_collider.bounds.center + move, m_detectionRadius, m_enemyLayer);
            if (enemy) {
                Vector2 posibleNewDirection = (enemy.transform.position - (m_collider.bounds.center + move));
                if (!Physics2D.CircleCast(m_collider.bounds.center + move, m_collider.bounds.extents.x, posibleNewDirection.normalized, posibleNewDirection.magnitude, m_collideLayer))
                    m_dir = posibleNewDirection.normalized;
            }
        }

        transform.position += move;
    }
}
