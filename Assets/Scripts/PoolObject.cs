using UnityEngine;

public class PoolObject : MonoBehaviour {
    [HideInInspector]
    public float timeToDie;

    public void DieOH()
    {
        Destroy(timeToDie);
    }

    public virtual void Destroy(float time)
    {
        Invoke("Destroy", time);
    }

	public virtual void OnObjectReuse() {
        DieOH();
    }

	protected void Destroy() {
		gameObject.SetActive (false);
	}
}
