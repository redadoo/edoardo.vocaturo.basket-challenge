using UnityEngine;

[CreateAssetMenu(fileName = "NewCampType", menuName = "Basketball/Camp type", order = 2)]
public class CampTypeSO : ScriptableObject
{
    public int matchDuration;
    public int matchRewardValue;
    public int matchAdmissionFee;
    public EnemyDifficultySO enemyDifficulty;
}
