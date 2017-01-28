using DG.Tweening;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Controller : MonoBehaviour {

    [HideInInspector]
    public GameObject[] lines;
    GameObject portal;
    GameObject dot;
    [SerializeField]
    GameObject tutorial;
    [SerializeField]
    Color color;
    [SerializeField]
    float showPlayerDelay;

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

        if(tutorial != null)
        {
            var text = tutorial.GetComponent<Text>();
            DOTween.To(() => text.color, x => text.color = x, color, 1.5f).OnComplete(() => 
            {
                tutorial.GetComponent<Button>().interactable = true;
            });
        }
	}

    IEnumerator InitPlayer(float delay)
    {
        yield return new WaitForSeconds(delay);
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

    public void DismissTutorial()
    {
        var text = tutorial.GetComponent<Text>();
        tutorial.GetComponent<Button>().interactable = false;
        DOTween.To(() => text.color, x => text.color = x, new Color(1,1,1,0), 1.5f).OnComplete(() =>
        {
            tutorial.gameObject.SetActive(false);
        });
        StartCoroutine(InitPlayer(showPlayerDelay));
    }
}
