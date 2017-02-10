using DG.Tweening;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using Firebase.Database;
using Firebase;
using Firebase.Unity.Editor;

public class TitleScreen : MonoBehaviour {

    CanvasGroup panel;
    public DatabaseReference reference;
    [SerializeField]
    Text uuidText;
    [SerializeField]
    InputField nameText;

    void Start()
    {
        nameText.text = GameManager.Instance.currentPlayer == null ? "" : GameManager.Instance.currentPlayer.name;

        panel = GetComponent<CanvasGroup>();
        DOTween.To(() => panel.alpha, a => panel.alpha = a, 1.0f, 2f);

        uuidText.text = GameManager.GetUniqueID();

#if UNITY_EDITOR
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://flux-73cca.firebaseio.com/");
#endif
        reference = FirebaseDatabase.DefaultInstance.RootReference;
    }

    public void RegisterUser()
    {
        print(GameManager.Instance.currentPlayer == null);
        PlayerData data;
        if (nameText.text.Length > 0)
        {
            data = new PlayerData { name = nameText.text, uuid = GameManager.GetUniqueID() };
        }
        else
        {
            data = new PlayerData { name = "Your Display Name", uuid = GameManager.GetUniqueID() };
        }
        GameManager.Instance.currentPlayer = data;
        string json = JsonUtility.ToJson(data);
        reference.Child("/players/" + GameManager.GetUniqueID()).SetRawJsonValueAsync(json);
    }

    public void StartGame()
    {
        if (nameText.text.Length == 0)
            RegisterUser();
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
