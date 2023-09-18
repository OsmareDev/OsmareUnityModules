using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletExample1 : BaseBullet
{
    public override float TimeToLive {get => 5; set {}}
    public override int TimesAlive {get; set;} = 0;
    public override int ID { get; set; } = 1;

    private Vector3 m_dir;

    public int id = 1;

    public override void Shooted(Vector3 pos, Vector3 dir)
    {
        transform.position = pos;
        m_dir = dir;
    }

    public void Update() {
        transform.position += m_dir * Time.deltaTime;
    }
}
