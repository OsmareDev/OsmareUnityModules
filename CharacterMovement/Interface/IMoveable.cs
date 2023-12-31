using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMoveable
{
    public void Move(Vector3 v);

    public bool isGrounded {get;}
    public CollisionFlags collisionFlags {get;}
}