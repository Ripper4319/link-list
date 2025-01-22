using UnityEngine;

public class ManagerGen : MonoBehaviour 
{

    public GameObject PauseMenu;
    public GameObject crosshairs;
    public bool Pausee = true;

    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        PauseMenu.SetActive(false);
        Pausee = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !Pausee)
        {

            Pausee = true;
            PauseMenu.SetActive(true);
            crosshairs.SetActive(false);
            Time.timeScale = 0;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.Locked;
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && Pausee)
        {
            Pausee = false;
            PauseMenu.SetActive(false);
            crosshairs.SetActive(true);
            Time.timeScale = 1;
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.None;
        }
        
    }
}
