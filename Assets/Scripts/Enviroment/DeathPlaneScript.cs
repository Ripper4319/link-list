using UnityEngine;

public class DeathPlaneScript : MonoBehaviour
{
    public playerMovement player;
    public int deathNumber = 200;

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {

            player.health -= deathNumber;
        }
    }

}