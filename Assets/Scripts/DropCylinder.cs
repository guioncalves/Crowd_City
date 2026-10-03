using UnityEngine;

public class DropCylinder : MonoBehaviour
{
    public GameObject obstacle;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;

            if (Physics.Raycast(ray, out hitInfo))
            {
                Instantiate(
                    obstacle,
                    hitInfo.point,
                    obstacle.transform.rotation
                );

                Debug.Log("Cilindro colocado em: " + hitInfo.point);

                AIControl[] agents =
                    FindObjectsByType<AIControl>(
                        FindObjectsSortMode.None
                    );

                Debug.Log("NPCs encontrados: " + agents.Length);

                foreach (AIControl ai in agents)
                {
                    ai.DetectNewObstacle(hitInfo.point);
                }
            }
        }
    }
}