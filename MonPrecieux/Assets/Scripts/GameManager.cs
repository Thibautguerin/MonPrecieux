using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public bool isDone;
    public GameObject endScreen;
    public TextMeshProUGUI T_Perc;
    [Range(1, 2)]
    public float bumpScaleFactor;
    [Range(0, 1)]
    public float bumpSpeed;
    public float hasBurned;
    Flamable[] flammables;
    void Awake() { if (instance == null) { instance = this; DontDestroyOnLoad(gameObject); } else Destroy(gameObject); }
    void Update()
    {
        flammables = FindObjectsOfType<Flamable>();
        T_Perc.gameObject.transform.localScale = Vector2.Lerp(T_Perc.gameObject.transform.localScale, Vector2.one, bumpSpeed);
        endScreen.SetActive(isDone);
    }
    public void IncPerc()
    {
        hasBurned++;
        float perc = hasBurned / flammables.Length;
        if (perc <= 1)
        {
            T_Perc.SetText("{0:0}%", perc * 100);
            T_Perc.gameObject.transform.localScale *= bumpScaleFactor;
            T_Perc.color = new Color(T_Perc.color.r, T_Perc.color.g * (1 - perc), T_Perc.color.b);
        }
    }
}