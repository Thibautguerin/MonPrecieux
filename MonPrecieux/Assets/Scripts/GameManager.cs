using UnityEngine;
using UnityEngine.UI;
class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public Text T_Perc;
    int hasBurned;
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
    }
    public void IncrementPerc()
    {
        hasBurned++;
        var perc = hasBurned / flammables.Length * 100;
        T_Perc.text = perc + "%";
    }
}