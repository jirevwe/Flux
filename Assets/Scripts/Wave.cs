using DG.Tweening;
using UnityEngine;
using System.Collections;

public class Wave : PoolObject {

    public override void OnObjectReuse()
    {
        timeToDie = 3f;

        GetComponent<PolygonCollider2D>().enabled = true;
        StartCoroutine(Disable());

        transform.localScale = Vector3.zero;
        gameObject.GetComponent<SpriteRenderer>().color = Color.black;

        transform.DOScale(2, timeToDie);
        gameObject.GetComponent<SpriteRenderer>().DOColor(new Color(255,255,255,0), timeToDie);

        base.OnObjectReuse();
    }

    IEnumerator Disable()
    {
        yield return new WaitForSeconds(3 * timeToDie / 4);
        GetComponent<PolygonCollider2D>().enabled = false;
    }
}
