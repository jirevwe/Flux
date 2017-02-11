using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
    private static GameManager instance = null;
    public SplashScreen screen;

    public DatabaseReference reference;
    public string appID = "1285424";
    public int turns = 0;
    public PlayerData currentPlayer;
    public bool onlineMode = false;
    public List<LevelScore> currentPlayerScores = new List<LevelScore>();

    public static GameManager Instance
    {
        get { return instance; }
    }

    private void Start()
    {
        string savedPlayerdata = PlayerPrefs.GetString(PlayerPrefSrings.player_data);
        currentPlayer = JsonUtility.FromJson<PlayerData>(savedPlayerdata);
        currentPlayerScores = new List<LevelScore>();
        onlineMode = false;

#if UNITY_EDITOR
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://flux-73cca.firebaseio.com/");
#endif
        reference = FirebaseDatabase.DefaultInstance.RootReference;
       
        reference.GetValueAsync().ContinueWith((t) => 
        {
            if(t.IsCompleted)
            {
                var database = JSONObject.Create(t.Result.GetRawJsonValue());
                //print(database.Print());
                if(database.HasField("players")) //at least one player has been registered
                {
                    if (database["players"].HasField(GetUniqueID())) //this player has been registered
                    {
                        reference.Child("/players/" + GetUniqueID()).GetValueAsync().ContinueWith((task) =>
                        {
                            if (task.IsFaulted)
                            {
                                print("An error occured while fetching data from the server");
                            }
                            else if (task.IsCompleted)
                            {
                                DataSnapshot snapshot = task.Result;
                                string data = snapshot.GetRawJsonValue();
                                JSONObject playerData = JSONObject.Create(data);

                                PlayerData thisPlayer = new PlayerData { name = playerData["name"].str, uuid = playerData["uuid"].str };

                                currentPlayer = thisPlayer;
                                PlayerPrefs.SetString(PlayerPrefSrings.player_data, JsonUtility.ToJson(thisPlayer));
                                PlayerPrefs.Save();

                                print("Done fetching player data");

                                reference.Child("/scores/").GetValueAsync().ContinueWith((innerTask) =>
                                {
                                    if (innerTask.IsCompleted)
                                    {
                                        snapshot = innerTask.Result;
                                        data = snapshot.GetRawJsonValue();
                                        JSONObject scoreData = JSONObject.Create(data);

                                        //print("Keys : " + scoreData.keys.Count);
                                        for (int i = 0; i < scoreData.keys.Count; i++)
                                        {
                                            if (scoreData[scoreData.keys[i]].HasField(thisPlayer.uuid)) {
                                                LevelScore score = new LevelScore
                                                {
                                                    player_uuid = scoreData[scoreData.keys[i]][thisPlayer.uuid]["player_uuid"].str,
                                                    level_number = scoreData[scoreData.keys[i]][thisPlayer.uuid]["level_number"].str,
                                                    score = scoreData[scoreData.keys[i]][thisPlayer.uuid]["score"].f
                                                };
                                                currentPlayerScores.Add(score);
                                            }
                                        }
                                        print("Done fetching past score data");
                                        screen.isLoadingDone = onlineMode = true;
                                    }
                                });
                            }
                        });
                    }
                    else //this player has not been registered
                    {
                        screen.isLoadingDone = onlineMode = true;
                    }
                }
                else //no player has been registered
                {
                    screen.isLoadingDone = onlineMode = true;
                }
            }
        });
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
        LevelScore storedScore = currentPlayerScores.Find(n => n.level_number == newScore.level_number);

        //new level played or lower score for old level
        if (storedScore == null || storedScore.score > newScore.score)
        {
            //update global database
            if (storedScore == null)
            {
                currentPlayerScores.Add(newScore);
            }
            else
            {
                storedScore.score = newScore.score;
            }

            if (onlineMode)
            {
                //update score in the background
                reference.Child("/scores/").Child(newScore.level_number).Child(newScore.player_uuid).UpdateChildrenAsync(newScore.ToDictionary());
            }

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
