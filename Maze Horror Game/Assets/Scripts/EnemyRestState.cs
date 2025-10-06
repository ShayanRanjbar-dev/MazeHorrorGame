using UnityEngine;
public class EnemyRestState : IEnemyState
{
    private float restTime;
    public void Enter(EnemyAi enemy)
    {
        restTime = UnityEngine.Random.Range(1f, 2.5f);
        Debug.Log($"enemy rest for {restTime} seconds");
    }

    public void Execute(EnemyAi enemy)
    {
        enemy.IsPlayerInSight();
        restTime -= Time.deltaTime;
        if (restTime <= 0)
        {
            enemy.ChangeEnemyState(new EnemyWanderState());
        }
    }

    public void Exit(EnemyAi enemy)
    {
        Debug.Log("enemy finished resting");
    }

}
