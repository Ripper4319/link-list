using UnityEngine;

public class ManagerGen : MonoBehaviour 
{

    public GameObject PauseMenu;

    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        PauseMenu.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.J) && !PauseMenu)
        {
            PauseMenu.SetActive(true);
            Debug.Log("fuc");
        }
        
        if (Input.GetKey(KeyCode.J) && PauseMenu)
        {
            PauseMenu.SetActive(false);
        }
    }
}
