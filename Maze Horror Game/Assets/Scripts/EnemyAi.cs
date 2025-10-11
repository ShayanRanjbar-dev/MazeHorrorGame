using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAi : MonoBehaviour
{
    [SerializeField] private LayerMask playerMask;
    [SerializeField] private Animator animator;
    [SerializeField] private Camera gameOverCamera;
    private NavMeshAgent agent;
    private IEnemyState currentState;
    private const int MAX_VIEW_DISTANCE = 30;
    private const int FOV = 60;
    private const string CATCH_PLAYER = "CatchPlayer";
    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }
    private void Start()
    {
        ChangeEnemyState(new EnemyWanderState());
        GameManager.Instance.OnEnemyCatchPlayer += OnEnemyCatchPlayer;
    }

    private void OnEnemyCatchPlayer(object sender, GameManager.EnemyCatchPlayerArgs e)
    {
        if (e.enemy == this)
        {
            return;
        }
        Destroy(gameObject);
    }

    private void Update()
    {
        IsPlayerInSight();
        currentState?.Execute(this); 
    }
    public void ChangeEnemyState(IEnemyState newState)
    {
        currentState?.Exit(this);
        currentState = newState;
        currentState.Enter(this);
    }
    public void SetEnemyDestination(Vector3 destination) 
    {
        agent.SetDestination(destination);
    }
    private void IsPlayerInSight()
    {
        Vector3 eyePos = transform.position + Vector3.up * 1.8f;
        Vector3 dirToPlayer = (Player.Instance.transform.position - eyePos).normalized;
        float angle = Vector3.Angle(transform.forward, dirToPlayer);
        if (angle <= FOV || Vector3.Distance(eyePos, Player.Instance.transform.position) < MAX_VIEW_DISTANCE / 3f)
        {
            if (Physics.Raycast(eyePos, dirToPlayer, out RaycastHit hit, MAX_VIEW_DISTANCE, playerMask))
            { 
                if (hit.collider.TryGetComponent<Player>(out _))
                {
                    ChangeEnemyState(new EnemyChaseState());
                }
                else
                {
                    if (currentState is EnemyChaseState) 
                    {
                        ChangeEnemyState(new EnemyFindState());
                    } 
                } 
                
            }

        }

    }
    public void CatchPlayer() 
    {
        GameManager.Instance.EnemyCatchPlayer(this);
        AudioSource footStep = GetComponent<AudioSource>();
        footStep.Stop();
        gameOverCamera.enabled = true;
        animator.SetBool(CATCH_PLAYER, true);
        StartCoroutine(StartGameOverTime(2.5f));
    }
    private IEnumerator StartGameOverTime(float delay) 
    {
        yield return new WaitForSeconds(delay);
        GameManager.Instance.GameOver();
    }
}
