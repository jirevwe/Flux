using UnityEngine;

public class PrefabHolder : MonoBehaviour {

    public static PrefabHolder instance;

    [HideInInspector]
    public PoolManager pool;

    public GameObject waveRect;
    public GameObject waveCircle;
    public GameObject waveTri;
    public GameObject waveRectCut;

    public int amount;

    void Awake () {
        pool = PoolManager.instance;
        instance = this;

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
