using UnityEngine;

public class DropCylinder : MonoBehaviour
{
    public GameObject obstacle;
    GameObject[] agents;

    void Start()
    {
        agents = GameObject.FindGameObjectsWithTag("agent");

        Debug.Log("NPCs encontrados: " + agents.Length);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray =
                Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit hitInfo;

            if (Physics.Raycast(ray, out hitInfo))
            {
                Instantiate(
                    obstacle,
                    hitInfo.point,
                    obstacle.transform.rotation
                );

                Debug.Log(
                    "Cilindro colocado em: " +
                    hitInfo.point
                );

                foreach (GameObject a in agents)
                {
                    AIControl ai = a.GetComponent<AIControl>();

                    if (ai != null)
                    {
                        ai.DetectNewObstacle(hitInfo.point);
                    }
                }
            }
        }
    }
}