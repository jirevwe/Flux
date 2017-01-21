using UnityEngine;
using DG.Tweening;

public class Dot : MonoBehaviour {

	void Start () {
		
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
            //gameObject.GetComponent<SpriteRenderer>().DOColor(new Color(255, 255, 255, 0), .05f).SetLoops(6, LoopType.Yoyo);
            //CameraShake.shakeDuration = 0.05f; //die basically
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "bad-dot-wave")
        {
            
        }
    }
}
