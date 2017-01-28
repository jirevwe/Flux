using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour {

    public float finalSize;

	void Start () {
        transform.DOScale(finalSize, 1).SetLoops(-1, LoopType.Yoyo);
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        transform.DOScale(.1f, 2f).OnComplete(() => 
        {
            if (SceneManager.GetActiveScene().buildIndex == 8)
            {
                SceneManager.LoadScene("Credits");
            }
            else
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        });
    }
}
