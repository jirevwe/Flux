using UnityEngine;

public class PrefabHolder : MonoBehaviour {

    public static PrefabHolder instance;

    [HideInInspector]
    public PoolManager pool;

    public GameObject wave;
    public int amount;

    void Awake () {
        pool = PoolManager.instance;
        instance = this;

        pool.CreatePool(wave, amount);
    }
	
	public void ReuseObject(GameObject prefab, Vector3 position)
    {
        pool.ReuseObject(prefab, position, Quaternion.identity);
    }
}
