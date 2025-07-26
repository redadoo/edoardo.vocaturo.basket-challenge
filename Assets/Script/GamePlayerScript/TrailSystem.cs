using UnityEngine;

public class TrailSystem : MonoBehaviour
{
    [SerializeField] private TrailRenderer trailRenderer;
    [SerializeField] private Camera mainCamera;

    private const float TrailXPosition = 5f;
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

    private void UpdateTrailPosition()
    {
        if (!InputManager.Instance.inputActions.Player.OnClick.IsPressed())
            return;

        Vector2 screenPos = InputManager.Instance.touchPos;
        float depth = Mathf.Abs(mainCamera.transform.position.x - TrailXPosition);
        Vector3 worldPos = mainCamera.ScreenToWorldPoint(new Vector3(screenPos.x, screenPos.y, depth));

        trailRenderer.transform.position = worldPos;

        if (!trailRenderer.emitting)
            trailRenderer.emitting = true;
    }

    public void ChangeTrailState(bool isTrailActive)
    {
        isActive = isTrailActive;
        if (trailRenderer != null)
            trailRenderer.emitting = isTrailActive;
    }
}
