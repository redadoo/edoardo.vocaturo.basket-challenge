using UnityEngine;
public enum ShotType
{
    NotReach,
    PerfectShot,
    HighShot,
    TooHigh
}

[CreateAssetMenu(fileName = "NewShotInfo", menuName = "Basketball/Shot Info", order = 1)]
public class ShotInfoSO : ScriptableObject
{
    [Header("Perfect Shot Timing")]
    [Tooltip("Minimum fill value for a perfect shot.")]
    public float perfectMin;

    [Tooltip("Maximum fill value for a perfect shot.")]
    public float perfectMax;

    [Tooltip("Angle for shot a perfect shot")]
    public float perfectShotAngle;

    [Tooltip("Tolerance margin added to the shot value.")]
    public float tolerance;

    [Header("High Shot Timing")]
    [Tooltip("Minimum fill value for a high (but not perfect) shot.")]
    public float highMin;

    [Tooltip("Maximum fill value for a high shot.")]
    public float highMax;

    [Tooltip("Angle for shot a high shot")]
    public float highShotAngle;

    /// <summary>
    /// Returns the ShotType for the given fill value, considering tolerance.
    /// </summary>
    /// <param name="fillValue">The normalized fill value (e.g., 0 to 1) representing shot timing.</param>
    /// <returns>Corresponding ShotType.</returns>
    public ShotType GetShotType(float fillValue)
    {
        if (fillValue < (perfectMin - tolerance))
            return ShotType.NotReach;

        if (fillValue >= (perfectMin - tolerance) && fillValue <= (perfectMax + tolerance))
            return ShotType.PerfectShot;

        if (fillValue > (perfectMax + tolerance) && fillValue < highMin)
            return ShotType.TooHigh;

        if (fillValue >= highMin && fillValue <= highMax)
            return ShotType.HighShot;

        return ShotType.TooHigh;
    }
}
