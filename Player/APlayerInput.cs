using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class APlayerInput : MonoBehaviour
{
    public abstract Vector2 GetDirection();
    public abstract bool JumpedThisFrame();
}
