using UnityEngine;

public enum DifficultyLevel
{
    Easy,
    Medium,
    Hard
}

[CreateAssetMenu(fileName = "NewEnemyDifficulty", menuName = "Basketball/Enemy Difficulty", order = 2)]
public class EnemyDifficultySO : ScriptableObject
{
    public DifficultyLevel difficultyLevel;
    public float minTimerDuration;
    public float maxnTimerDuration;

    public ShotType GetRandomShotType()
    {
        float rand = Random.value;

        switch (difficultyLevel)
        {
            case DifficultyLevel.Easy:
                if (rand < 0.15f) return ShotType.PerfectShot;
                if (rand < 0.45f) return ShotType.NormalShot;
                if (rand < 0.70f) return ShotType.TooHigh;
                return ShotType.NotReach;

            case DifficultyLevel.Medium:
                if (rand < 0.25f) return ShotType.PerfectShot;
                if (rand < 0.60f) return ShotType.NormalShot;
                if (rand < 0.85f) return ShotType.TooHigh;
                return ShotType.NotReach;

            case DifficultyLevel.Hard:
                if (rand < 0.40f) return ShotType.PerfectShot;
                if (rand < 0.80f) return ShotType.NormalShot;
                if (rand < 0.95f) return ShotType.TooHigh;
                return ShotType.NotReach;

            default:
                return ShotType.NormalShot;
        }
    }


}
