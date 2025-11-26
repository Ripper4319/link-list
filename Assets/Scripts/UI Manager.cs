using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class UIManager : MonoBehaviour
{
    [Header("Manager")]
    public ManagerGen mangen;

    [Header("Main Settings Buttons")]
    public GameObject load;
    public GameObject save;
    public GameObject settings;
    public GameObject quit;

    [Header("Settings Root")]
    public GameObject Settingsmenu;

    [Header("Settings Panels")]
    public GameObject displayPanel;
    public GameObject controlsPanel;
    public GameObject qualityPanel;
    public GameObject audioPanel;

    [Header("Dropdowns")]
    public TMP_Dropdown resolutionDropdown;
    public TMP_Dropdown screenModeDropdown;
    public TMP_Dropdown qualityDropdown;

    [Header("Sliders")]
    public UnityEngine.UI.Slider mouseSensitivity;
    public UnityEngine.UI.Slider masterVolume;
    public UnityEngine.UI.Slider sfxVolume;
    public UnityEngine.UI.Slider musicVolume;

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
    private GameObject lastSettingsPanel;

    void Start()
    {
        Settingsmenu.SetActive(false);

        streakRenderer = streakFX.GetComponent<Renderer>();
        Color c = streakRenderer.material.color;
        c.a = 0;
        streakRenderer.material.color = c;

        resolutions = Screen.resolutions;
        PopulateResolutionDropdown();

        screenModeDropdown.value = (int)Screen.fullScreenMode;
        qualityDropdown.value = QualitySettings.GetQualityLevel();

        StartCoroutine(RefreshDropdown(resolutionDropdown));


        mouseSensitivity.value = mangen.mouseSensitivity;
        masterVolume.value = mangen.masterVol;
        sfxVolume.value = mangen.sfxVol;
        musicVolume.value = mangen.musicVol;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            ApplyAllSettings();
        }

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
        FullScreenMode mode = Screen.fullScreenMode; 
        Screen.SetResolution(r.width, r.height, mode);
        Debug.Log($"Resolution applied: {r.width}x{r.height}, Fullscreen mode: {mode}");
    }

    public void ApplyAllSettings()
    {
        ApplyResolution();
        ApplyScreenMode();
        ApplyQuality();

        ApplyMouseSensitivity();

        ApplyMasterVolume();
        ApplySFXVolume();
        ApplyMusicVolume();

        Debug.Log("All settings applied from UI.");
    }

    public void ApplyScreenMode()
    {
        Screen.fullScreenMode = (FullScreenMode)screenModeDropdown.value;
    }

    public void ApplyQuality()
    {
        QualitySettings.SetQualityLevel(qualityDropdown.value);
    }

    private GameObject currentSettingsPanel;

    public void OpenSettings()
    {
        load.SetActive(false);
        save.SetActive(false);
        settings.SetActive(false);
        quit.SetActive(false);
        Settingsmenu.SetActive(true);

        ShowSettingsPanel(displayPanel);

        InitializeResolutionDropdown();
    }

    public void CloseSettings()
    {
        load.SetActive(true);
        save.SetActive(true);
        settings.SetActive(true);
        quit.SetActive(true);

        Settingsmenu.SetActive(false);

        if (lastSettingsPanel != null)
            lastSettingsPanel.SetActive(false);
    }

    public void ShowSettingsPanel(GameObject panel)
    {
        if (lastSettingsPanel != null)
            lastSettingsPanel.SetActive(false);

        lastSettingsPanel = panel;
        lastSettingsPanel.SetActive(true);
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

    public void OnResolutionChanged(int index)
    {
        if (index < 0 || index >= resolutions.Length) return;

        Resolution res = resolutions[index];
        bool fullscreen = Screen.fullScreenMode != FullScreenMode.Windowed;
        mangen.ApplyResolution(res.width, res.height, fullscreen);

        Debug.Log("Resolution dropdown changed: " + resolutionDropdown.value);
    }

    IEnumerator RefreshDropdown(TMP_Dropdown dropdown)
    {
        yield return null; 
        dropdown.RefreshShownValue();
    }



    public void OnScreenModeChanged()
    {
        int mode = screenModeDropdown.value;
        mangen.ApplyFullscreenMode((FullScreenMode)mode);
    }
    void PopulateResolutionDropdown()
    {
        resolutionDropdown.onValueChanged.RemoveAllListeners();
        resolutionDropdown.ClearOptions();

        List<TMPro.TMP_Dropdown.OptionData> optionData = new List<TMPro.TMP_Dropdown.OptionData>();
        int currentIndex = 0;

        for (int i = 0; i < resolutions.Length; i++)
        {
            string label = resolutions[i].width + " x " + resolutions[i].height + " @ " + resolutions[i].refreshRateRatio + "Hz";
            optionData.Add(new TMPro.TMP_Dropdown.OptionData(label));

            if (resolutions[i].width == Screen.currentResolution.width &&
                resolutions[i].height == Screen.currentResolution.height &&
                Mathf.Approximately((float)resolutions[i].refreshRateRatio.value,
                                    (float)Screen.currentResolution.refreshRateRatio.value))
            {
                currentIndex = i;
            }

        }

        resolutionDropdown.AddOptions(optionData);
        resolutionDropdown.value = currentIndex;
        resolutionDropdown.RefreshShownValue();

        Canvas.ForceUpdateCanvases();

        resolutionDropdown.onValueChanged.AddListener(OnResolutionChanged);
    }

    public void InitializeResolutionDropdown()
    {
        bool wasActive = Settingsmenu.activeSelf;
        Settingsmenu.SetActive(true);

        resolutionDropdown.ClearOptions();

        List<TMP_Dropdown.OptionData> options = new List<TMP_Dropdown.OptionData>();
        int currentIndex = 0;

        resolutions = Screen.resolutions;

        for (int i = 0; i < resolutions.Length; i++)
        {
            string label = resolutions[i].width + " x " + resolutions[i].height + " @ " + resolutions[i].refreshRateRatio + "Hz";
            options.Add(new TMP_Dropdown.OptionData(label));

            if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
            {
                currentIndex = i;
            }

        }


        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentIndex;

        resolutionDropdown.RefreshShownValue();

        Settingsmenu.SetActive(wasActive);
    }

}

