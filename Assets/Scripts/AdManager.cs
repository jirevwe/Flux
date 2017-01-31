using System.Collections;
using UnityEngine;
using UnityEngine.Advertisements;

public class AdManager : MonoBehaviour {
    private static AdManager instance = null;

    public string appID = "1285424";
    public int turns = 0;

    public static AdManager Instance
    {
        get { return instance; }
    }

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            instance = this;
        }
        DontDestroyOnLoad(gameObject);
    }

    public void ShowAd(string zone = "")
    {
#if UNITY_EDITOR
        StartCoroutine(WaitForAd());
#endif

        if (string.Equals(zone, ""))
            zone = null;

        ShowOptions options = new ShowOptions();
        options.resultCallback = AdCallbackhandler;

        if (Advertisement.IsReady(zone))
            Advertisement.Show(zone, options);
    }

    void AdCallbackhandler(ShowResult result)
    {
        switch (result)
        {
            case ShowResult.Finished:
                print("Finished");
                break;
            case ShowResult.Skipped:
                print("Skipped");
                break;
            case ShowResult.Failed:
                print("Failed");
                break;
        }
    }

    IEnumerator WaitForAd()
    {
        float currentTimeScale = Time.timeScale;
        Time.timeScale = 0f;
        yield return null;

        while (Advertisement.isShowing)
            yield return null;

        Time.timeScale = currentTimeScale;
    }
}
