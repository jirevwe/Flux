using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class MenuListItem : MonoBehaviour {

    public bool isExpanded = false;
    CanvasGroup expandedContent;
    LayoutElement layoutElement;
    RectTransform rectTranform;
    Button itemButton;

    void OnEnable() { 
        rectTranform = GetComponent<RectTransform>();

        layoutElement = GetComponent<LayoutElement>();
        expandedContent = transform.Find("Expanded Content").GetComponent<CanvasGroup>();
        expandedContent.gameObject.SetActive(false);

        itemButton = transform.Find("Item Button").GetComponent<Button>();
        itemButton.onClick.AddListener(OnButtonClick);
    }

    public void OnButtonClick()
    {
        var expanded = new Vector2(rectTranform.sizeDelta.x, 100);
        var contracted = new Vector2(rectTranform.sizeDelta.x, 50);

        layoutElement.preferredHeight = !isExpanded ? 50 : 100;

        if (isExpanded)
        {
            DOTween.To(() => expandedContent.alpha, x => expandedContent.alpha = x, 0f, .5f).OnComplete(() => {
                DOTween.To(() => rectTranform.sizeDelta, x => rectTranform.sizeDelta = x, contracted, .5f).OnComplete(() =>
                {
                    expandedContent.interactable = false;
                    expandedContent.gameObject.SetActive(isExpanded);
                });
            });
        }
        else
        {
            DOTween.To(() => rectTranform.sizeDelta, x => rectTranform.sizeDelta = x, expanded, .5f).OnComplete(() => {
                expandedContent.interactable = true;
                expandedContent.gameObject.SetActive(isExpanded);
            });
            DOTween.To(() => expandedContent.alpha, x => expandedContent.alpha = x, 1f, .1f);
           
        }
        isExpanded = !isExpanded;
    }
}
