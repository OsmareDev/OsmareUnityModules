using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInputManager
{
    public Vector2 GetMoveDirection();
    public Vector2 GetLookDirection();
    public bool JumpedThisFrame();
    public bool ShootedThisFrame();
    public bool Action1ThisFrame();
    public bool Action2ThisFrame();
    public bool Action3ThisFrame();
    public bool Action4ThisFrame();
}
