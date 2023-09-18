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
        if (Physics2D.OverlapCircle(m_collider.bounds.center, m_collider.radius, m_enemyLayer)) {
            m_deactivationFunction(this);
        }

        Vector3 move = m_dir * m_speed * Time.deltaTime;
        RaycastHit2D hit = Physics2D.CircleCast(m_collider.bounds.center, m_collider.radius, m_dir, move.magnitude + 0.01f, m_collideLayer);
        if (hit) {
            move = m_dir * (hit.distance - 0.01f);

            m_dir = hit.normal;
            Collider2D enemy = Physics2D.OverlapCircle(m_collider.bounds.center + move, m_detectionRadius, m_enemyLayer);
            if (enemy) m_dir = (enemy.transform.position - (m_collider.bounds.center + move)).normalized;
        }

        transform.position += move;
    }
}
