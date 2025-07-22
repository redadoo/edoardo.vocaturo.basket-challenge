using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : GenericSingleton<InputManager>
{
    public PlayerInputMap inputActions;

    public Vector2 touchPos { get; private set; }
    public Vector2? firstTouchPos { get; private set; } = null;

    public override void Awake()
    {
        base.Awake();
        inputActions = new PlayerInputMap();
    }

    private void OnEnable()
    {
        inputActions.Player.Enable();

        inputActions.Player.PointerPosition.performed += OnPointerPosition;
        inputActions.Player.OnClick.started += OnClickStarted;
        inputActions.Player.OnClick.canceled += OnClickCanceled;
    }

    private void OnDisable()
    {
        inputActions.Player.PointerPosition.performed -= OnPointerPosition;
        inputActions.Player.OnClick.started -= OnClickStarted;
        inputActions.Player.OnClick.canceled -= OnClickCanceled;

        inputActions.Player.Disable();
    }

    private void OnClickStarted(InputAction.CallbackContext context)
    {
        firstTouchPos = touchPos;
    }

    private void OnClickCanceled(InputAction.CallbackContext context)
    {
        firstTouchPos = null;
    }

    private void OnPointerPosition(InputAction.CallbackContext context)
    {
        touchPos = context.ReadValue<Vector2>();
    }
}
