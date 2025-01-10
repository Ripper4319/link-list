using System.Collections.Generic;
using UnityEngine;

public class EnemyLinkedListManager : MonoBehaviour
{
    private LinkedList<EnemyAttributes> enemyAttributesList = new LinkedList<EnemyAttributes>();
    public EnemyBehavior enemyBehavior;

    private void Start()
    {
        AddEnemyAttributes();
        PrintEnemyList();
        ApplyFirstAttributes();
    }

    private void AddEnemyAttributes()
    {
        enemyAttributesList.AddLast(new EnemyAttributes(Color.red, 3.5f, 100));
        enemyAttributesList.AddLast(new EnemyAttributes(Color.green, 2.0f, 150));
        enemyAttributesList.AddLast(new EnemyAttributes(Color.blue, 1.5f, 200));
    }

    private void PrintEnemyList()
    {
        Debug.Log("Enemy List Attributes:");
        foreach (var attributes in enemyAttributesList)
        {
            Debug.Log($"Color: {attributes.Color}, Speed: {attributes.WalkSpeed}, Health: {attributes.Health}");
        }
    }

    private void ApplyFirstAttributes()
    {
        if (enemyAttributesList.Count == 0)
        {
            Debug.LogWarning("No attributes in the list to apply!");
            return;
        }

        if (enemyBehavior == null)
        {
            Debug.LogWarning("EnemyBehavior script not assigned!");
            return;
        }

        var firstNode = enemyAttributesList.First.Value;
        enemyBehavior.SetAttributes(firstNode.Color, firstNode.WalkSpeed, firstNode.Health);
    }
}

public class EnemyAttributes
{
    public Color Color { get; }
    public float WalkSpeed { get; }
    public float Health { get; }

    public EnemyAttributes(Color color, float walkSpeed, float health)
    {
        Color = color;
        WalkSpeed = walkSpeed;
        Health = health;
    }
}
