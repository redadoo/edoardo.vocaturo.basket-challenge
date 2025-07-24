using System;
using System.Collections;
using UnityEngine;

public class BallSystem : MonoBehaviour
{
    [Header("Shot parameter")]
    [SerializeField] private float shootAngle = 45f;
    [SerializeField] private float gravity = 9.81f;
    [SerializeField] private Transform hopperTransform;
    [SerializeField] private Transform backBoardTransform;

    [Header("Ball value")]
    [SerializeField] private Vector3 startPos;
    [SerializeField] private Quaternion startRot;

    private Rigidbody rig;
    public event Action OnBallHitFloor;


    private void Start()
    {
        startPos = transform.position;
        startRot = transform.rotation;
        rig = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Floor"))
        {
            ResetPos();
            OnBallHitFloor?.Invoke();
        }
    }

    [ContextMenu("ShootBall")]
    public void PerfectShootBall()
    {
        rig.velocity = Vector3.zero;
        rig.angularVelocity = Vector3.zero;

        Vector3 velocity = CalculateVelocity(hopperTransform.position, transform.position, shootAngle);
        rig.AddForce(velocity, ForceMode.VelocityChange);
    }

    public void ShootBall(ShotType shotType)
    {
        rig.velocity = Vector3.zero;
        rig.angularVelocity = Vector3.zero;

        Vector3 targetPos = hopperTransform.position;

        switch (shotType)
        {
            case ShotType.NotReach:
                targetPos += (Vector3.right * 2);
                break;
            case ShotType.PerfectShot:
                targetPos = hopperTransform.position;
                break;
            case ShotType.HighShot:
                targetPos = backBoardTransform.position * 1.2f;
                break;
            case ShotType.TooHigh:
                targetPos = backBoardTransform.position + (backBoardTransform.up * 0.8f);
                break;
            default:
                break;
        }

        Vector3 velocity = CalculateVelocity(targetPos, transform.position, shootAngle);
        rig.AddForce(velocity, ForceMode.VelocityChange);
    }

    private Vector3 CalculateVelocity(Vector3 target, Vector3 origin, float angle)
    {
        Vector3 direction = target - origin;
        Vector3 directionXZ = new(direction.x, 0f, direction.z);

        float distance = directionXZ.magnitude;
        float heightDifference = direction.y;
        float radianAngle = angle * Mathf.Deg2Rad;

        float numerator = gravity * distance * distance;
        float denominator = 2f * Mathf.Pow(Mathf.Cos(radianAngle), 2) * (distance * Mathf.Tan(radianAngle) - heightDifference);

        if (denominator <= 0)
            return Vector3.zero;

        float velocityMagnitude = Mathf.Sqrt(numerator / denominator);

        Vector3 result = directionXZ.normalized;
        result *= velocityMagnitude * Mathf.Cos(radianAngle);
        result.y = velocityMagnitude * Mathf.Sin(radianAngle);

        return result;
    }

    private void ResetPos()
    {
        rig.velocity = Vector3.zero;
        rig.angularVelocity = Vector3.zero;
        transform.position = startPos;
        transform.rotation = startRot;
    }
}
