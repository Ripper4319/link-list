using UnityEngine;
using UnityEngine.UI;

public class ManagerGen : MonoBehaviour
{
    public GameObject PauseMenu;
    public GameObject crosshairs;
    public bool Pausee = true;
    public playerMovement player;
    public Image healthBar;
    public bool isinhealtharea = false;
    public bool coroutinecockblocker = true;

    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        PauseMenu.SetActive(false);
        Pausee = false;

        if (player != null && !isinhealtharea)
            StartCoroutine(DecreasePlayerHealth());
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
            Cursor.lockState = CursorLockMode.None;
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && Pausee)
        {
            Pausee = false;
            PauseMenu.SetActive(false);
            crosshairs.SetActive(true);
            Time.timeScale = 1;
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }

        if (player != null && healthBar != null)
            healthBar.fillAmount = Mathf.Clamp01(player.health / 20f);

        if (!isinhealtharea && coroutinecockblocker)
        {
            StartCoroutine(DecreasePlayerHealth());
            coroutinecockblocker = false;
        }
    }

    private System.Collections.IEnumerator DecreasePlayerHealth()
    {
        if (player != null)
            player.health--;

        yield return new WaitForSeconds(1f);
        coroutinecockblocker = true;
    }
}
