using UnityEngine;

public class TrailSystem : MonoBehaviour
{
    [SerializeField] private TrailRenderer trailRenderer;
    [SerializeField] private Camera mainCamera;

    private const float TrailXPosition = -7f;
    private bool isActive = true;

    private void Awake()
    {
        if (trailRenderer == null) 
            trailRenderer = GetComponent<TrailRenderer>();
        if (mainCamera == null) 
            mainCamera = Camera.main;
    }

    private void OnEnable()
    {
        if (ShootingSystem.Instance != null)
            ShootingSystem.Instance.OnTimerEnd += OnTimerEnd;
    }

    private void OnDisable()
    {
        if (ShootingSystem.Instance != null)
            ShootingSystem.Instance.OnTimerEnd -= OnTimerEnd;
    }

    private void OnTimerEnd()
    {
        isActive = false;
        if (trailRenderer != null)
            trailRenderer.emitting = false;
    }

    private void Update()
    {
        if (isActive)
            UpdateTrailPosition();
    }

    private void UpdateTrailPosition()
    {
        if (!InputManager.Instance.inputActions.Player.OnClick.IsPressed())
            return;

        Vector2 screenPos = InputManager.Instance.touchPos;
        float depth = Mathf.Abs(mainCamera.transform.position.x - TrailXPosition);

        Vector3 worldPos = mainCamera.ScreenToWorldPoint(new Vector3(screenPos.x, screenPos.y, depth));
        worldPos.x = TrailXPosition;

        trailRenderer.transform.position = worldPos;

        if (!trailRenderer.emitting)
            trailRenderer.emitting = true;
    }
}
