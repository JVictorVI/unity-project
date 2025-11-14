using UnityEngine;
using UnityEngine.AI;
using System.Collections;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyBehaviorFix : MonoBehaviour
{
    public Transform player;
    public Transform returnTarget;

    [Header("Percepção")]
    public float chaseRadius = 10f;
    public float giveUpDistance = 30f;
    public bool requireLineOfSight = true;
    public LayerMask obstacleMask;      // layers que bloqueiam visão (paredes)
    public float loseSightTime = 2.0f;

    [Header("Agent tune")]
    public float agentSpeed = 3.5f;
    public float agentRadius = 0.5f;
    public ObstacleAvoidanceType avoidanceQuality = ObstacleAvoidanceType.HighQualityObstacleAvoidance;
    public bool autoRepath = true;

    [Header("Repair / sample")]
    public bool autoRepairIfOffMesh = true;
    public float sampleRadius = 5f;

    [Header("Debug")]
    public bool debugDraw = true;

    NavMeshAgent agent;
    Vector3 startPos;
    float lastSeenTime = -999f;
    enum State { Idle, Chase, Return }
    State state = State.Idle;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        // apply some safety defaults (you can override in inspector)
        agent.speed = agentSpeed;
        agent.radius = agentRadius;
        agent.obstacleAvoidanceType = avoidanceQuality;
        agent.autoRepath = autoRepath;

        startPos = transform.position;
    }

    void Start()
    {
        if (player == null)
        {
            var p = GameObject.FindGameObjectWithTag("Player");
            if (p) player = p.transform;
        }

        if (!agent.isOnNavMesh && autoRepairIfOffMesh) TryRepairAgent();
    }

    void Update()
    {
        // quick diagnostics
        if (debugDraw && player != null)
        {
            float dist = Vector3.Distance(transform.position, player.position);
            Debug.DrawLine(transform.position + Vector3.up * 1.2f, player.position + Vector3.up * 1.2f, Color.red);
        //    Debug.Log($"[E-Fix] State={state} dist={dist:F2} isOnNavMesh={agent.isOnNavMesh} vel={agent.velocity.magnitude:F2} remDist={agent.remainingDistance:F2}");
        }

        if (!agent.isOnNavMesh && autoRepairIfOffMesh) TryRepairAgent();

        switch (state)
        {
            case State.Idle: UpdateIdle(); break;
            case State.Chase: UpdateChase(); break;
            case State.Return: UpdateReturn(); break;
        }
    }

    void UpdateIdle()
    {
        if (player == null) return;
        float sqrDist = (player.position - transform.position).sqrMagnitude;
        if (sqrDist <= chaseRadius * chaseRadius)
        {
            if (!requireLineOfSight || HasLineOfSight())
            {
                lastSeenTime = Time.time;
                state = State.Chase;
           //     if (debugDraw) Debug.Log("[E-Fix] Start Chase (Idle->Chase)");
            }
        }
    }

    void UpdateChase()
    {
        if (player == null) { StartReturn(); return; }

        float sqrDist = (player.position - transform.position).sqrMagnitude;

        // hard give up if too far
        if (sqrDist > giveUpDistance * giveUpDistance)
        {
          //  if (debugDraw) Debug.Log("[E-Fix] giveUpDistance reached -> Return");
            StartReturn();
            return;
        }

        // check LOS strictly: only update lastSeenTime if HasLineOfSight() true
        bool sees = !requireLineOfSight || HasLineOfSight();
        if (sees)
        {
            lastSeenTime = Time.time;
        }
        else
        {
            float lostFor = Time.time - lastSeenTime;
          //  if (debugDraw) Debug.Log($"[E-Fix] LOS=false lostFor={lostFor:F2}s (limit {loseSightTime}s)");
            if (lostFor > loseSightTime)
            {
          //      if (debugDraw) Debug.Log("[E-Fix] Lost sight long enough -> Return");
                StartReturn();
                return;
            }
        }

        // Path calculation: only set destination if a full path exists.
        if (agent.isOnNavMesh)
        {
            NavMeshPath path = new NavMeshPath();
            agent.CalculatePath(player.position, path);

            if (path.status == NavMeshPathStatus.PathComplete)
            {
                agent.SetPath(path);
             //   if (debugDraw) Debug.Log($"[E-Fix] PathComplete -> chasing. path.corners={path.corners.Length}");
            }
            else
            {
             //   if (debugDraw) Debug.Log($"[E-Fix] Path status: {path.status} -> trying sample pos near player");
                // try to sample a nearby reachable point around player
                if (NavMesh.SamplePosition(player.position, out NavMeshHit hit, 4.0f, NavMesh.AllAreas))
                {
                    NavMeshPath p2 = new NavMeshPath();
                    agent.CalculatePath(hit.position, p2);
                    if (p2.status == NavMeshPathStatus.PathComplete)
                    {
                        agent.SetPath(p2);
                     //   if (debugDraw) Debug.Log("[E-Fix] Found path to sampled player position -> chasing to sample");
                    }
                    else
                    {
                   //     if (debugDraw) Debug.Log("[E-Fix] Sampled point path invalid -> will give up soon if still unreachable");
                        // if unreachable for long enough, forget (handled by lastSeenTime timeout)
                    }
                }
                else
                {
                   // if (debugDraw) Debug.Log("[E-Fix] No sampled navmesh near player");
                }
            }
        }

        // catch if very close (ignores path)
        if (sqrDist <= 1.2f * 1.2f)
        {
            OnCatch();
        }
    }

    void UpdateReturn()
    {
        Vector3 dest = returnTarget != null ? returnTarget.position : startPos;
        if (agent.isOnNavMesh)
        {
            agent.SetDestination(dest);
        }
        // if near enough, idle
        if ((transform.position - dest).sqrMagnitude <= 0.5f * 0.5f)
        {
            agent.ResetPath();
            state = State.Idle;
            //if (debugDraw) Debug.Log("[E-Fix] Reached return point -> Idle");
        }
    }

    bool HasLineOfSight()
    {
        if (player == null) return false;
        Vector3 from = transform.position + Vector3.up * 1.2f;
        Vector3 to = player.position + Vector3.up * 1.0f;
        Vector3 dir = to - from;
        if (Physics.Raycast(from, dir.normalized, out RaycastHit hit, dir.magnitude, obstacleMask))
        {
            if (debugDraw) Debug.DrawRay(from, dir.normalized * dir.magnitude, Color.yellow, 0.2f);
            return false; // something blocking
        }
        return true;
    }

    void StartReturn()
    {
        state = State.Return;
        //if (debugDraw) Debug.Log("[E-Fix] StartReturn()");
    }

    void OnCatch()
    {
        agent.isStopped = true;
        //if (debugDraw) Debug.Log("[E-Fix] Player caught!");
        var gm = GameOverManager.Instance;
        if (gm != null) gm.ShowGameOver();
        StartReturn();
    }

    void TryRepairAgent()
    {
        if (NavMesh.SamplePosition(transform.position, out NavMeshHit hit, sampleRadius, NavMesh.AllAreas))
        {
            agent.Warp(hit.position);
            //if (debugDraw) Debug.Log($"[E-Fix] Warped to navmesh: {hit.position}");
        }
        else
        {
            //if (debugDraw) Debug.LogWarning("[E-Fix] No NavMesh near to warp");
        }
    }
}
