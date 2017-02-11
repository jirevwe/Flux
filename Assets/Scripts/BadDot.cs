using System.Collections;
using UnityEngine;
 public enum Special
{
    none = 0,
    shouldRotateLeft = 1,
    shouldRotateRight = 2,
    isFlipped = 3,
    shouldSpiralLeft = 4,
    shouldSpiralRight = 5,
    shouldMove = 6
}

[System.Serializable]
public class WaveData
{
    public float initialScale;
    public float timeToFade;
    public float maxExpandSize;
    public Color colorOfWave;
    public Special specialMove;
    public WaveData()
    { }
}

public class BadDot : MonoBehaviour {

    public static BadDot Instance;

    public GameObject wave;

    public WaveData args;
    public float sqawnInterval;

    private void Awake()
    {
        Instance = this;
    }

    void Start () {
        StartCoroutine(DOWave());
    }

    IEnumerator DOWave()
    {
        yield return new WaitForSeconds(sqawnInterval);
        PrefabHolder.instance.ReuseObject(wave, transform.position, args);
        StartCoroutine(DOWave());
    }
}
