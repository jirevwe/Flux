using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BadDot : MonoBehaviour {

    public static BadDot Instance;

    public GameObject wave;
    public float timeToDie;

    private GameObject[] allBadDots;

    private void Awake()
    {
        Instance = this;
        allBadDots = GameObject.FindGameObjectsWithTag("bad-dot");
    }

    void Start () {
        StartCoroutine(DOWave());
    }

    IEnumerator DOWave()
    {
        yield return new WaitForSeconds(1.5f);
        wave.GetComponent<Wave>().timeToDie = timeToDie;
        PrefabHolder.instance.ReuseObject(wave, transform.position);
        StartCoroutine(DOWave());
    }

    private void Update()
    {
        FindOtherBadDots();
    }

    public void FindOtherBadDots() 
    {
        var points = new List<Vector2>();

        for (int i = 0; i < allBadDots.Length - 1; i++)
        {
            allBadDots[i].GetComponent<LineRenderer>().SetPosition(0, allBadDots[i].transform.position);
            allBadDots[i].GetComponent<LineRenderer>().SetPosition(1, allBadDots[i + 1].transform.position);

            points.Add(allBadDots[i].GetComponent<EdgeCollider2D>().points[0] = new Vector2(allBadDots[i].transform.position.x, allBadDots[i].transform.position.y));
            points.Add(allBadDots[i].GetComponent<EdgeCollider2D>().points[1] = new Vector2(allBadDots[i + 1].transform.position.x, allBadDots[i + 1].transform.position.y));
        }

        allBadDots[allBadDots.Length - 1].GetComponent<LineRenderer>().SetPosition(0, allBadDots[allBadDots.Length - 1].transform.position);
        allBadDots[allBadDots.Length - 1].GetComponent<LineRenderer>().SetPosition(1, allBadDots[0].transform.position);

        points.Add(new Vector2(allBadDots[allBadDots.Length - 1].transform.position.x, allBadDots[allBadDots.Length - 1].transform.position.y));
        points.Add(new Vector2(allBadDots[0].transform.position.x, allBadDots[0].transform.position.y));

        allBadDots[allBadDots.Length - 1].GetComponent<EdgeCollider2D>().points = points.ToArray();
    }
}
