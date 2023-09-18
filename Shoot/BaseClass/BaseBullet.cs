using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseBullet : MonoBehaviour, ILiveTime
{
    public abstract int ID {get; set;}
    public abstract float TimeToLive {get; set;}
    public abstract int TimesAlive {get; set;}

    public abstract void Shooted(Vector3 pos, Vector3 dir);
}
