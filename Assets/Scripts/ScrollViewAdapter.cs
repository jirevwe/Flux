using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollViewAdapter : MonoBehaviour {

    public string[] levelNames;
    public string[] levelNumbers;

    public RectTransform prefab;
    public Text countText;
    public ScrollRect scrollView;
    public RectTransform content;

    List<ItemView> views = new List<ItemView>();

	// Use this for initialization
	void Start () {
        StartCoroutine(FetchModelsFromDataSet(levelNames.Length, n => OnRecievedNewModels(n)));
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

        var menuItem = viewGameObject.GetComponent<MenuListItem>();

        Button buttonInstance = menuItem.GetComponent<Button>();
        buttonInstance.onClick.AddListener(menuItem.OnButtonClick);

        view.levelName.text = model.title;
        view.levelNumber.text = model.levelNumber.ToString();

        return view;
   }

    // Update is called once per frame
    public IEnumerator FetchModelsFromDataSet(int count, Action<DataModel[]> OnDone) {
        var results = new DataModel[count]; 
        for (int i = 0; i < count; i++)
        {
            results[i]             = new DataModel();
            results[i].title       = levelNames[i];
            results[i].levelNumber = levelNumbers[i];
        }
        OnDone(results);

        yield return null;
	}
}

public class ItemView
{
    public Text levelName;
    public Text levelNumber;

    public ItemView(Transform root)
    {
        levelName = root.Find("Item Text").GetComponent<Text>();
        levelNumber = root.Find("Item Image").GetComponent<Text>();
    }
}

public class DataModel
{
    public string title;
    public string levelNumber;
}