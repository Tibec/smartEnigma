using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

using Com.LuisPedroFonseca.ProCamera2D;

public class SceneLoader : MonoBehaviour {

    private string sceneToLoad;

    // Use this for initialization
    void Start () {
        SceneManager.sceneLoaded += OnSceneLoaded;
	}

    private void OnSceneLoaded(Scene arg0, LoadSceneMode mode)
    {

    }

    private void Awake()
    {
        DontDestroyOnLoad(this);

        if (!Application.isEditor || SceneManager.sceneCount == 1)
        {
            LoadScene("Menu");
        }
    }
    // Update is called once per frame
    void Update () {
		
	}

    private void AsynchLoadScene()
    {
        SceneManager.LoadScene(sceneToLoad);
    }

    public void LoadScene(string scene)
    {
        ProCamera2DTransitionsFX camFX = FindObjectOfType<ProCamera2DTransitionsFX>();

        if (camFX != null)
        {
            camFX.OnTransitionExitEnded += AsynchLoadScene;
            camFX.TransitionExit();
            sceneToLoad = scene;
        }
        else
            SceneManager.LoadScene(scene);
    }
}
