using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Advertisements;
using System;
using Firebase.Database;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {
    private static GameManager instance = null;

    public string appID = "1285424";
    public int turns = 0;
    public PlayerData currentPlayer;
    public List<LevelScore> currentPlayerScores = new List<LevelScore>();

    public static GameManager Instance
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

    public static string GetUniqueID()
    {
        string key = "ID";

        var random = new System.Random();
        DateTime epochStart = new DateTime(1970, 1, 1, 8, 0, 0, DateTimeKind.Utc);
        double timestamp = (DateTime.UtcNow - epochStart).TotalSeconds;

        string uniqueID = string.Format("{0:X}", Convert.ToInt32(timestamp))                //Time
                + "-" + string.Format("{0:X}", Convert.ToInt32(Time.time * 1000000))        //Time in game
                + "-" + string.Format("{0:X}", random.Next(1000000000));                //random number

        if (PlayerPrefs.HasKey(key))
        {
            uniqueID = PlayerPrefs.GetString(key);
        }
        else
        {
            Debug.Log("Generated Unique ID: " + uniqueID);
            PlayerPrefs.SetString(key, uniqueID);
            PlayerPrefs.Save();
        }

        return uniqueID;
    }

    public void ShowAd(string zone = "")
    {
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

    public void FinishLevel()
    {
        var reference = FirebaseDatabase.DefaultInstance.RootReference;

        LevelScore newScore = new LevelScore { level_number = SceneManager.GetActiveScene().name.Replace(".", ""),
                player_uuid = currentPlayer.uuid,
                score = Controller.Instance.levelTime };
        LevelScore stroredScore = currentPlayerScores.Find(n => n.level_number == newScore.level_number);

        if (stroredScore == null || stroredScore.score > newScore.score) {
            //update global database
            reference.Child("/scores/").Child(newScore.level_number).Child(newScore.player_uuid).UpdateChildrenAsync(newScore.ToDictionary()).ContinueWith((task) =>
            {
                if (SceneManager.GetActiveScene().buildIndex == SceneManager.sceneCountInBuildSettings - 1)
                {
                    SceneManager.LoadScene("Splash");
                }
                else
                {
#if UNITY_ANDROID && !UNITY_EDITOR
                turns += 1;
                if (turns % 10 == 0)
                {
                    ShowAd("video");
                }
#endif
                    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
                }

            });
        }
        else
        {
            if (SceneManager.GetActiveScene().buildIndex == SceneManager.sceneCountInBuildSettings - 1)
            {
                SceneManager.LoadScene("Splash");
            }
            else
            {
#if UNITY_ANDROID && !UNITY_EDITOR
                turns += 1;
                if (turns % 10 == 0)
                {
                    ShowAd("video");
                }
#endif
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            }
        }
    }

    private void OnApplicationPause(bool pause)
    {
        PlayerPrefs.SetString(PlayerPrefSrings.player_data, JsonUtility.ToJson(currentPlayer));
        PlayerPrefs.Save();
    }

    private void OnApplicationQuit()
    {
        PlayerPrefs.SetString(PlayerPrefSrings.player_data, JsonUtility.ToJson(currentPlayer));
        PlayerPrefs.Save();
    }
}
