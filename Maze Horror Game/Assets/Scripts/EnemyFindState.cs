using UnityEngine;
public class EnemyFindState : IEnemyState
{
    private float findTime = 2.5f;
    public void Enter(EnemyAi enemy)
    {
        findTime = UnityEngine.Random.Range(1.5f, 2.5f);
        Debug.Log("enemy is trying to find player!");
    }

    public void Execute(EnemyAi enemy)
    {
        findTime -= Time.deltaTime;
        enemy.SetEnemyDestination(Player.Instance.transform.position);
        if (findTime <= 0)
        {
            enemy.SetEnemyDestination(Player.Instance.transform.position);
            enemy.ChangeEnemyState(new EnemyRestState());
        }
    }

    public void Exit(EnemyAi enemy)
    {
        Debug.Log("enemy Finished Finding");
    }
}
