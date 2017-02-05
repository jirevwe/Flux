using DG.Tweening;
using UnityEngine;

public class Portal : MonoBehaviour {

    public float finalSize;

	void Start () {
        transform.DOScale(finalSize, 1).SetLoops(-1, LoopType.Yoyo);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        GetComponent<CircleCollider2D>().enabled = false;
        Controller.Instance.started = false;
        transform.DOScale(.1f, 2f).OnComplete(() => 
        {
            transform.DOKill(true);
            GameManager.Instance.FinishLevel();
        });
    }
}
