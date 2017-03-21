using UnityEngine;

public class PrefabHolder : MonoBehaviour {

    public static PrefabHolder instance;

    PoolManager pool;

    public GameObject waveRect;
    public GameObject waveRectCut;

    public GameObject waveCircle;
    public GameObject waveCircleCut;

    public GameObject waveTri;

    public int amount;

    void Awake () {
        pool = PoolManager.instance;
        instance = this;

        pool.CreatePool(waveCircleCut, amount);
        pool.CreatePool(waveRectCut, amount);
        pool.CreatePool(waveCircle, amount);
        pool.CreatePool(waveRect, amount);
        pool.CreatePool(waveTri, amount);
    }
	
	public void ReuseObject(GameObject prefab, Vector3 position, object args = null)
    {
        pool.ReuseObject(prefab, position, Quaternion.identity, args);
    }
}
