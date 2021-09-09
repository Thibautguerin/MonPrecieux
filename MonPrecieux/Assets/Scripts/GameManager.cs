using TMPro;
using UnityEngine;
using UnityEngine.UI;
class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public TextMeshProUGUI T_Perc;
    [Range(1, 2)]
    public float bumpScaleFactor;
    [Range(0, 1)]
    public float bumpSpeed;
    float hasBurned;
    Flamable[] flammables;
    void Awake() { if (instance == null) { instance = this; DontDestroyOnLoad(gameObject); } else Destroy(gameObject); }
    void Start()
    {
        flammables = FindObjectsOfType<Flamable>();
    }
    void Update()
    {
        // if (flammables.Length > 0)
        // {
        //     var hasBurned = 0;
        //     foreach (var i in flammables)
        //         if (i.hasBurned) hasBurned++;
        //     var perc = hasBurned / flammables.Length * 100;
        // }
        if (Input.GetKeyDown(KeyCode.C)) IncrementPerc();
        T_Perc.gameObject.transform.localScale = Vector2.Lerp(T_Perc.gameObject.transform.localScale, Vector2.one, bumpSpeed);
    }
    public void IncrementPerc()
    {
        hasBurned++;
        float perc = hasBurned / 10;
        if (perc <= 1)
        {
            T_Perc.SetText("{0:0}%", perc * 100);
            T_Perc.gameObject.transform.localScale *= bumpScaleFactor;
            T_Perc.color = new Color(T_Perc.color.r, T_Perc.color.g * (1 - perc), T_Perc.color.b);
        }
    }
}