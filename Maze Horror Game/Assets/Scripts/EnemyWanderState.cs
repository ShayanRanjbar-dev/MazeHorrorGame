using UnityEngine;
public class EnemyWanderState : IEnemyState
{
    private Vector3 destination;
    private const float MIN_WANDER_DISTANCE = 1f;
    private const float MAX_DESTINATION_DISTANCE = 35f;
    
    public void Enter(EnemyAi enemy)
    {
        destination = GetDestination(enemy);
        enemy.SetEnemyDestination(destination);
    }

    public void Execute(EnemyAi enemy)
    {
        Vector2 enemyTransform = new(enemy.transform.position.x, enemy.transform.position.z);
        Vector2 NewDestination = new(destination.x, destination.z);
        if (Vector2.Distance(enemyTransform, NewDestination) <= MIN_WANDER_DISTANCE) 
        {
            enemy.ChangeEnemyState(new EnemyRestState());
        }
    }

    public void Exit(EnemyAi enemy)
    {
    }

    private Vector3 GetDestination(EnemyAi enemy)
    {
        Vector3 newDestination = GameManager.Instance.GetRandomPosition();
        if (Vector3.Distance(enemy.transform.position, newDestination) > MAX_DESTINATION_DISTANCE)
        {
            newDestination = GetDestination(enemy);
        }
        return newDestination;
    }
}
