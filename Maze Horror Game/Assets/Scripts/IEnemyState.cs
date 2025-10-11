public interface IEnemyState 
{
    public void Enter(EnemyAi enemy);
    public void Exit(EnemyAi enemy);
    public void Execute(EnemyAi enemy);
}
