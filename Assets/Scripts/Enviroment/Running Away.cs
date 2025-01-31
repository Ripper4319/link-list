using UnityEngine;

public class RunningAway : MonoBehaviour
{



    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {

        }
    }
}
