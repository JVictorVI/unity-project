using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class EnemyChase : MonoBehaviour
{
    public Transform player;                 // arraste o Player aqui no Inspector
    public float catchDistance = 1.2f;       // distância para "pegar"
    public float updateInterval = 0.18f;     // frequência de atualização do destino
    public bool useLineOfSight = false;      // se true, só pega se enxergar (raycast)
    public LayerMask obstacleMask;           // defina obstáculos (se usar LOS)
    NavMeshAgent agent;
    bool caught = false;
    float sqrCatchDistance;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        if (player == null)
        {
            GameObject p = GameObject.FindGameObjectWithTag("Player");
            if (p != null) player = p.transform;
        }
        sqrCatchDistance = catchDistance * catchDistance;
        StartCoroutine(UpdateDestinationLoop());
    }

    IEnumerator UpdateDestinationLoop()
    {
        while (!caught)
        {
            if (player != null && agent.isOnNavMesh)
            {
                agent.SetDestination(player.position);
            }
            yield return new WaitForSeconds(updateInterval);
        }
    }

    void Update()
    {
        if (caught || player == null) return;

        float sqrDist = (player.position - transform.position).sqrMagnitude;
        if (sqrDist <= sqrCatchDistance)
        {
            if (useLineOfSight)
            {
                Vector3 eyePos = transform.position + Vector3.up * 1.2f;
                Vector3 dir = (player.position + Vector3.up * 1f) - eyePos;
                if (!Physics.Raycast(eyePos, dir.normalized, Mathf.Sqrt(sqrDist), obstacleMask))
                {
                    OnCatch();
                }
            }
            else
            {
                OnCatch();
            }
        }
    }

    void OnCatch()
    {
        caught = true;
        if (agent != null) agent.isStopped = true;
        GameOverManager.Instance.ShowGameOver();
    }

    public void ResetChase()
    {
        caught = false;
        if (agent != null) agent.isStopped = false;
        StartCoroutine(UpdateDestinationLoop());
    }
}
