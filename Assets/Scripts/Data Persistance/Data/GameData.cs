using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public long lastUpdated;
    public int playerHealth;
    public Vector3 playerPosition;
    public Dictionary<string, bool> enemiesDefeated = new Dictionary<string, bool>();
    public Dictionary<string, Vector3> enemyPositions = new Dictionary<string, Vector3>();

    public GameData()
    {
        this.playerHealth = 20;
        this.playerPosition = Vector3.zero;
        enemiesDefeated = new Dictionary<string, bool>();
        enemyPositions = new Dictionary<string, Vector3>();

    }

    public int GetPercentageEnemiesDefeated()
    {
        int totalDefeated = 0;
        foreach (bool defeated in enemiesDefeated.Values)
        {
            if (defeated)
            {
                totalDefeated++;
            }
        }

        int percentageCompleted = -1;
        if (enemiesDefeated.Count != 0)
        {
            percentageCompleted = (totalDefeated * 100 / enemiesDefeated.Count);
        }
        return percentageCompleted;
    }
}