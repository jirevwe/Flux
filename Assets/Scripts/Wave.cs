using DG.Tweening;
using UnityEngine;
using System.Collections;

public class Wave : PoolObject {

    public override void OnObjectReuse(object args)
    {
        var _args  = (WaveData)args;
        timeToDie  = _args.timeToFade;

        GetComponent<PolygonCollider2D>().enabled = true;
        StartCoroutine(Disable(3 * _args.timeToFade / 4));
        
        transform.localScale = Vector3.one * _args.initialScale;
        gameObject.GetComponent<SpriteRenderer>().color = _args.colorOfWave;
        
        transform.DOScale(_args.maxExpandSize, _args.timeToFade);

        switch(_args.specialMove)
        {
            case Special.isFlipped:
                transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 180));
                break;
            case Special.shouldMove:
                
                break;
            case Special.shouldRotateLeft:
                transform.DORotate(new Vector3(0, 0, transform.localRotation.eulerAngles.z + 45f), 1f);
                break;
            case Special.shouldRotateRight:
                transform.DORotate(new Vector3(0, 0, transform.localRotation.eulerAngles.z - 45f), 1f);
                break;
            case Special.shouldSpiralLeft:
                transform.DORotate(new Vector3(0, 0, 60f), _args.timeToFade);
                break;
            case Special.shouldSpiralRight:
                transform.DORotate(new Vector3(0, 0, -60f), _args.timeToFade);
                break;
            case Special.none:
                break;
        }
        gameObject.GetComponent<SpriteRenderer>().DOColor(new Color(1,1,1,0), _args.timeToFade);
        
        base.OnObjectReuse(args);
    }

    IEnumerator Disable(float fadeTime)
    {
        yield return new WaitForSeconds(fadeTime);
        GetComponent<PolygonCollider2D>().enabled = false;
    }
}
