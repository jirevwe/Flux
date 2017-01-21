using DG.Tweening;
using UnityEngine.SceneManagement;
using UnityEngine;

public class SplashScreen : MonoBehaviour {

	// Use this for initialization
	void Start () {
        transform.DOScale(.7f, 1).SetLoops(-1, LoopType.Yoyo);
    }
	
	public void StartGame()
    {
        SceneManager.LoadScene(1);
    }
}
