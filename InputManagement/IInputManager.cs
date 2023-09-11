using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInputManager
{
    public Vector2 GetMoveDirection();
    public Vector2 GetLookDirection();
    public bool JumpedThisFrame();
}
