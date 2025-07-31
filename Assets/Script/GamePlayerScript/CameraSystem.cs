using Cinemachine;
using UnityEngine;
using UIScript;

/// <summary>
/// Controls the switching between cameras during and after a match.
/// </summary>
public class CameraSystem : MonoBehaviour
{
    [Header("Camera References")]
    [SerializeField] private CinemachineVirtualCamera ballCamera;
    [SerializeField] private CinemachineVirtualCamera finishMatchCamera;

    private void OnEnable()
    {
        if (UIGameTimer.TryGetInstance())
            UIGameTimer.Instance.OnGameEnd += OnGameEnd;
    }

    private void OnDisable()
    {
        if (UIGameTimer.TryGetInstance())
            UIGameTimer.Instance.OnGameEnd -= OnGameEnd;
    }

    private void Start()
    {
        ActivateBallCamera();
    }

    /// <summary>
    /// Activates the ball-following camera and disables the end-match camera.
    /// </summary>
    private void ActivateBallCamera()
    {
        if (ballCamera != null) ballCamera.gameObject.SetActive(true);
        if (finishMatchCamera != null) finishMatchCamera.gameObject.SetActive(false);
    }

    /// <summary>
    /// Callback when the game ends. Switches to the match-end camera.
    /// </summary>
    private void OnGameEnd()
    {
        if (ballCamera != null) ballCamera.gameObject.SetActive(false);
        if (finishMatchCamera != null) finishMatchCamera.gameObject.SetActive(true);
    }
}
