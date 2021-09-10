using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public bool isDone;
    public GameObject endScreen;
    public GameObject s0;
    public GameObject s1;
    public GameObject s2;
    public TextMeshProUGUI T_Perc;
    [Range(1, 2)]
    public float bumpScaleFactor;
    [Range(0, 1)]
    public float bumpSpeed;
    public float hasBurned;
    Flamable[] flammables;
    void Awake() { if (instance == null) { instance = this; DontDestroyOnLoad(gameObject); } else Destroy(gameObject); }
    void Start()
    {
        s0 = GameObject.Find("s0");
        s1 = GameObject.Find("s1");
        s2 = GameObject.Find("s2");
        s0.SetActive(false);
        s1.SetActive(false);
        s2.SetActive(false);
    }
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
        s0.SetActive(perc * 100 > 0 ? true : false);
        s1.SetActive(perc * 100 > 33 ? true : false);
        s2.SetActive(perc * 100 > 66 ? true : false);
    }
}