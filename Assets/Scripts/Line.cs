using UnityEngine;
using DG.Tweening;

public class Line : MonoBehaviour {

	void Start () {
		
	}
	
	void Update () {
		
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "dot")
        {
            gameObject.GetComponent<SpriteRenderer>().DOColor(new Color(255, 255, 255, 0), 1f).OnComplete(() => 
            {
                gameObject.SetActive(false);
            });
        }
    }
}
