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
    [SerializeField] private bool isFirst = true;
    
    private Rigidbody rig;

    public event Action OnBallHitFloor;
    public event Action OnBallScored;
    public event Action OnBackboardHit;


    private void Start()
    {
        startPos = transform.position;
        startRot = transform.rotation;
        rig = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!isFirst)
        {
            if (collision.transform.CompareTag("Floor"))
            {
                ResetPos();
                OnBallHitFloor?.Invoke();
            }
        }
        else
            isFirst = false;
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.transform.CompareTag("Basket"))
            OnBallScored?.Invoke();
        else if(collision.transform.CompareTag("Backboard"))
            OnBackboardHit?.Invoke();
    }

    [ContextMenu("PerfectShootBall")]
    public void PerfectShootBall()
    {
        rig.velocity = Vector3.zero;
        rig.angularVelocity = Vector3.zero;

        Vector3 velocity = CalculateVelocity(hopperTransform.position, transform.position, shootAngle);
        rig.AddForce(velocity, ForceMode.VelocityChange);
    }

    [ContextMenu("ShootBall")]
    public void HighShootBall()
    {
        rig.velocity = Vector3.zero;
        rig.angularVelocity = Vector3.zero;

        Vector3 velocity = CalculateVelocity(backBoardTransform.position, transform.position, shootAngle);
        rig.AddForce(velocity, ForceMode.VelocityChange);
    }

    public void ShootBall(ShotType shotType, ShotInfoSO shotInfo)
    {
        rig.velocity = Vector3.zero;
        rig.angularVelocity = Vector3.zero;
        startPos = transform.position;

        Vector3 targetPos = hopperTransform.position;
        float shotAngle = shotInfo.perfectShotAngle;

        switch (shotType)
        {
            case ShotType.NotReach:
                targetPos += Vector3.down * 2f;
                shotAngle = shotInfo.perfectShotAngle;
                break;
            case ShotType.PerfectShot:
                targetPos = hopperTransform.position;
                shotAngle = shotInfo.perfectShotAngle;
                break;
            case ShotType.HighShot:
                targetPos = backBoardTransform.position;
                shotAngle = shotInfo.highShotAngle;
                break;
            case ShotType.TooHigh:
                targetPos = backBoardTransform.position + (backBoardTransform.up * 0.9f);
                shotAngle = shotInfo.perfectShotAngle;
                break;
            default:
                break;
        }

        print($"shot of type {shotType} at {targetPos} with angle {shotAngle}");

        Vector3 velocity = CalculateVelocity(targetPos, transform.position, shotAngle);

        print($"asdss velotic {velocity}");

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
