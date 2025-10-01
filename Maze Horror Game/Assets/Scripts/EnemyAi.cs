using UnityEngine;
using UnityEngine.AI;

public class EnemyAi : MonoBehaviour
{
    [SerializeField] private LayerMask playerMask;
    [SerializeField] private Player player;
    private NavMeshAgent agent;
    private Vector3 destination;
    private  const float MAX_VIEW_DISTANCE = 30f;
    private float restTime = 1.5f;
    private float findTime = 2.5f;

    public enum EnemyStates 
    {
        Resting , Wandering , Chasing , Finding
    }
    public EnemyStates enemyState = EnemyStates.Resting;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }
    void Start()
    {
        destination = GetDestination();
        agent.SetDestination(destination);
        enemyState = EnemyStates.Wandering;
    }
    void Update()
    {
        IsPlayerInSight();
        if (enemyState == EnemyStates.Finding)
        {
            findTime -= Time.deltaTime;
            destination = player.transform.position;
            agent.SetDestination(destination);
            if (findTime <= 0f)
            {
                enemyState = EnemyStates.Resting;
                findTime = 2.5f;
            }
        }
        else { findTime = 2.5f; }
            Vector2 XZTransform = new(transform.position.x, transform.position.z);
        Vector2 Destin = new(destination.x, destination.z);
        if (Vector2.Distance(XZTransform, Destin) <= 1f)
        {
            enemyState = EnemyStates.Resting ;
            restTime -= Time.deltaTime;
            if (restTime <= 0f)
            {
                enemyState = EnemyStates.Wandering;
                destination = GetDestination();
                agent.SetDestination(destination);
                restTime = 1.5f;
            }
        }  
        
    }
    private Vector3 GetDestination() 
    {
        Vector3 newDestination = GameManager.Instance.GetRandomPosition();
        if (Vector3.Distance(transform.position , newDestination) > 35f)
        {
            newDestination = GetDestination();
        }
        return newDestination;
    }
    private void IsPlayerInSight() 
    {
        Vector3 eyePos = transform.position + Vector3.up * 1.8f;
        Vector3 dirToPlayer = (player.transform.position - eyePos).normalized;
        float angle = Vector3.Angle(transform.forward, dirToPlayer);
        if (angle <= 45f || Vector3.Distance(eyePos , player.transform.position) < MAX_VIEW_DISTANCE / 3f)
        {
            if (Physics.Raycast(eyePos, dirToPlayer, out RaycastHit hit, MAX_VIEW_DISTANCE, playerMask))
            {
                bool isPlayer = hit.collider.TryGetComponent<Player>(out Player playerHit);
                if (isPlayer)
                {
                    enemyState = EnemyStates.Chasing;
                    destination = playerHit.transform.position;
                    agent.SetDestination(destination);
                }
                if (!isPlayer && enemyState == EnemyStates.Chasing )
                {
                    enemyState = EnemyStates.Finding;
                }
            }
     
        }

    }
}
