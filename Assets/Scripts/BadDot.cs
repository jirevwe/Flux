using System.Collections;
using UnityEngine;

[System.Serializable]
public class WaveData
{
    public float timeToFade;
    public float maxExpandSize;
    public Color colorOfWave;
    public bool shouldRotate, isFlipped;
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
