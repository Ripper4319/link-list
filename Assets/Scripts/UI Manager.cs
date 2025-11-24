using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class UIManager : MonoBehaviour
{
    [Header("Manager")]
    public ManagerGen mangen;

    [Header("Dropdowns")]
    public Dropdown resolutionDropdown;
    public Dropdown screenModeDropdown;
    public Dropdown qualityDropdown;

    [Header("Sliders")]
    public UnityEngine.UI.Slider mouseSensitivity;
    public UnityEngine.UI.Slider masterVolume;
    public UnityEngine.UI.Slider sfxVolume;
    public UnityEngine.UI.Slider musicVolume;

    [Header("Buttons / Panels")]
    public GameObject load;
    public GameObject save;
    public GameObject settings;
    public GameObject quit;
    public GameObject Settingsmenu;

    [Header("Damage UI")]
    public UnityEngine.UI.Image damageImage;

    [Header("Movement / Camera")]
    public Rigidbody rb;
    public Camera cam;
    public float baseFOV = 70f;
    public float maxFOV = 95f;
    public float velocityForMax = 20f;
    public float fovSmooth = 8f;

    [Header("Streak FX")]
    public GameObject streakFX;
    public float streakSpeed = 15f;
    public float streakFade = 6f;

    [Header("Internals")]
    float streakAlpha;
    Renderer streakRenderer;
    Resolution[] resolutions;


    void Start()
    {
        Settingsmenu.SetActive(false);

        streakRenderer = streakFX.GetComponent<Renderer>();
        Color c = streakRenderer.material.color;
        c.a = 0;
        streakRenderer.material.color = c;

        resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();
        foreach (var r in resolutions) resolutionDropdown.options.Add(new Dropdown.OptionData(r.width + " x " + r.height));
        resolutionDropdown.value = resolutions.Length - 1;

        screenModeDropdown.value = (int)Screen.fullScreenMode;
        qualityDropdown.value = QualitySettings.GetQualityLevel();

        mouseSensitivity.value = mangen.mouseSensitivity;
        masterVolume.value = mangen.masterVol;
        sfxVolume.value = mangen.sfxVol;
        musicVolume.value = mangen.musicVol;
    }

    void Update()
    {
        float tHealth = mangen.isinhealtharea ? 0f : 1f;
        float sp = mangen.isinhealtharea ? 7f : 2f;
        Color d = damageImage.color;
        d.a = Mathf.Lerp(d.a, tHealth, Time.deltaTime * sp);
        damageImage.color = d;

        float vel = rb.linearVelocity.magnitude;
        float t = Mathf.Clamp01(vel / velocityForMax);
        float targetFOV = Mathf.Lerp(baseFOV, maxFOV, t);
        cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, targetFOV, Time.deltaTime * fovSmooth);

        float targetAlpha = vel > streakSpeed ? 1f : 0f;
        streakAlpha = Mathf.Lerp(streakAlpha, targetAlpha, Time.deltaTime * streakFade);

        Color sc = streakRenderer.material.color;
        sc.a = streakAlpha;
        streakRenderer.material.color = sc;
    }

    public void ApplyResolution()
    {
        Resolution r = resolutions[resolutionDropdown.value];
        Screen.SetResolution(r.width, r.height, Screen.fullScreenMode);
    }

    public void ApplyScreenMode()
    {
        Screen.fullScreenMode = (FullScreenMode)screenModeDropdown.value;
    }

    public void ApplyQuality()
    {
        QualitySettings.SetQualityLevel(qualityDropdown.value);
    }

    public void OpenSettings()
    {
        load.SetActive(false);
        save.SetActive(false);
        settings.SetActive(false);
        quit.SetActive(false);
        Settingsmenu.SetActive(true);
    }

    public void CloseSettings()
    {
        load.SetActive(true);
        save.SetActive(true);
        settings.SetActive(true);
        quit.SetActive(true);
        Settingsmenu.SetActive(false);
    }

    public void ApplyMouseSensitivity()
    {
        mangen.mouseSensitivity = mouseSensitivity.value;
    }

    public void ApplyMasterVolume()
    {
        mangen.masterVol = masterVolume.value;
    }

    public void ApplySFXVolume()
    {
        mangen.sfxVol = sfxVolume.value;
    }

    public void ApplyMusicVolume()
    {
        mangen.musicVol = musicVolume.value;
    }
}

