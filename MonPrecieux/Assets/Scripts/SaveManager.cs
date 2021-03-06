using UnityEngine;
class SaveManager : MonoBehaviour
{
    public static SaveManager instance;
    void Start()
    {
        if (instance == null) { instance = this; DontDestroyOnLoad(gameObject); }
        else { Destroy(gameObject); return; }
        Load();
    }
    void OnApplicationQuit() { Save(); }
    void Load()
    {
        AudioManager.instance.isGlobalOn = PlayerPrefs.GetInt("isGlobalOn", 1) == 1 ? true : false;
        AudioManager.instance.isMusicOn = PlayerPrefs.GetInt("isMusicOn", 1) == 1 ? true : false;
        AudioManager.instance.isSfxOn = PlayerPrefs.GetInt("isSfxOn", 1) == 1 ? true : false;
        AudioManager.instance.globalVolume = PlayerPrefs.GetFloat("globalVolume", 1);
        AudioManager.instance.musicVolume = PlayerPrefs.GetFloat("musicVolume", 1);
        AudioManager.instance.sfxVolume = PlayerPrefs.GetFloat("sfxVolume", 1);
        UI.instance.showHTP = PlayerPrefs.GetInt("showHTP", 1) == 1 ? true : false;
    }
    void Save()
    {
        PlayerPrefs.SetInt("isGlobalOn", AudioManager.instance.isGlobalOn ? 1 : 0);
        PlayerPrefs.SetInt("isMusicOn", AudioManager.instance.isMusicOn ? 1 : 0);
        PlayerPrefs.SetInt("isSfxOn", AudioManager.instance.isSfxOn ? 1 : 0);
        PlayerPrefs.SetFloat("globalVolume", AudioManager.instance.globalVolume);
        PlayerPrefs.SetFloat("musicVolume", AudioManager.instance.musicVolume);
        PlayerPrefs.SetFloat("sfxVolume", AudioManager.instance.sfxVolume);
        PlayerPrefs.SetInt("showHTP", UI.instance.showHTP ? 1 : 0);
    }
}