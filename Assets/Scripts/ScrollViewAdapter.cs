using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ScrollViewAdapter : MonoBehaviour {

    public string[] levelNames;
    public string[] levelNumbers;

    public RectTransform prefab;
    public Text countText;
    public ScrollRect scrollView;
    public RectTransform content;

    List<ItemView> views = new List<ItemView>();

	void Start () {
        StartCoroutine(FetchModelsFromDataSet(levelNames.Length, n => OnRecievedNewModels(n)));
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            SceneManager.LoadScene(0);
        }
    }

    //use it to add new items to the list
    public void UpdateItems()
    {
        int count = 0;
        int.TryParse(countText.text, out count);
        StartCoroutine(FetchModelsFromDataSet(count, n => OnRecievedNewModels(n)));
    }

    void OnRecievedNewModels(DataModel[] models)
    {
        for (int i = 0; i < content.childCount; i++)
        {
            Destroy(content.GetChild(i));
        }

        views.Clear();

        for (int i = 0; i < models.Length; i++)
        {
            var model = models[i];
            var instance = Instantiate(prefab.gameObject) as GameObject;
            instance.transform.SetParent(content, false);
            var newView = InitializeItemView(instance, model);
            views.Add(newView);
        }
    }

    ItemView InitializeItemView(GameObject viewGameObject, DataModel model)
    {
        ItemView view = new ItemView(viewGameObject.transform);

        view.startButton.onClick.AddListener(() => 
        {
            SceneManager.LoadScene(model.levelNumber);
        });
        view.levelName.text = model.title;
        view.levelNumber.text = model.levelNumber.ToString();
        view.levelTimeText.text = model.bestTime.ToString("Best Time: 0.00s");

        return view;
   }

    public IEnumerator FetchModelsFromDataSet(int count, Action<DataModel[]> OnDone)
    {
        var results = new DataModel[count]; 
        for (int i = 0; i < count; i++)
        {
            //static data
            results[i]             = new DataModel();
            results[i].title       = levelNames[i];
            results[i].levelNumber = levelNumbers[i];
            try
            {
                //player data
                results[i].bestTime = GameManager.Instance.currentPlayerScores[i].score;
            }
            catch (ArgumentOutOfRangeException) {
                results[i].bestTime = 0f;
            }
        }
        OnDone(results);

        yield return null;
	}
}

public class ItemView
{
    public Text levelName;
    public Text levelNumber;
    public Button startButton;
    public Text levelTimeText;

    public ItemView(Transform root)
    {
        levelName   = root.Find("Item Text").GetComponent<Text>();
        levelNumber = root.Find("Item Image").GetComponent<Text>();
        startButton  = root.Find("Expanded Content/Start Button").GetComponent<Button>();
        levelTimeText = root.Find("Expanded Content/Best time").GetComponent<Text>();
    }
}

public class DataModel
{
    public string title;
    public string levelNumber;
    public float bestTime;
}