using DG.Tweening;
using UnityEngine;
using System.Collections;

public class Wave : PoolObject {

    public bool doRotate, isFlipped;

    public override void OnObjectReuse(object args)
    {
        var _args = (WaveData)args;
        timeToDie = _args.timeToFade;
        doRotate = _args.shouldRotate;
        isFlipped = _args.isFlipped;

        GetComponent<PolygonCollider2D>().enabled = true;
        StartCoroutine(Disable(3 * _args.timeToFade / 4));
        
        transform.localScale = Vector3.zero;
        gameObject.GetComponent<SpriteRenderer>().color = _args.colorOfWave;
        
        transform.DOScale(_args.maxExpandSize, _args.timeToFade);
        if (doRotate && !isFlipped)
            transform.DORotate(new Vector3(0, 0, 60f), _args.timeToFade);
        else if (!doRotate && isFlipped)
            transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 180));

        gameObject.GetComponent<SpriteRenderer>().DOColor(new Color(255,255,255,0), _args.timeToFade);
        
        base.OnObjectReuse(args);
    }

    IEnumerator Disable(float fadeTime)
    {
        yield return new WaitForSeconds(fadeTime);
        GetComponent<PolygonCollider2D>().enabled = false;
    }
}
