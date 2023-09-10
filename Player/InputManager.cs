using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent (typeof(PlayerInput))]
public class InputManager : AInputManager
{
    [SerializeField] private InputActionReference _moveInput, _lookInput, _jumpInput;

    public override Vector2 GetMoveDirection() => (_moveInput)? _moveInput.action.ReadValue<Vector2>().normalized : default(Vector2);
    public override Vector2 GetLookDirection() => (_lookInput)? _lookInput.action.ReadValue<Vector2>().normalized : default(Vector2);
    public override bool JumpedThisFrame() => (_jumpInput)? _jumpInput.action.WasPerformedThisFrame() : default(bool);
}
