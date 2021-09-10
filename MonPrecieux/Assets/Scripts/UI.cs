using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
class UI : MonoBehaviour
{
    public static UI instance;
    [Header("State")]
    public bool isPaused;
    public bool isMenued;
    public bool isSettinged;
    public bool isCredited;
    public bool isHTP;
    public bool showHTP;
    [Header("Layers")]
    public GameObject L_InGame;
    public GameObject L_Pause;
    public GameObject L_Menu;
    public GameObject L_Settings;
    public GameObject L_Credits;
    public GameObject L_HTP;
    [Header("Toggles")]
    public Toggle Toggle_Global;
    public Toggle Toggle_Music;
    public Toggle Toggle_SFX;
    public Toggle Toggle_HTP;
    [Header("Sliders")]
    public Slider Slider_Global;
    public Slider Slider_Music;
    public Slider Slider_SFX;
    void Awake() { if (instance == null) { instance = this; DontDestroyOnLoad(gameObject); } else Destroy(gameObject); }
    void Update()
    {
        Time.timeScale = isPaused ? 0 : 1;
        L_InGame.SetActive(!isPaused && !isMenued && !isHTP);
        L_Pause.SetActive(isPaused && !isSettinged && !isCredited);
        L_Menu.SetActive(isMenued && !isSettinged && !isCredited && !isHTP);
        L_Settings.SetActive(isSettinged);
        L_Credits.SetActive(isCredited);
        L_HTP.SetActive(isHTP);
        if (!isMenued && !isSettinged && !isCredited && !isHTP && Input.GetButtonDown("Cancel")) SwitchIsPaused();
        if (isSettinged && Input.GetButtonDown("Cancel")) SwitchIsSettinged();
        if (isCredited && Input.GetButtonDown("Cancel")) SwitchIsCredited();
        if (isHTP && Input.GetButtonDown("Cancel")) SwitchIsHTP();
        Toggle_Global.onValueChanged.AddListener(delegate { AudioManager.instance.isGlobalOn = Toggle_Global.isOn; });
        Toggle_Music.onValueChanged.AddListener(delegate { AudioManager.instance.isMusicOn = Toggle_Music.isOn; });
        Toggle_SFX.onValueChanged.AddListener(delegate { AudioManager.instance.isSfxOn = Toggle_SFX.isOn; });
        Toggle_HTP.onValueChanged.AddListener(delegate { showHTP = Toggle_HTP.isOn; });
        Toggle_Global.isOn = AudioManager.instance.isGlobalOn;
        Toggle_Music.isOn = AudioManager.instance.isMusicOn;
        Toggle_SFX.isOn = AudioManager.instance.isSfxOn;
        Toggle_HTP.isOn = showHTP;
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
    public void SwitchIsHTP() { isHTP ^= true; }
    public void Retry()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        isPaused = false;
        GameManager.instance.isDone = false;
    }
    public void Play()
    {
        if (showHTP) isHTP = true;
        else StartGame();
    }
    public void StartGame()
    {
        isMenued = false;
        isHTP = false;
        showHTP = false;
        StartLevel("1");
    }
    public void Menu()
    {
        SceneManager.LoadScene("Menu");
        isMenued = true;
        isPaused = false;
    }
    public void Quit() { Application.Quit(); }
    public void StartLevel(string name)
    {
        SceneManager.LoadScene(name);
        GameManager.instance.isDone = false;
        GameManager.instance.hasBurned = 0;
        GameManager.instance.T_Perc.SetText("{0:0}%", 0);
    }
}