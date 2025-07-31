using System.Collections.Generic;
using UnityEngine;
public enum ShotType
{
    NotReach,
    PerfectShot,
    NormalShot,
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

    [Tooltip("Position avaible for this shot")]
    public List<GameObject> shotPositions;

    /// <summary>
    /// Returns the ShotType for the given fill value, considering tolerance.
    /// </summary>
    /// <returns>Corresponding ShotType.</returns>
    public ShotType GetShotType(float fillValue)
    {
        if (fillValue < (perfectMin - tolerance))
            return ShotType.NotReach;

        if (fillValue >= (perfectMin - tolerance) && fillValue < perfectMin)
            return ShotType.NormalShot;

        if (fillValue >= perfectMin && fillValue <= perfectMax)
            return ShotType.PerfectShot;

        if (fillValue > perfectMax && fillValue <= perfectMax + tolerance)
            return ShotType.NormalShot;

        if (fillValue >= highMin && fillValue <= highMax)
            return ShotType.NormalShot;
        
        return ShotType.TooHigh;
    }


    public void SetTransform(Transform transform, int index, bool isPlayer)
    {
        index = index >= shotPositions.Count ? shotPositions.Count - 1 : index;
        Vector3 pos = shotPositions[index].transform.position;
        if (isPlayer) pos.z -= 1;
        else pos.z += 1;
        transform.SetPositionAndRotation(pos, shotPositions[index].transform.rotation); 
    }

}
