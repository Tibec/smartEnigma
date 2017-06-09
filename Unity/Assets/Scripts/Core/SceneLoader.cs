using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

using Com.LuisPedroFonseca.ProCamera2D;

public class SceneLoader : MonoBehaviour {

    ProCamera2DTransitionsFX camFX;

    private string sceneToLoad;

    // Use this for initialization
    void Start () {
        DontDestroyOnLoad(this);
        SceneManager.sceneLoaded += OnSceneLoaded;
	}

    private void OnSceneLoaded(Scene arg0, LoadSceneMode mode)
    {
        ProCamera2DTransitionsFX[] newCam = FindObjectsOfType<ProCamera2DTransitionsFX>();
        camFX = newCam[0];
        camFX.TransitionEnter();
    }

    private void Awake()
    {
		if (!Application.isEditor) {
			SceneManager.LoadScene (1);
		}
        else
        {
            OnSceneLoaded(new Scene(), LoadSceneMode.Single);
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
