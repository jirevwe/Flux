using DG.Tweening;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using Firebase.Database;
using Firebase;
using Firebase.Unity.Editor;
using System.Collections.Generic;

public class TitleScreen : MonoBehaviour {

    CanvasGroup panel;
    public DatabaseReference reference;
    [SerializeField]
    Text uuidText;
    [SerializeField]
    InputField nameText;

    void Start()
    {
        string savedPlayerdata = PlayerPrefs.GetString(PlayerPrefSrings.player_data);
        GameManager.Instance.currentPlayer = JsonUtility.FromJson<PlayerData>(savedPlayerdata);
        nameText.text = GameManager.Instance.currentPlayer == null ? "" : GameManager.Instance.currentPlayer.name;

        GameManager.Instance.currentPlayerScores = new List<LevelScore>();

        panel = GetComponent<CanvasGroup>();
        DOTween.To(() => panel.alpha, a => panel.alpha = a, 1.0f, 2f);

        uuidText.text = GameManager.GetUniqueID();
#if UNITY_EDITOR
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://flux-73cca.firebaseio.com/");
#endif
        reference = FirebaseDatabase.DefaultInstance.RootReference;

        reference.Child("/players/" + GameManager.GetUniqueID()).GetValueAsync().ContinueWith((task) => 
        {
            if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                string data = snapshot.GetRawJsonValue();
                JSONObject playerData = JSONObject.Create(data);

                nameText.text = playerData["name"].str;

                PlayerData thisPlayer = new PlayerData { name = playerData["name"].str, uuid = playerData["uuid"].str };

                GameManager.Instance.currentPlayer = thisPlayer;
                PlayerPrefs.SetString(PlayerPrefSrings.player_data, JsonUtility.ToJson(thisPlayer));
                PlayerPrefs.Save();

                print("Done fetching player data");

                reference.Child("/scores/").GetValueAsync().ContinueWith((innerTask) =>
                {
                    if(innerTask.IsCompleted)
                    {
                        snapshot = innerTask.Result;
                        data = snapshot.GetRawJsonValue();
                        JSONObject scoreData = JSONObject.Create(data);

                        foreach (var key in scoreData.keys)
                        {
                            LevelScore score = new LevelScore { player_uuid = scoreData[key][thisPlayer.uuid]["player_uuid"].str,
                                                                level_number = scoreData[key][thisPlayer.uuid]["level_number"].str,
                                                                score = scoreData[key][thisPlayer.uuid]["score"].f
                            };
                            GameManager.Instance.currentPlayerScores.Add(score);
                        }
                        print("Done fetching past score data");
                    }
                });
            }
        });
    }

    public void RegisterUser()
    {
        PlayerData data = new PlayerData { name = nameText.text, uuid = GameManager.GetUniqueID() };
        string json = JsonUtility.ToJson(data);
        reference.Child("/players/" + GameManager.GetUniqueID()).SetRawJsonValueAsync(json);
    }

    public void StartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void ClearData()
    {
        PlayerPrefs.DeleteAll();
    }

    public void Settings()
    {
        SceneManager.LoadScene("Settings");
    }
}
