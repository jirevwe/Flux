using DG.Tweening;
using System.Linq;
using UnityEngine;

public class Dot : MonoBehaviour {

    GameObject checkPoint;
    bool isDead;
    [SerializeField]
    float inverseSpeed;

    void Awake () {
        checkPoint = new GameObject();
        checkPoint.transform.position = transform.position;
        transform.DORotate(new Vector3(0, 0, transform.localRotation.eulerAngles.z + 45f), .6f).SetLoops(-1, LoopType.Incremental);
    }

    void FixedUpdate()
    {
        if(Input.GetMouseButtonUp(0) && !isDead)
        {
            var pointToMoveTo = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            pointToMoveTo.z = gameObject.transform.position.z;
            Vector2 headingVector = (pointToMoveTo - transform.position).normalized;

            var distance = Vector2.Distance(transform.localPosition, pointToMoveTo) / inverseSpeed;
            distance = distance > 3 ? distance : 1f;
            GetComponent<Rigidbody2D>().DOMove(pointToMoveTo, distance * 1f);
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.tag == "bad-dot-wave")
        {
            GetComponent<CircleCollider2D>().enabled = false;
            GetComponent<TrailRenderer>().enabled = false;
            CameraShake.shakeDuration = 0.1f;
            transform.DOKill();
            isDead = true;
            AdManager.Instance.turns += 1;

            gameObject.GetComponent<SpriteRenderer>().DOColor(new Color(1, 1, 1, 0), 1f).OnComplete(() =>
            {
                Controller.Instance.lines.ToList().ForEach(n =>
                {
                    n.SetActive(true);
                    n.GetComponent<SpriteRenderer>().DOColor(new Color(56f / 255f, 9f / 255f, 217f / 255f, 1), .5f);
                });

                GetComponent<CircleCollider2D>().enabled = true;
                GetComponent<TrailRenderer>().enabled = true;
                gameObject.GetComponent<SpriteRenderer>().DOColor(new Color(1, 1, 1, 1), .1f);

                transform.position = checkPoint.transform.position;
                isDead = false;

                if(AdManager.Instance.turns%10 == 0)
                {
                    AdManager.Instance.ShowAd("video");
                }
            });
        }
        if (other.gameObject.tag == "line")
        {
            other.gameObject.GetComponent<SpriteRenderer>().DOColor(new Color(1, 1, 1, 0), .5f).OnComplete(() =>
            {
                other.gameObject.SetActive(false);
            });
        }
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        if (other.gameObject.tag == "line")
        {
            other.gameObject.GetComponent<SpriteRenderer>().DOColor(new Color(1, 1, 1, 0), .5f).OnComplete(() =>
            {
                other.gameObject.SetActive(false);
            });
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.tag == "line")
        {
            other.gameObject.GetComponent<SpriteRenderer>().DOColor(new Color(1, 1, 1, 0), .5f).OnComplete(() =>
            {
                other.gameObject.SetActive(false);
            });
        }
    }
}
