using Cinemachine;
using UnityEngine;

public class CameraSystem : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera ballCamera;
    [SerializeField] private CinemachineVirtualCamera finishMatchCamera;

    private void OnEnable()
    {
        UIGameTimer.Instance.OnGameEnd += OnGameEnd;
    }

    private void Start()
    {
        ballCamera.gameObject.SetActive(true);
        finishMatchCamera.gameObject.SetActive(false);
    }

    private void OnGameEnd()
    {
        ballCamera.gameObject.SetActive(false);
        finishMatchCamera.gameObject.SetActive(true);
    }

}
