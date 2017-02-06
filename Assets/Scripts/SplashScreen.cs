using DG.Tweening;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SplashScreen : MonoBehaviour {

    CanvasGroup panel;
    public Image logoImage;
    public Text loadingText;
    public bool isLoadingDone = false;

    void Start () {
        logoImage.gameObject.SetActive(true);
        loadingText.gameObject.SetActive(false);

        panel = GetComponent<CanvasGroup>();
        StartLoad();
        DOTween.To(() => panel.alpha, a => panel.alpha = a, 1.0f, 2f).OnComplete(() => 
        {
            DOTween.To(() => panel.alpha, a => panel.alpha = a, 0.0f, 2f).OnComplete(ShowLoading);
        });
    }

    private void ShowLoading()
    {
        logoImage.gameObject.SetActive(false);
        loadingText.gameObject.SetActive(true);
        DOTween.To(() => panel.alpha, a => panel.alpha = a, 1.0f, 2f);
    }

	public void StartGame()
    {
        DOTween.To(() => panel.alpha, a => panel.alpha = a, 0.0f, 2f).OnComplete(() => 
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        });
    }

    IEnumerator Load()
    {
        yield return new WaitForSeconds(.5f);
        if (loadingText.text != "Loading...")
        {
            loadingText.text += ".";
        }
        else
        {
            loadingText.text = "Loading";
        }
        if (isLoadingDone)
            StopLoad();
        else
            StartCoroutine(Load());
    }

    public void StartLoad()
    {
        StartCoroutine(Load());
    }

    public void StopLoad()
    {
        StopCoroutine(Load());
        StartGame();
    }
}
