using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections.Generic;
using System;

public class InputManager : PersistentSingleton<InputManager>
{
    public PlayerInputMap inputActions;

    public Vector2 touchPos { get; private set; }
    public Vector2? firstTouchPos { get; private set; } = null;

    public EventSystem eventSystem;
    public GraphicRaycaster graphicRaycaster;

    public event Action OnClickStartNotOverUI;
    public event Action OnClickCanceledNotOverUI;

    private void Start()
    {
        FindReference();

        LoadingSceneManager.Instance.OnSceneChange += OnSceneChange;
    }

    private void FindReference()
    {
        if (eventSystem == null)
            eventSystem = FindObjectOfType<EventSystem>();
        if (graphicRaycaster == null)
            graphicRaycaster = FindObjectOfType<GraphicRaycaster>();
    }

    private void OnEnable()
    {
        inputActions = new PlayerInputMap();
        inputActions.Player.Enable();

        inputActions.Player.PointerPosition.performed += OnPointerPosition;
        inputActions.Player.OnClick.started += OnClickStarted;
        inputActions.Player.OnClick.canceled += OnClickCanceled;
    }

    private void OnClickStarted(InputAction.CallbackContext context)
    {
        if (!IsPointerOverUI())
        {
            firstTouchPos = touchPos;
            OnClickStartNotOverUI?.Invoke();
        }
    }

    private void OnClickCanceled(InputAction.CallbackContext context)
    {
        firstTouchPos = null;
        if (!IsPointerOverUI())
            OnClickCanceledNotOverUI?.Invoke();
    }

    private void OnPointerPosition(InputAction.CallbackContext context) =>
        touchPos = context.ReadValue<Vector2>();

    private void OnSceneChange(object sender, Scene e) =>
        FindReference();

    public bool IsPointerOverUI()
    {
        PointerEventData pointerData = new PointerEventData(eventSystem)
        {
            position = touchPos
        };

        List<RaycastResult> results = new List<RaycastResult>();
        graphicRaycaster.Raycast(pointerData, results);
        return results.Count > 0;
    }
}
