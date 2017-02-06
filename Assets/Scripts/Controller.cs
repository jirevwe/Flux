//using admob;
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
    Color color = Color.black;
    [SerializeField]
    float showPlayerDelay;

    [Header("Level Time Stuff")]
    [SerializeField]
    Text levelTimeText;
    [HideInInspector]
    public float levelTime = 0, levelStartTime = 0;
    [HideInInspector]
    public bool started = false;

    public static Controller Instance;

    private void Awake()
    {
        Instance = this;
    }

    void Start () {
//#if UNITY_ANDROID && !UNITY_EDITOR
//        Admob.Instance().showBannerRelative(AdSize.SmartBanner, AdPosition.TOP_CENTER, 0);
//#endif

        dot = GameObject.FindGameObjectWithTag("dot");
        lines = GameObject.FindGameObjectsWithTag("line");
        portal = GameObject.FindGameObjectWithTag("portal");

        dot.SetActive(false);
        portal.SetActive(false);

        levelTimeText.text = levelTime.ToString("0.00");

        if (tutorial != null)
        {
            var text = tutorial.GetComponent<Text>();
            var bgImage = tutorial.transform.GetChild(0).GetComponent<Image>();
            var levelText = tutorial.transform.GetChild(1).GetComponent<Text>();

            DOTween.To(() => bgImage.color, x => bgImage.color = x, new Color(1, 1, 1, 1), 1.5f);
            DOTween.To(() => levelText.color, x => levelText.color = x, color, 1.5f);
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
        started = true;
        levelStartTime = Time.timeSinceLevelLoad;
    }
	
	void Update () {
        var active = lines.Count( n => { return n.activeInHierarchy; } );
        portal.gameObject.SetActive(active == 0);

        if (Input.GetKeyUp(KeyCode.Escape) || Input.GetKeyUp(KeyCode.E))
        {
            SceneManager.LoadScene("0");
        }

        if(started)
        {
            levelTime = Time.timeSinceLevelLoad - levelStartTime;
            levelTimeText.text = levelTime.ToString("0.00");
        }
    }

    public void DismissTutorial()
    {
        var text = tutorial.GetComponent<Text>();

        tutorial.GetComponent<Button>().interactable = false;
        var bgImage = tutorial.transform.GetChild(0).GetComponent<Image>();
        var levelText = tutorial.transform.GetChild(1).GetComponent<Text>();

        DOTween.To(() => bgImage.color, x => bgImage.color = x, new Color(1, 1, 1, 0), 1.5f);
        DOTween.To(() => levelText.color, x => levelText.color = x, new Color(1, 1, 1, 0), 1.5f);
        DOTween.To(() => text.color, x => text.color = x, new Color(1,1,1,0), 1.5f).OnComplete(() =>
        {
            tutorial.gameObject.SetActive(false);
        });
        StartCoroutine(InitPlayer(showPlayerDelay));
    }
}
