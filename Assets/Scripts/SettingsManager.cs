using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour {

    [SerializeField]
    Text uuidText;

	void Start () {
        uuidText.text = SystemInfo.deviceUniqueIdentifier;
	}
	
	void Update () {
		if(Input.GetKeyUp(KeyCode.Escape))
        {
            SceneManager.LoadScene("Splash");
        }
	}
}
