using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AInputManager : MonoBehaviour
{
    public abstract Vector2 GetMoveDirection();
    public abstract Vector2 GetLookDirection();
    public abstract bool JumpedThisFrame();
}
