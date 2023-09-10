using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent (typeof(PlayerInput))]
public class PlayerInputScriptManager : APlayerInput
{
    [SerializeField] private InputActionReference _moveInput, _jumpInput;

    public override Vector2 GetDirection() => _moveInput.action.ReadValue<Vector2>().normalized;
    public override bool JumpedThisFrame() => _jumpInput.action.WasPerformedThisFrame();
}
