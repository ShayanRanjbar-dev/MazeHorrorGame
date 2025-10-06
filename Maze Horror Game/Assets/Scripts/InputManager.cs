using System;
using UnityEngine;
using UnityEngine.InputSystem;
public class InputManager : MonoBehaviour
{
    private PlayerInputAction inputActions;
    public Vector2 Move { get; private set; }
    public Vector2 Camera { get; private set; }
    public bool IsSprinting { get; private set; }
    public bool LookingBack { get; private set; }

    public event Action OnPlayerJumpPressed;

    private void Update()
    {
        if (LookingBack)
        {
            return;
        }
            HandleTouch();
    }
    private void HandleTouch()
    {
        Camera = Vector2.zero;
        if (Touchscreen.current != null)
        {
            foreach (var touchRaw in Touchscreen.current.touches)
            {
                if (touchRaw.press.isPressed)
                {
                    if (touchRaw.startPosition.ReadValue().x > Screen.width / 2)
                    {
                        Camera = touchRaw.delta.ReadValue();
                    }
                }
            }
        }
    }
    private void SubscribeInputActions()
    {
        inputActions.Player.Move.performed += OnMovePerformed;
        inputActions.Player.Move.canceled += OnMoveCanceled;
        inputActions.Player.Camera.performed += OnCameraPerformed;
        inputActions.Player.Camera.canceled += OnCameraCanceled;
        inputActions.Player.Sprint.performed += OnSprintPerformed;
        inputActions.Player.Sprint.canceled += OnSprintCanceled;
        inputActions.Player.Jump.performed += OnJumpPerformed;
        inputActions.Player.LookBack.performed += OnLookBackPerformed;
        inputActions.Player.LookBack.canceled += OnlookBackCanceled;
    }

    private void OnlookBackCanceled(InputAction.CallbackContext obj)
    {
        LookingBack = false;
    }

    private void OnLookBackPerformed(InputAction.CallbackContext obj)
    {
        LookingBack = true;
    }

    private void UnSubscribeInputActions()
    {
        inputActions.Player.Move.performed -= OnMovePerformed;
        inputActions.Player.Move.canceled -= OnMoveCanceled;
        inputActions.Player.Camera.performed -= OnCameraPerformed;
        inputActions.Player.Camera.canceled -= OnCameraCanceled;
        inputActions.Player.Sprint.performed -= OnSprintPerformed;
        inputActions.Player.Sprint.canceled -= OnSprintCanceled;
        inputActions.Player.Jump.performed -= OnJumpPerformed;
        inputActions.Player.LookBack.performed += OnLookBackPerformed;
        inputActions.Player.LookBack.canceled += OnlookBackCanceled;
    }
    private void Awake() => inputActions = new PlayerInputAction();
    private void OnJumpPerformed(InputAction.CallbackContext obj) => OnPlayerJumpPressed?.Invoke();
    private void OnSprintCanceled(InputAction.CallbackContext obj) => IsSprinting = false;
    private void OnSprintPerformed(InputAction.CallbackContext obj) => IsSprinting = true;
    private void OnCameraCanceled(InputAction.CallbackContext obj) => Camera = Vector2.zero;
    private void OnCameraPerformed(InputAction.CallbackContext obj) => Camera = obj.ReadValue<Vector2>();
    private void OnMoveCanceled(InputAction.CallbackContext obj) => Move = Vector2.zero;
    private void OnMovePerformed(InputAction.CallbackContext obj) => Move = obj.ReadValue<Vector2>();
    private void OnEnable()
    {
        inputActions.Enable();
        SubscribeInputActions();
    }
    private void OnDisable()
    {
        UnSubscribeInputActions();
        inputActions.Disable();
    }
    private void OnDestroy() => inputActions.Dispose();
}
