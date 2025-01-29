using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public long lastUpdated;
    public int playerHealth;
    public Vector3 playerPosition;
    public SerializableDictionary<string, bool> enemiesDefeated;

    public GameData()
    {
        this.playerHealth = 20;
        this.playerPosition = Vector3.zero;
        this.enemiesDefeated = new SerializableDictionary<string, bool>();
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
