using System.Collections.Generic;
using UnityEngine;
using System;

public enum RangeType
{
    Out,
    Perfect,
    High
}

public class PlayerShootingSystem : MonoBehaviour
{
    [Serializable]
    public struct ShotRangeInfo
    {
        public float perfectShotMin;
        public float perfectShotMax;

        public float highShotMin;
        public float highShotMax;
    }

    [Header("Gameplay Settings")]
    [SerializeField] private List<ShotRangeInfo> shotRanges;
    [SerializeField] private int positionIndex;

    [Header("References")]
    [SerializeField] private Camera mainCamera;
    [SerializeField] private TrailRenderer trailRenderer;
    [SerializeField] private FillBarSystem manageFillBar;
    [SerializeField] private BallSystem ballSystem;

    private const float TRAIL_X_POS = -7f;

    private void Awake()
    {
        if (mainCamera == null)
            mainCamera = Camera.main;

        if (trailRenderer == null)
            trailRenderer = GetComponent<TrailRenderer>();
    }

    private void Start()
    {
        positionIndex = 0;

        shotRanges = new List<ShotRangeInfo>
        {
            new() { perfectShotMin = 0.4f, perfectShotMax = 0.5f, highShotMin = 0.55f, highShotMax = 0.6f },
            new() { perfectShotMin = 0.5f, perfectShotMax = 0.6f, highShotMin = 0.65f, highShotMax = 0.7f },
            new() { perfectShotMin = 0.7f, perfectShotMax = 0.8f, highShotMin = 0.9f, highShotMax = 1f },
        };

        GameManager.Instance.OnGameStart += OnGameStart;
        InputManager.Instance.inputActions.Player.OnClick.canceled += OnClick_canceled;
    }

    private void OnClick_canceled(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        ShootBall();
    }

    private void Update()
    {
        if (GameManager.Instance.gameState != GameState.Playing)
            return;

        UpdateTrailPosition();
    }

    private void OnGameStart(object sender, EventArgs e)
    {
        manageFillBar.SetShotRange(shotRanges[positionIndex].perfectShotMin, shotRanges[positionIndex].highShotMin);
    }

    private void UpdateTrailPosition()
    {
        if (!InputManager.Instance.inputActions.Player.OnClick.IsPressed())
            return;

        Vector2 screenPos = InputManager.Instance.touchPos;
        float depth = Mathf.Abs(mainCamera.transform.position.x - TRAIL_X_POS);

        Vector3 worldPos = mainCamera.ScreenToWorldPoint(new Vector3(screenPos.x, screenPos.y, depth));
        worldPos.x = TRAIL_X_POS;

        trailRenderer.transform.position = worldPos;

        if (!trailRenderer.emitting)
            trailRenderer.emitting = true;
    }

    private RangeType GetRangeType(float fillAmount)
    {
        if (fillAmount >= shotRanges[positionIndex].perfectShotMin && fillAmount <= shotRanges[positionIndex].perfectShotMax)
            return RangeType.Perfect;

        if (fillAmount >= shotRanges[positionIndex].highShotMin && fillAmount <= shotRanges[positionIndex].highShotMax)
            return RangeType.High;

        return RangeType.Out;
    }

    private void ShootBall()
    {
        RangeType rangeType = GetRangeType(manageFillBar.GetFillAmount());

        ballSystem.ShootBall();
    }
}