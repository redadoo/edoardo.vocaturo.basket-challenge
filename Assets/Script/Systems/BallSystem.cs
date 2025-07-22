using System.Collections;
using UnityEngine;

public class BallSystem : MonoBehaviour
{
    [Header("")]
    [SerializeField] private float shootAngle = 45f;
    [SerializeField] private float gravity = 9.81f;
    [SerializeField] private Transform hopperTransform;

    private Rigidbody rig;

    [ContextMenu("Test")]
    public void ShootBall()
    {
        rig = GetComponent<Rigidbody>();
        rig.velocity = Vector3.zero;
        rig.angularVelocity = Vector3.zero;

        Vector3 velocity = CalculateVelocity(hopperTransform.position, transform.position, shootAngle);
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
}
