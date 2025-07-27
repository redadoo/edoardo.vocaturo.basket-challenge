using UnityEngine;

public abstract class ShooterController : MonoBehaviour
{
    [Header("ShooterController reference")]
    [SerializeField] protected ShootingManager shootingManager;
    [SerializeField] protected BallSystem ballSystem;

    [SerializeField] protected ShotInfoSO currentShotInfo;
    [SerializeField] protected int currentPositionIndex;
    [SerializeField] protected bool isPossibleToShoot;
    [SerializeField] protected bool hasScored;
    [SerializeField] protected ShotType shotType;
    [SerializeField] protected int pointScored;
    [SerializeField] public int points { get; protected set; }

    protected virtual void OnEnable()
    {
        if (ballSystem != null)
        {
            ballSystem.OnBallHitFloor += OnBallHitFloor;
            ballSystem.OnBallScored += OnBallScored;
            ballSystem.OnBackboardHit += OnBackboardHit;
        }
    }

    protected virtual void OnDestroy()
    {
        if (ballSystem != null)
        {
            ballSystem.OnBallHitFloor -= OnBallHitFloor;
            ballSystem.OnBallScored -= OnBallScored;
            ballSystem.OnBackboardHit -= OnBackboardHit;
        }
    }

    public virtual void Init(ShotInfoSO shotInfo)
    {
        currentShotInfo = shotInfo;
        isPossibleToShoot = true;
    }


    public void SetShotInfo(ShotInfoSO shotInfo)
    {
        currentShotInfo = shotInfo;
    }

    protected virtual void OnBallHitFloor()
    {
        if (pointScored != 0 && pointScored % 3 == 0)
        {
            currentPositionIndex++;
            currentShotInfo = shootingManager.GetShotRange(currentPositionIndex);
        }
    }

    protected virtual void OnBallScored()
    {
        if (shotType == ShotType.PerfectShot)
            points += 3;
        else if (shotType == ShotType.HighShot)
            points += 2;
        else
            return;

        pointScored++;
    }

    protected virtual void OnBackboardHit() { }

}
