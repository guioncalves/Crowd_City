using UnityEngine;
using UnityEngine.AI;

public class AIControl : MonoBehaviour
{
    public GameObject[] goalLocations;

    private NavMeshAgent agent;
    private Animator anim;

    public float normalSpeed = 1f;
    public float fleeSpeed = 10f;

    public float detectionRadius = 20f;
    public float fleeRadius = 10f;

    private bool isFleeing = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();

        goalLocations = GameObject.FindGameObjectsWithTag("goal");

        // Movimento normal
        agent.speed = normalSpeed;
        agent.angularSpeed = 120f;

        // Animação normal
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

        Debug.Log(
            gameObject.name +
            " | Distância ao cilindro: " +
            distance
        );

        // Está suficientemente perto do cilindro?
        if (distance <= detectionRadius)
        {
            Debug.Log(
                gameObject.name +
                " ENTROU EM FLEE MODE!"
            );

            // Direção PARA LONGE do cilindro
            Vector3 fleeDirection =
                (transform.position - obstaclePosition).normalized;

            Vector3 desiredPosition =
                transform.position +
                fleeDirection * fleeRadius;

            NavMeshHit hit;

            if (NavMesh.SamplePosition(
                desiredPosition,
                out hit,
                fleeRadius,
                NavMesh.AllAreas))
            {
                isFleeing = true;

                // Acelera
                agent.speed = fleeSpeed;
                agent.angularSpeed = 500f;

                // Muda Walking -> Running
                anim.ResetTrigger("isWalking");
                anim.SetFloat("SpeedMult", 1f);
                anim.SetTrigger("isRunning");

                // Foge
                agent.SetDestination(hit.position);

                Debug.Log(
                    gameObject.name +
                    " A CORRER | Speed = " +
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
                // Acabou de fugir
                isFleeing = false;

                // Volta à velocidade normal
                agent.speed = normalSpeed;
                agent.angularSpeed = 120f;

                // Running -> Walking
                anim.ResetTrigger("isRunning");
                anim.SetFloat("SpeedMult", 1f);
                anim.SetTrigger("isWalking");

                Debug.Log(
                    gameObject.name +
                    " terminou Flee/Speed Mode = " +
                    agent.speed
                );
            }

            SetRandomGoal();
        }
    }
}