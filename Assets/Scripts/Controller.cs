using UnityEngine;
using System.Linq;
using System.Collections;
using UnityEngine.SceneManagement;

public class Controller : MonoBehaviour {

    [HideInInspector]
    public GameObject[] lines;
    GameObject portal;
    GameObject dot;

    public static Controller Instance;

    private void Awake()
    {
        Instance = this;
    }

    void Start () {
        dot = GameObject.FindGameObjectWithTag("dot");
        lines = GameObject.FindGameObjectsWithTag("line");
        portal = GameObject.FindGameObjectWithTag("portal");

        dot.SetActive(false);
        portal.SetActive(false);

        StartCoroutine(InitPlayer());
	}

    IEnumerator InitPlayer()
    {
        yield return new WaitForSeconds(5f);
        dot.SetActive(true);
    }
	
	void Update () {
        var active = lines.Count( n => { return n.activeInHierarchy; } );
        portal.gameObject.SetActive(active == 0);

        if (Input.GetKeyUp(KeyCode.Escape))
        {
            SceneManager.LoadScene(0);
        }
    }
}
