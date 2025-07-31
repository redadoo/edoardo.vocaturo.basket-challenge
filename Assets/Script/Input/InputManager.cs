using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine;
using Utility;
using System;

/// <summary>
/// Handles player input and UI interaction detection. 
/// Ensures that inputs are processed only when not over UI elements.
/// </summary>
public class InputManager : PersistentSingleton<InputManager>
{
    public PlayerInputMap inputActions;

    public Vector2 touchPos { get; private set; }
    public Vector2? firstTouchPos { get; private set; } = null;

    public EventSystem eventSystem;
    public GraphicRaycaster graphicRaycaster;

    public event Action OnClickStartNotOverUI;
    public event Action OnClickCanceledNotOverUI;
    
    private void OnEnable()
    {
        inputActions = new PlayerInputMap();
        inputActions.Player.Enable();

        inputActions.Player.PointerPosition.performed += OnPointerPosition;
        inputActions.Player.OnClick.started += OnClickStarted;
        inputActions.Player.OnClick.canceled += OnClickCanceled;
    }

    private void Start()
    {
        FindReference();
        LoadingSceneManager.Instance.OnSceneChange += OnSceneChange;
    }

    /// <summary>
    /// Finds and saves the EventSystem and GraphicRaycaster if not already set.
    /// </summary>
    private void FindReference()
    {
        if (eventSystem == null)
            eventSystem = FindObjectOfType<EventSystem>();
        if (graphicRaycaster == null)
            graphicRaycaster = FindObjectOfType<GraphicRaycaster>();
    }

    /// <summary>
    /// Called when a click or touch begins.
    /// If the input is not over a UI element, records the start position and triggers the event.
    /// </summary>
    private void OnClickStarted(InputAction.CallbackContext context)
    {
        if (!IsPointerOverUI())
        {
            firstTouchPos = touchPos;
            OnClickStartNotOverUI?.Invoke();
        }
    }

    /// <summary>
    /// Called when a click or touch ends.
    /// If not over a UI element, triggers the cancellation event.
    /// </summary>
    private void OnClickCanceled(InputAction.CallbackContext context)
    {
        if (!IsPointerOverUI())
        {
            firstTouchPos = null;
            OnClickCanceledNotOverUI?.Invoke();
        }
    }

    /// <summary>
    /// Checks whether the pointer is currently over a UI element.
    /// Uses the GraphicRaycaster and EventSystem to perform a UI raycast.
    /// </summary>
    /// <returns>True if over UI, false otherwise.</returns>
    private bool IsPointerOverUI()
    {
        PointerEventData pointerData = new(eventSystem)
        {
            position = touchPos
        };

        List<RaycastResult> results = new();

        if (graphicRaycaster == null)
            FindReference();

        graphicRaycaster.Raycast(pointerData, results);
        return results.Count > 0;
    }

    /// <summary>
    /// Updates the current pointer/touch position when moved.
    /// </summary>
    private void OnPointerPosition(InputAction.CallbackContext context)
    {
        touchPos = context.ReadValue<Vector2>();
        if (firstTouchPos != null && IsPointerOverUI())
        {
            OnClickCanceledNotOverUI?.Invoke();
            firstTouchPos = null;
        }
    }

    /// <summary>
    /// Called when a new scene is loaded. Reacquires references to EventSystem and GraphicRaycaster.
    /// </summary>
    private void OnSceneChange(object sender, Scene e) =>
        FindReference();

    public bool IsPressedNotOverUi() =>
         inputActions.Player.OnClick.IsPressed() && !IsPointerOverUI();
}
