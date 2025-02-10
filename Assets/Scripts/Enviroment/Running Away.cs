using UnityEngine;
using UnityEngine.SceneManagement;

public class RunningAway : MonoBehaviour
{
    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SceneManager.LoadScene(1);
        }
    }
}