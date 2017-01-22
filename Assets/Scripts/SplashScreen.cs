using DG.Tweening;
using UnityEngine.SceneManagement;
using UnityEngine;

public class SplashScreen : MonoBehaviour {

    CanvasGroup panel;

	void Start () {
        panel = GetComponent<CanvasGroup>();

        DOTween.To(() => panel.alpha, a => panel.alpha = a, 1.0f, 2f).OnComplete(() => 
        {
            DOTween.To(() => panel.alpha, a => panel.alpha = a, 0.0f, 2f).OnComplete(StartGame);
        });
    }
	
	public void StartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
