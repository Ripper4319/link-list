using UnityEngine;
using UnityEngine.AI;

public class NavDisabler : MonoBehaviour
{
    public bool on;

    public void DisableAllAgents()
    {
        GameObject[] agents = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject agent in agents)
        {
            NavMeshAgent navMeshAgent = agent.GetComponent<NavMeshAgent>();
            Rigidbody rb = agent.GetComponent<Rigidbody>();

            if (navMeshAgent != null)
            {
                navMeshAgent.enabled = false;
            }

            if (rb != null)
            {
                rb.useGravity = true;
            }

            on = false;
        }
    }

    public void EnableAllAgents()
    {
        GameObject[] agents = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject agent in agents)
        {
            NavMeshAgent navMeshAgent = agent.GetComponent<NavMeshAgent>();
            Rigidbody rb = agent.GetComponent<Rigidbody>();

            if (navMeshAgent != null)
            {
                navMeshAgent.enabled = true; 
            }

            if (rb != null)
            {
                rb.useGravity = false; 
            }
            on = true;
        }
    }
}
