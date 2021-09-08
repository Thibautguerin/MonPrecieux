using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
class UI : MonoBehaviour
{
    [Header("State")]
    public bool isPaused;
    public bool isMenued;
    public bool isSettinged;
    public bool isCredited;
    [Header("Layers")]
    public GameObject L_InGame;
    public GameObject L_Pause;
    public GameObject L_Menu;
    public GameObject L_Settings;
    public GameObject L_Credits;
    [Header("Toggles")]
    public Toggle Toggle_Global;
    public Toggle Toggle_Music;
    public Toggle Toggle_SFX;
    [Header("Sliders")]
    public Slider Slider_Global;
    public Slider Slider_Music;
    public Slider Slider_SFX;
    void Update()
    {
        Time.timeScale = isPaused ? 0 : 1;
        L_InGame.SetActive(!isPaused && !isMenued);
        L_Pause.SetActive(isPaused && !isSettinged && !isCredited);
        L_Menu.SetActive(isMenued && !isSettinged && !isCredited);
        L_Settings.SetActive(isSettinged);
        L_Credits.SetActive(isCredited);
        if (!isMenued && !isSettinged && !isCredited && Input.GetKeyDown(KeyCode.Escape)) SwitchIsPaused();
        if (isSettinged && Input.GetKeyDown(KeyCode.Escape)) SwitchIsSettinged();
        if (isCredited && Input.GetKeyDown(KeyCode.Escape)) SwitchIsCredited();
        Toggle_Global.onValueChanged.AddListener(delegate { AudioManager.instance.isGlobalOn = Toggle_Global.isOn; });
        Toggle_Music.onValueChanged.AddListener(delegate { AudioManager.instance.isMusicOn = Toggle_Music.isOn; });
        Toggle_SFX.onValueChanged.AddListener(delegate { AudioManager.instance.isSfxOn = Toggle_SFX.isOn; });
        Toggle_Global.isOn = AudioManager.instance.isGlobalOn;
        Toggle_Music.isOn = AudioManager.instance.isMusicOn;
        Toggle_SFX.isOn = AudioManager.instance.isSfxOn;
        Slider_Global.onValueChanged.AddListener(delegate { AudioManager.instance.globalVolume = Slider_Global.value; });
        Slider_Music.onValueChanged.AddListener(delegate { AudioManager.instance.musicVolume = Slider_Music.value; });
        Slider_SFX.onValueChanged.AddListener(delegate { AudioManager.instance.sfxVolume = Slider_SFX.value; });
        Slider_Global.value = AudioManager.instance.globalVolume;
        Slider_Music.value = AudioManager.instance.musicVolume;
        Slider_SFX.value = AudioManager.instance.sfxVolume;
        Slider_Global.interactable = AudioManager.instance.isGlobalOn;
        Slider_Music.interactable = AudioManager.instance.isMusicOn;
        Slider_SFX.interactable = AudioManager.instance.isSfxOn;
    }
    public void SwitchIsPaused() { isPaused ^= true; }
    public void SwitchIsSettinged() { isSettinged ^= true; }
    public void SwitchIsCredited() { isCredited ^= true; }
    public void Retry()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        isPaused = false;
    }
    public void Play()
    {
        SceneManager.LoadScene("Game");
        isMenued = false;
    }
    public void Menu()
    {
        SceneManager.LoadScene("Menu");
        isMenued = true;
        isPaused = false;
    }
    public void Quit() { Application.Quit(); }
}