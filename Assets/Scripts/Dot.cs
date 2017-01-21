using UnityEngine;
using DG.Tweening;

public class Dot : MonoBehaviour {

    GameObject checkPoint;

    void Awake () {
        checkPoint = new GameObject();
        checkPoint.transform.position = transform.position;
	}
	
	void Update () {
        if(Input.GetMouseButtonUp(0))
        {
            var pointToMoveTo = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            pointToMoveTo.z = gameObject.transform.position.z;
            gameObject.transform.DOMove(pointToMoveTo, 1f);
        }
	}

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "bad-dot-wave")
        {
            GetComponent<CircleCollider2D>().enabled = false;
            CameraShake.shakeDuration = 0.1f;
            gameObject.GetComponent<SpriteRenderer>().DOColor(new Color(255, 255, 255, 0), 1f).OnComplete(() =>
            {
                transform.position = checkPoint.transform.position;
                GetComponent<CircleCollider2D>().enabled = true;
                gameObject.GetComponent<SpriteRenderer>().DOColor(new Color(255, 255, 255, 1), .1f);
            });
           
        }
    }
}
