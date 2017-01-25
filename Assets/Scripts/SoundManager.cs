using UnityEngine;
using UnityEngine.SceneManagement;

public class SoundManager : MonoBehaviour {
    private static SoundManager instance = null;

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

        Application.targetFrameRate = 60;
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape) && SceneManager.GetActiveScene().buildIndex == 1)
        {
            Application.Quit();
        }
    }
}
