using DG.Tweening;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class TitleScreen : MonoBehaviour {

    CanvasGroup panel;
    [SerializeField]
    Button startButton; 

    void Start()
    {
        panel = GetComponent<CanvasGroup>();
        startButton.transform.DOScale(.7f, 1).SetLoops(-1, LoopType.Yoyo);
        DOTween.To(() => panel.alpha, a => panel.alpha = a, 1.0f, 2f);
    }

    public void StartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void Credits()
    {
        SceneManager.LoadScene("Credits");
    }
}
