using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent (typeof(PlayerInput))]
public class InputManagerPlayer : MonoBehaviour, IInputManager
{
    [SerializeField] private InputActionReference _moveInput, _lookInput, _jumpInput, _shootAction;

    public Vector2 GetMoveDirection() => (_moveInput)? _moveInput.action.ReadValue<Vector2>().normalized : default(Vector2);
    public Vector2 GetLookDirection() => (_lookInput)? _lookInput.action.ReadValue<Vector2>().normalized : default(Vector2);
    public bool JumpedThisFrame() => (_jumpInput)? _jumpInput.action.WasPerformedThisFrame() : default(bool);
    public bool ShootedThisFrame() => (_shootAction)? _shootAction.action.WasPerformedThisFrame() : default(bool);
    public bool Action1ThisFrame() => false;
    public bool Action2ThisFrame() => false;
    public bool Action3ThisFrame() => false;
    public bool Action4ThisFrame() => false;
}
