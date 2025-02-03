using UnityEngine;
using UnityEngine.AI;

public class NavDisabler : MonoBehaviour
{
    public void DisableAllAgents()
    {
        GameObject[] agents = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject agent in agents)
        {
            NavMeshAgent navMeshAgent = agent.GetComponent<NavMeshAgent>();
            if (navMeshAgent != null)
            {
                navMeshAgent.enabled = false; 
            }
        }
    }

    public void EnableAllAgents()
    {
        GameObject[] agents = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject agent in agents)
        {
            NavMeshAgent navMeshAgent = agent.GetComponent<NavMeshAgent>();
            if (navMeshAgent != null)
            {
                navMeshAgent.enabled = true; 
            }
        }
    }

}
