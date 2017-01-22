using UnityEngine;
using DG.Tweening;
using System.Linq;

public class Dot : MonoBehaviour {

    GameObject checkPoint;
    bool isDead;

    void Awake () {
        checkPoint = new GameObject();
        checkPoint.transform.position = transform.position;
        transform.DORotate(new Vector3(0, 0, transform.localRotation.eulerAngles.z + 45f), .6f).SetLoops(-1, LoopType.Incremental);
    }
	
	void Update () {
        if(Input.GetMouseButtonUp(0) && !isDead)
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
            GetComponent<TrailRenderer>().enabled = false;
            CameraShake.shakeDuration = 0.1f;
            isDead = true;

            gameObject.GetComponent<SpriteRenderer>().DOColor(new Color(1, 1, 1, 0), 1f).OnComplete(() =>
            {
                Controller.Instance.lines.ToList().ForEach(n => 
                {
                    n.SetActive(true);
                    n.GetComponent<SpriteRenderer>().DOColor(new Color(56f/255f, 9f/255f, 217f/255f, 1), .5f);
                });
                
                GetComponent<CircleCollider2D>().enabled = true;
                GetComponent<TrailRenderer>().enabled = true;
                gameObject.GetComponent<SpriteRenderer>().DOColor(new Color(1, 1, 1, 1), .1f);

                transform.position = checkPoint.transform.position;
                isDead = false;
            });
        }
    }
}
