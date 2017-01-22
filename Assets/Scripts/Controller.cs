using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;

public class Controller : MonoBehaviour {

    [HideInInspector]
    public GameObject[] lines;
    GameObject portal;

    public static Controller Instance;

    private void Awake()
    {
        Instance = this;
    }

    void Start () {
        lines = GameObject.FindGameObjectsWithTag("line");
        portal = GameObject.FindGameObjectWithTag("portal");

        portal.SetActive(false);
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
