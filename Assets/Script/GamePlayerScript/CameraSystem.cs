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
    [SerializeField] private CinemachineTargetGroup targetGroup;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private BallSystem playerBall;

    private void OnEnable()
    {
        playerBall.OnShotBall += OnShotBall;
        playerBall.OnBallHitFloor += OnBallHitFloor;

        if (UIGameTimer.TryGetInstance())
            UIGameTimer.Instance.OnGameEnd += OnGameEnd;
    }

    private void OnDisable()
    {
        playerBall.OnShotBall -= OnShotBall;
        playerBall.OnBallHitFloor -= OnBallHitFloor;

        if (UIGameTimer.TryGetInstance())
            UIGameTimer.Instance.OnGameEnd -= OnGameEnd;
    }

    private void Start()
    {
        ActivateBallCamera();
    }

    private void OnShotBall()
    {
        var oldTargets = targetGroup.m_Targets;
        var newTargets = new System.Collections.Generic.List<CinemachineTargetGroup.Target>();

        foreach (var t in oldTargets)
        {
            if (t.target != playerTransform)
                newTargets.Add(t);
        }

        targetGroup.m_Targets = newTargets.ToArray();
    }

    private void OnBallHitFloor()
    {
        foreach (var t in targetGroup.m_Targets)
        {
            if (t.target == playerTransform)
                return;
        }

        var oldTargets = targetGroup.m_Targets;
        var newTargets = new CinemachineTargetGroup.Target[oldTargets.Length + 1];
        oldTargets.CopyTo(newTargets, 0);

        newTargets[oldTargets.Length] = new CinemachineTargetGroup.Target
        {
            target = playerTransform,
            weight = 3f,
            radius = 1f
        };

        targetGroup.m_Targets = newTargets;
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
