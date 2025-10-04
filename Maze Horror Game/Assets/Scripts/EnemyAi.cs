using UnityEngine;
using UnityEngine.AI;

public class EnemyAi : MonoBehaviour
{
    [SerializeField] private LayerMask playerMask;
    private NavMeshAgent agent;
    private IEnemyState currentState;
    private const float MAX_VIEW_DISTANCE = 30f;
    public void ChangeEnemyState(IEnemyState newState) 
    {
        currentState?.Exit(this);
        currentState = newState;
        currentState.Enter(this);
    }

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }
    void Start()
    {
        ChangeEnemyState(new EnemyWanderState());
    }
    void Update()
    {
        currentState?.Execute(this);
        
    }
    public void SetEnemyDestination(Vector3 destination) 
    {
        agent.SetDestination(destination);
    }
    public void IsPlayerInSight()
    {
        Vector3 eyePos = transform.position + Vector3.up * 1.8f;
        Vector3 dirToPlayer = (Player.Instance.transform.position - eyePos).normalized;
        float angle = Vector3.Angle(transform.forward, dirToPlayer);
        if (angle <= 45f || Vector3.Distance(eyePos, Player.Instance.transform.position) < MAX_VIEW_DISTANCE / 3f)
        {
            if (Physics.Raycast(eyePos, dirToPlayer, out RaycastHit hit, MAX_VIEW_DISTANCE, playerMask))
            {
                bool isPlayer = hit.collider.TryGetComponent<Player>(out Player playerHit);
                if (isPlayer)
                {
                    if (currentState is not EnemyChaseState)
                        ChangeEnemyState(new EnemyChaseState());
                }
                else if (currentState is EnemyChaseState)
                {
                    ChangeEnemyState(new EnemyFindState());
                }
            }

        }

    }
}
