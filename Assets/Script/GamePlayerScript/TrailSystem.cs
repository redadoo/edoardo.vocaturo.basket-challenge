using UnityEngine;

/// <summary>
/// Controls the trail based on player input and camera positioning.
/// </summary>
public class TrailSystem : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private TrailRenderer trailRenderer;
    [SerializeField] private Camera mainCamera;

    private const float TrailXPosition = -0.3000002f;
    private bool isActive = false;

    private void Awake()
    {
        if (trailRenderer == null)
            trailRenderer = GetComponent<TrailRenderer>();

        if (mainCamera == null)
            mainCamera = Camera.main;
    }

    private void Update()
    {
        if (!isActive)
            return;

        UpdateTrailPosition();
    }

    /// <summary>
    /// Updates the trail position based on player input.
    /// </summary>
    private void UpdateTrailPosition()
    {
        if (InputManager.Instance == null || !InputManager.Instance.inputActions.Player.OnClick.IsPressed())
            return;

        Vector2 screenPos = InputManager.Instance.touchPos;
        float depth = Mathf.Abs(mainCamera.transform.position.x - TrailXPosition);

        Vector3 worldPos = mainCamera.ScreenToWorldPoint(new Vector3(screenPos.x, screenPos.y, depth));
        trailRenderer.transform.position = worldPos;

        if (!trailRenderer.emitting)
            trailRenderer.emitting = true;
    }

    /// <summary>
    /// Enables or disables the trail system.
    /// </summary>
    /// <param name="isTrailActive">Whether the trail should be active.</param>
    public void ChangeTrailState(bool isTrailActive)
    {
        isActive = isTrailActive;

        if (trailRenderer != null)
            trailRenderer.emitting = isTrailActive;
    }
}
