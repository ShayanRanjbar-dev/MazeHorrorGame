public class EnemyChaseState : IEnemyState
{
    public void Enter(EnemyAi enemy)
    {
    }

    public void Execute(EnemyAi enemy)
    {
        enemy.SetEnemyDestination(Player.Instance.transform.position);
    }

    public void Exit(EnemyAi enemy)
    {
    }
}
