using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Credits : MonoBehaviour {

    CanvasGroup panel;

    void Start()
    {
        panel = GetComponent<CanvasGroup>();
        DOTween.To(() => panel.alpha, a => panel.alpha = a, 1.0f, 2f);
    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            SceneManager.LoadScene(0);
        }
    }
}
