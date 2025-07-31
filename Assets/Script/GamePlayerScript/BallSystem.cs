using UnityEngine;
using System;

/// <summary>
/// Handles all logic related to the basketball's physics and scoring interactions.
/// </summary>
public class BallSystem : MonoBehaviour
{
    [Header("Shooting Parameters")]
    [Tooltip("Gravity used to calculate ball trajectories.")]
    [SerializeField] private float gravity = 9.81f;

    [Tooltip("Target position for perfect shots.")]
    [SerializeField] private Transform hopperTransform;

    [Tooltip("Target position for high and too-high shots.")]
    [SerializeField] private Transform backboardTransform;

    [Header("Ball State")]
    [SerializeField] private Vector3 initialPosition;
    [SerializeField] private Quaternion initialRotation;
    [SerializeField] private bool isFirstThrow = true;

    private Rigidbody rb;

    public event Action OnBallHitFloor;
    public event Action OnBallScored;
    public event Action<BackboardBonus> OnBackboardHit;

    private const float DropDistanceNotReach = 2f;
    private const float TooHighUpwardOffset = 0.9f;
    private const float TooHighAnglePenalty = 8f;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        initialPosition = transform.localPosition;
        initialRotation = transform.localRotation;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!isFirstThrow)
        {
            if (collision.transform.CompareTag("Floor"))
            {
                ResetBall();
                OnBallHitFloor?.Invoke();
            }
        }
        else
        {
            isFirstThrow = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Basket"))
        {
            OnBallScored?.Invoke();
        }
        else if (other.CompareTag("Backboard") && other.TryGetComponent(out BackboardBonus bonus))
        {
            OnBackboardHit?.Invoke(bonus);
        }
    }

    /// <summary>
    /// Shoots the ball based on the shot type and parameters.
    /// </summary>
    public void ShootBall(ShotType shotType, ShotInfoSO shotInfo)
    {
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        Vector3 targetPosition = hopperTransform.position;
        float shotAngle = shotInfo.perfectShotAngle;

        switch (shotType)
        {
            case ShotType.NotReach:
                targetPosition += Vector3.down * DropDistanceNotReach;
                break;

            case ShotType.PerfectShot:
                break;

            case ShotType.NormalShot:
                targetPosition = backboardTransform.position;
                shotAngle = shotInfo.highShotAngle;
                break;

            case ShotType.TooHigh:
                targetPosition = backboardTransform.position + (backboardTransform.up * TooHighUpwardOffset);
                shotAngle -= TooHighAnglePenalty;
                break;
        }

        Vector3 launchVelocity = CalculateLaunchVelocity(targetPosition, transform.position, shotAngle);
        rb.AddForce(launchVelocity, ForceMode.VelocityChange);
    }

    /// <summary>
    /// Calculates the required launch velocity to reach a target point at a specific angle.
    /// </summary>
    /// <returns>The calculated velocity vector.</returns>
    private Vector3 CalculateLaunchVelocity(Vector3 target, Vector3 origin, float angleDegrees)
    {
        Vector3 displacement = target - origin;
        Vector3 horizontal = new(displacement.x, 0f, displacement.z);

        float distance = horizontal.magnitude;
        float height = displacement.y;
        float angleRadians = angleDegrees * Mathf.Deg2Rad;

        float cos = Mathf.Cos(angleRadians);
        float sin = Mathf.Sin(angleRadians);

        float denominator = 2f * cos * cos * (distance * Mathf.Tan(angleRadians) - height);

        if (denominator <= 0f)
            return Vector3.zero;

        float velocityMagnitude = Mathf.Sqrt((gravity * distance * distance) / denominator);

        Vector3 velocity = horizontal.normalized * (velocityMagnitude * cos);
        velocity.y = velocityMagnitude * sin;

        return velocity;
    }

    /// <summary>
    /// Resets the ball's position and velocity to its initial state.
    /// </summary>
    public void ResetBall()
    {
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        transform.localPosition = initialPosition;
        transform.localRotation = initialRotation;
    }
}
