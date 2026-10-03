using UnityEngine;
using UnityEngine.AI;

public class AIControl : MonoBehaviour
{
    public GameObject[] goalLocations;

    NavMeshAgent agent;
    Animator anim;

    public float normalSpeed = 3.5f;
    public float fleeSpeed = 10f;

    public float detectionRadius = 20f;
    public float fleeRadius = 10f;

    bool isFleeing = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();

        goalLocations = GameObject.FindGameObjectsWithTag("goal");

        // Todos começam à mesma velocidade
        agent.speed = normalSpeed;
        agent.angularSpeed = 120f;

        // Animação de caminhada
        anim.SetFloat("SpeedMult", 1f);
        anim.SetFloat("Woffset", Random.Range(0f, 1f));
        anim.SetTrigger("isWalking");

        SetRandomGoal();

        Debug.Log(
            gameObject.name +
            " | Velocidade normal: " +
            agent.speed
        );
    }

    void SetRandomGoal()
    {
        if (goalLocations.Length == 0)
            return;

        int i = Random.Range(0, goalLocations.Length);

        agent.SetDestination(
            goalLocations[i].transform.position
        );
    }

    public void DetectNewObstacle(Vector3 obstaclePosition)
    {
        float distance = Vector3.Distance(
            transform.position,
            obstaclePosition
        );

        if (distance <= detectionRadius)
        {
            // Direção para LONGE do cilindro
            Vector3 fleeDirection =
                (transform.position - obstaclePosition).normalized;

            Vector3 desiredPosition =
                transform.position + fleeDirection * fleeRadius;

            NavMeshHit hit;

            if (NavMesh.SamplePosition(
                desiredPosition,
                out hit,
                fleeRadius,
                NavMesh.AllAreas))
            {
                isFleeing = true;

                // FLEE MODE
                agent.speed = fleeSpeed;
                agent.angularSpeed = 500f;

                anim.SetFloat("SpeedMult", 2f);
                anim.SetTrigger("isRunning");

                agent.SetDestination(hit.position);

                Debug.Log(
                    gameObject.name +
                    " entrou em FLEE MODE | Speed: " +
                    agent.speed
                );
            }
        }
    }

    void Update()
    {
        if (agent.pathPending)
            return;

        if (agent.remainingDistance <= 1f)
        {
            if (isFleeing)
            {
                // Acabou o pânico
                isFleeing = false;

                agent.speed = normalSpeed;
                agent.angularSpeed = 120f;

                // Volta à animação normal
                anim.SetFloat("SpeedMult", 1f);
                anim.SetTrigger("isWalking");

                Debug.Log(
                    gameObject.name +
                    " terminou FLEE | Speed: " +
                    agent.speed
                );
            }

            SetRandomGoal();
        }
    }
}