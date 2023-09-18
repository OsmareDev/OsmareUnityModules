using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletExample2 : BaseBullet
{
    public override float TimeToLive {get; set;} = 5;
    public override int TimesAlive {get; set;} = 0;
    public override int ID { get; set; } = 2;

    private Vector3 m_dir;

    public int id = 2;

    public override void Shooted(Vector3 pos, Vector3 dir)
    {
        transform.position = pos;
        m_dir = dir;
    }

    public void Update() {
        transform.position += m_dir * Time.deltaTime;
    }
}
