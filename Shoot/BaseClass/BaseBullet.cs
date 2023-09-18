using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class BaseBullet : MonoBehaviour, ILiveTime
{
    public abstract int TimesAlive {get; set;}
    public Action<BaseBullet> m_deactivationFunction;

    protected Vector3 m_dir;
    protected float m_speed;

    public virtual void Shooted(Vector3 pos, Vector3 dir, WeaponStats stats)
    {
        transform.position = pos;
        m_dir = dir;
        m_speed = stats.bulletSpeed;
    }

    protected virtual void Update() => transform.position += m_dir * m_speed * Time.deltaTime;
}
