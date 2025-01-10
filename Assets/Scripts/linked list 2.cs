using UnityEngine;
using UnityEngine.AI;

public class EnemyBehavior : MonoBehaviour
{
    private Color color;
    private float speed;
    private float health;

    private NavMeshAgent navMeshAgent;
    private Transform player;
    private Renderer enemyRenderer;

    private void Start()
    {
        enemyRenderer = GetComponent<Renderer>();
        navMeshAgent = GetComponent<NavMeshAgent>();

        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            player = playerObject.transform;
        }

        if (enemyRenderer != null)
        {
            enemyRenderer.material.color = color;
        }
    }

    private void Update()
    {
        if (player != null)
        {
            navMeshAgent.SetDestination(player.position);
        }
    }

    public void SetAttributes(Color newColor, float newSpeed, float newHealth)
    {
        color = newColor;
        speed = newSpeed;
        health = newHealth;

        if (navMeshAgent != null)
        {
            navMeshAgent.speed = speed;
        }

        if (enemyRenderer != null)
        {
            enemyRenderer.material.color = color;
        }

        Debug.Log($"Enemy Attributes Updated: Color = {color}, Speed = {speed}, Health = {health}");
    }
}

