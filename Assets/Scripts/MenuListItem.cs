using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class MenuListItem : MonoBehaviour {

    public bool isExpanded = false;
    public CanvasGroup expandedContent;
    public LayoutElement layoutElement;
    public RectTransform rectTranform;

    // Use this for initialization
    void OnEnable() { 
        rectTranform = GetComponent<RectTransform>();

        layoutElement = GetComponent<LayoutElement>();
        expandedContent = transform.Find("Expanded Content").GetComponent<CanvasGroup>();
        expandedContent.gameObject.SetActive(false);
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
                    expandedContent.gameObject.SetActive(isExpanded);
                });
            });
        }
        else
        {
            DOTween.To(() => rectTranform.sizeDelta, x => rectTranform.sizeDelta = x, expanded, .5f).OnComplete(() => {
                expandedContent.gameObject.SetActive(isExpanded);
            });
            DOTween.To(() => expandedContent.alpha, x => expandedContent.alpha = x, 1f, .1f);
           
        }
        isExpanded = !isExpanded;
    }
}
