using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

using Com.LuisPedroFonseca.ProCamera2D;
using UnityEngine.Assertions;

public class SceneLoader : MonoBehaviour {

    private bool loadInProgress = false;
    private string sceneToLoad = "";

    // Use this for initialization
    void Start () {
        SceneManager.sceneLoaded += OnSceneLoaded;
	}

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        loadInProgress = false;
        PlayerMgr.Instance().UpdatePlayerStatus(EnigmaInProgress());
    }

    public string CurrentScene()
    {
        return sceneToLoad;
    }

    public bool EnigmaInProgress()
    {
        return !(sceneToLoad.Contains("Menu") || sceneToLoad.Contains("EnigmaSelect") || sceneToLoad.Contains("EnigmaClear"));
    }

    static public SceneLoader Instance()
    {
        SceneLoader loader = FindObjectOfType<SceneLoader>();
        Assert.IsNotNull(loader, "Cannot found SceneLoader !");
        return loader;
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
        loadInProgress = true;
        sceneToLoad = scene;
        PlayerMgr.Instance().UpdatePlayerStatus(false);
        if (camFX != null)
        {
            camFX.OnTransitionExitEnded += AsynchLoadScene;
            camFX.TransitionExit();
        }
        else
            SceneManager.LoadScene(scene);
    }
}
