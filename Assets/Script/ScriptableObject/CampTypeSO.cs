using UnityEngine;

[CreateAssetMenu(fileName = "NewCampType", menuName = "Basketball/Camp type", order = 3)]
public class CampTypeSO : ScriptableObject
{
    [Header("Match Settings")]

    [Tooltip("Duration of the match in seconds.")]
    public int matchDuration;

    [Tooltip("Reward given to the player after completing the match.")]
    public int matchRewardValue;

    [Tooltip("Cost required to enter this match.")]
    public int matchAdmissionFee;

    [Header("Enemy Configuration")]

    [Tooltip("Enemy difficulty settings used in this camp.")]
    public EnemyDifficultySO enemyDifficulty;
}
