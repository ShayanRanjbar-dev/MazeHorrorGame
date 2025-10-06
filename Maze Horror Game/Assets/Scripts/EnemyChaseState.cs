using UnityEngine;
public class EnemyChaseState : IEnemyState
{
    public void Enter(EnemyAi enemy)
    {
        Debug.Log("Enemy is Chasing Player!");
    }

    public void Execute(EnemyAi enemy)
    {
        enemy.IsPlayerInSight();
        enemy.SetEnemyDestination(Player.Instance.transform.position);
    }

    public void Exit(EnemyAi enemy)
    {
        Debug.Log("enemy quits chasing");
    }
}
