using DG.Tweening;
using System.Linq;
using UnityEngine;

public class Dot : MonoBehaviour {

    GameObject checkPoint;
    bool isDead;
    [SerializeField]
    float inverseSpeed;

    //components
    Rigidbody2D body;
    TrailRenderer trail;
    CircleCollider2D circleCollider;
    SpriteRenderer spriteRenderer;

    void Awake () {
        spriteRenderer = GetComponent<SpriteRenderer>();
        body = GetComponent<Rigidbody2D>();
        trail = GetComponent<TrailRenderer>();
        circleCollider = GetComponent<CircleCollider2D>();

        checkPoint = new GameObject();
        checkPoint.transform.position = transform.position;
        checkPoint.name = "checkPoint";
    }

    void FixedUpdate()
    {
        if(Input.GetMouseButtonUp(0) && !isDead)
        {
            var pointToMoveTo = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            pointToMoveTo.z = gameObject.transform.position.z;
            var headingVector = (pointToMoveTo - transform.position).normalized;

            var distance = Vector2.Distance(transform.localPosition, pointToMoveTo) / inverseSpeed;
            distance = distance > 3 ? distance : 1f;
            body.DOMove(pointToMoveTo, distance * 1f);
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {

        if(other.gameObject.CompareTag("bad-dot-wave"))
        {
            GameManager.Instance.controller.started = false;

            circleCollider.enabled = false;
            trail.enabled = false;

            CameraShake.shakeDuration = 0.1f;
            transform.DOKill();
            isDead = true;

#if UNITY_ANDROID && !UNITY_EDITOR
            GameManager.Instance.turns += 1;
#endif
            spriteRenderer.DOColor(new Color(1, 1, 1, 0), 1f).OnComplete(() =>
            {
                GameManager.Instance.controller.lines.ToList().ForEach(n =>
                {
                    n.SetActive(true);
                    n.GetComponent<SpriteRenderer>().DOColor(Color.white, .5f);
                });

                circleCollider.enabled = true;
                trail.enabled = true;
                spriteRenderer.DOColor(new Color(1, 1, 1, 1), .1f);

                transform.position = checkPoint.transform.position;
                isDead = false;

                GameManager.Instance.controller.levelStartTime = Time.timeSinceLevelLoad;
                GameManager.Instance.controller.started = true;
#if UNITY_ANDROID && !UNITY_EDITOR
                GameManager.Instance.turns += 1;
                if (GameManager.Instance.turns % 10 == 0)
                {
                    GameManager.Instance.ShowAd("video");
                }
#endif
            });
        }

        //line collision
        if (other.gameObject.CompareTag("line"))
        {
            other.gameObject.GetComponent<SpriteRenderer>().DOColor(new Color(1, 1, 1, 0), .5f).OnComplete(() =>
            {
                other.gameObject.SetActive(false);
            });
        }
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("line"))
        {
            other.gameObject.GetComponent<SpriteRenderer>().DOColor(new Color(1, 1, 1, 0), .5f).OnComplete(() =>
            {
                other.gameObject.SetActive(false);
            });
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("line"))
        {
            other.gameObject.GetComponent<SpriteRenderer>().DOColor(new Color(1, 1, 1, 0), .5f).OnComplete(() =>
            {
                other.gameObject.SetActive(false);
            });
        }
    }
}