using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour {

    public float finalSize;

	void Start () {
        transform.DOScale(finalSize, 1).SetLoops(-1, LoopType.Yoyo);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        transform.DOScale(.1f, 2f).OnComplete(() => 
        {
            if (SceneManager.GetActiveScene().buildIndex == SceneManager.sceneCountInBuildSettings - 1)
            {
                SceneManager.LoadScene("Splash");
            }
            else
            {
                AdManager.Instance.turns += 1;
                if (AdManager.Instance.turns % 10 == 0)
                {
                    AdManager.Instance.ShowAd("video");
                }
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            }
        });
    }
}
