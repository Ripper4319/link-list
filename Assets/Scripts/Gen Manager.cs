using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ManagerGen : MonoBehaviour 
{

    public GameObject PauseMenu;
    public GameObject crosshairs;
    public bool Pausee = true;
    public playerController player;
    public Image healthBar;
    public bool isinhealtharea = true;

    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        PauseMenu.SetActive(false);
        Pausee = false;

        if (player != null)
        {
            StartCoroutine(DecreasePlayerHealth());
        }
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

        UpdateHealthBar();
    }

    private IEnumerator DecreasePlayerHealth()
    {
        while (true)
        {

            if (!isinhealtharea)
            {
                if (player != null)
                {
                    player.health--;
                }
                yield return new WaitForSeconds(1f);
            }

        }
    }

    private void UpdateHealthBar()
    {
        if (player != null && healthBar != null)
        {
            healthBar.fillAmount = Mathf.Clamp01(player.health / 20f);
        }
    }
}
