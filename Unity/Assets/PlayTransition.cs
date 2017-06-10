using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Com.LuisPedroFonseca.ProCamera2D;

public class PlayTransition : MonoBehaviour {

	// Use this for initialization
	void Start () {
        ProCamera2DTransitionsFX camFX = FindObjectOfType<ProCamera2DTransitionsFX>();
        camFX.TransitionEnter();
    }

    // Update is called once per frame
    void Update () {
		
	}
}
