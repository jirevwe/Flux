﻿using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour {

	void Start () {
        transform.DOScale(.7f, 1).SetLoops(-1, LoopType.Yoyo);
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        transform.DOScale(.1f, 2f).OnComplete(() => 
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        });
    }
}