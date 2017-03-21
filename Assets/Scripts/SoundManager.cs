//using admob;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SoundManager : MonoBehaviour {
    private static SoundManager instance = null;
    
    void Start()
    {
        //Admob.Instance().initAdmob("ca-app-pub-4072806619028958/3995380022", "ca-app-pub-4072806619028958/3679152429");
    }

    public static SoundManager Instance
    {
        get { return instance; }
    }

    void Awake()
    {
        if (instance != null && instance != this) {
            Destroy(gameObject);
            return;
        } else {
            instance = this;
        }
        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        if ((Input.GetKeyUp(KeyCode.Escape) || Input.GetKeyUp(KeyCode.E)) && SceneManager.GetActiveScene().buildIndex == 1)
        {
            Application.Quit();
        }
    }
}
