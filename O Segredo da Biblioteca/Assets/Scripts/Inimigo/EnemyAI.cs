using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyAI : MonoBehaviour
{
    public Transform player;
    public Transform returnTarget;
    private Animator animator;

    [Header("Patrulha")]
    public Transform[] patrolPoints;
    public float patrolWaitTime = 1f;
    private int patrolIndex = 0;
    private float patrolWaitCounter = 0f;

    [Header("Percepção")]
    public float chaseRadius = 10f;
    public float giveUpDistance = 30f;
    public bool requireLineOfSight = true;
    public LayerMask obstacleMask;
    public float loseSightTime = 2.0f;

    [Header("NavMesh Config")]
    public float agentSpeed = 3.5f;
    public float agentRadius = 0.5f;
    public ObstacleAvoidanceType avoidance = ObstacleAvoidanceType.HighQualityObstacleAvoidance;
    public bool autoRepath = true;

    [Header("Auto Fix")]
    public bool autoRepairIfOffMesh = true;
    public float sampleRadius = 5f;

    NavMeshAgent agent;
    float lastSeenTime = -999f;
    Vector3 startPosition;

    enum State { Patrol, Chase, Return }
    State state = State.Patrol;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = agentSpeed;
        agent.radius = agentRadius;
        agent.obstacleAvoidanceType = avoidance;
        agent.autoRepath = autoRepath;

        startPosition = transform.position;
    }

    void Start()
    {
        animator = GetComponent<Animator>();

        if (player == null)
        {
            var p = GameObject.FindGameObjectWithTag("Player");
            if (p) player = p.transform;
        }

        if (!agent.isOnNavMesh && autoRepairIfOffMesh)
            TryRepairAgent();

        if (patrolPoints.Length > 0)
            agent.SetDestination(patrolPoints[patrolIndex].position);
    }

    void Update()
    {
        if (!agent.isOnNavMesh && autoRepairIfOffMesh)
            TryRepairAgent();

        // animação simples de movimento
        if (animator != null)
            animator.SetBool("IsMoving", agent.velocity.magnitude > 0.1f);

        switch (state)
        {
            case State.Patrol: UpdatePatrol(); break;
            case State.Chase: UpdateChase(); break;
            case State.Return: UpdateReturn(); break;
        }
    }

    // -----------------------
    //        PATROL
    // -----------------------
    void UpdatePatrol()
    {
        if (player != null && InChaseRange() && HasLineOfSightOrNotRequired())
        {
            lastSeenTime = Time.time;
            state = State.Chase;
            return;
        }

        if (patrolPoints.Length == 0) return;

        float dist = Vector3.Distance(transform.position, patrolPoints[patrolIndex].position);

        if (dist <= 0.5f)
        {
            patrolWaitCounter += Time.deltaTime;

            if (patrolWaitCounter >= patrolWaitTime)
            {
                patrolIndex = (patrolIndex + 1) % patrolPoints.Length;
                agent.SetDestination(patrolPoints[patrolIndex].position);
                patrolWaitCounter = 0f;
            }
        }
    }

    // -----------------------
    //        CHASE
    // -----------------------
    void UpdateChase()
    {
        if (player == null)
        {
            StartReturn();
            return;
        }

        float sqrDist = (player.position - transform.position).sqrMagnitude;

        if (sqrDist > giveUpDistance * giveUpDistance)
        {
            StartReturn();
            return;
        }

        bool sees = HasLineOfSightOrNotRequired();
        if (sees)
            lastSeenTime = Time.time;
        else if (Time.time - lastSeenTime > loseSightTime)
        {
            StartReturn();
            return;
        }

        if (agent.isOnNavMesh)
        {
            NavMeshPath path = new NavMeshPath();
            agent.CalculatePath(player.position, path);
            if (path.status == NavMeshPathStatus.PathComplete)
                agent.SetPath(path);
        }

        if (sqrDist <= 1.2f * 1.2f)
            OnCatch();
    }

    // -----------------------
    //        RETURN
    // -----------------------
    void UpdateReturn()
    {
        Vector3 dest = returnTarget != null ? returnTarget.position : startPosition;

        if (agent.isOnNavMesh)
            agent.SetDestination(dest);

        if ((transform.position - dest).sqrMagnitude <= 0.5f * 0.5f)
        {
            agent.ResetPath();
            state = State.Patrol;

            if (patrolPoints.Length > 0)
                agent.SetDestination(patrolPoints[patrolIndex].position);
        }
    }

    // -----------------------
    //   AUXILIARES
    // -----------------------
    bool HasLineOfSightOrNotRequired()
    {
        if (!requireLineOfSight) return true;
        if (player == null) return false;

        Vector3 from = transform.position + Vector3.up * 1.2f;
        Vector3 to = player.position + Vector3.up * 1f;
        Vector3 dir = to - from;

        if (Physics.Raycast(from, dir.normalized, out RaycastHit hit, dir.magnitude, obstacleMask))
            return false;

        return true;
    }

    bool InChaseRange()
    {
        float sq = (player.position - transform.position).sqrMagnitude;
        return sq <= chaseRadius * chaseRadius;
    }

    void StartReturn()
    {
        state = State.Return;
    }

    void OnCatch()
    {
        agent.isStopped = true;
        var gm = GameOverManager.Instance;
        if (gm != null) gm.ShowGameOver();
        StartReturn();
    }

    void TryRepairAgent()
    {
        if (NavMesh.SamplePosition(transform.position, out NavMeshHit hit, sampleRadius, NavMesh.AllAreas))
            agent.Warp(hit.position);
    }
}
