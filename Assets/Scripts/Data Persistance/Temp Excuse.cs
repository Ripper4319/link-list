using UnityEngine;

public class TempExcuse : MonoBehaviour
{
    public DataPersistenceManager datP;


    public void start()
    {
        datP.LoadGame();
    }
}
