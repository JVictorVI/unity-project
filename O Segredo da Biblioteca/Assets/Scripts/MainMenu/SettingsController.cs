using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using TMPro;

public class SettingsController : MonoBehaviour
{
    [Header("Áudio")]
    public AudioMixer audioMixer;
    public Slider volumeSlider;

    [Header("Gráficos")]
    public TMP_Dropdown resolutionDropdown;
    public TMP_Dropdown qualityDropdown;
    public Toggle fullscreenToggle;
    public Toggle vsyncToggle; // <- novo toggle de V-Sync

    private Resolution[] resolutions;

    void Start()
    {
        // --- Configura lista de resoluções ---
        resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();

        var options = new System.Collections.Generic.List<string>();
        int currentResolutionIndex = 0;

        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);

            if (resolutions[i].width == Screen.currentResolution.width &&
                resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();

        // --- Popula opções de qualidade gráfica ---
        qualityDropdown.ClearOptions();
        var qualities = new System.Collections.Generic.List<string>(QualitySettings.names);
        qualityDropdown.AddOptions(qualities);

        // --- Carrega preferências salvas ---
        float savedVolume = PlayerPrefs.GetFloat("Volume", 0.75f);
        int savedQuality = PlayerPrefs.GetInt("QualityLevel", QualitySettings.GetQualityLevel());
        bool savedFullscreen = PlayerPrefs.GetInt("Fullscreen", Screen.fullScreen ? 1 : 0) == 1;
        bool savedVSync = PlayerPrefs.GetInt("VSync", 1) == 1;

        // --- Aplica preferências ---
        volumeSlider.value = savedVolume;
        SetVolume(savedVolume);

        qualityDropdown.value = savedQuality;
        qualityDropdown.RefreshShownValue();
        SetQuality(savedQuality);

        fullscreenToggle.isOn = savedFullscreen;
        SetFullscreen(savedFullscreen);

        vsyncToggle.isOn = savedVSync;
        SetVSync(savedVSync);
    }

    // --- Volume ---
    public void SetVolume(float volume)
    {
        if (volume <= 0.0001f)
            volume = 0.0001f;

        audioMixer.SetFloat("Volume", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("Volume", volume);
    }

    // --- Resolução ---
    public void SetResolution(int index)
    {
        Resolution resolution = resolutions[index];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
        PlayerPrefs.SetInt("ResolutionIndex", index);
    }

    // --- Tela cheia ---
    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
        PlayerPrefs.SetInt("Fullscreen", isFullscreen ? 1 : 0);
    }

    // --- Qualidade gráfica global ---
    public void SetQuality(int index)
    {
        QualitySettings.SetQualityLevel(index);
        PlayerPrefs.SetInt("QualityLevel", index);
        PlayerPrefs.Save();

        Debug.Log($"[Settings] Qualidade: {QualitySettings.names[index]}");
    }

    // --- V-Sync ---
    public void SetVSync(bool isOn)
    {
        QualitySettings.vSyncCount = isOn ? 1 : 0;
        PlayerPrefs.SetInt("VSync", isOn ? 1 : 0);
        PlayerPrefs.Save();

        Debug.Log($"[Settings] V-Sync {(isOn ? "ativado" : "desativado")}");
    }

    // --- Aplicar e voltar ---
    public void SaveSettings()
    {
        PlayerPrefs.Save();
    }

    public void BackToMainMenu()
    {
        gameObject.SetActive(false);
    }

    public void GetPreferences()
    {
        // --- Carrega preferências salvas ---
        float savedVolume = PlayerPrefs.GetFloat("Volume", 0.75f);
        int savedQuality = PlayerPrefs.GetInt("QualityLevel", QualitySettings.GetQualityLevel());
        bool savedFullscreen = PlayerPrefs.GetInt("Fullscreen", Screen.fullScreen ? 1 : 0) == 1;
        bool savedVSync = PlayerPrefs.GetInt("VSync", 1) == 1;

        // --- Aplica preferências ---
        volumeSlider.value = savedVolume;
        SetVolume(savedVolume);

        qualityDropdown.value = savedQuality;
        qualityDropdown.RefreshShownValue();
        SetQuality(savedQuality);

        fullscreenToggle.isOn = savedFullscreen;
        SetFullscreen(savedFullscreen);

        vsyncToggle.isOn = savedVSync;
        SetVSync(savedVSync);
    }
}
