using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisconnectManager : MonoBehaviour {

    public UISprite disconnectBloc;
    public UILabel countdown;

    public float TimerBeforeDisconnection = 30;
    private bool countdownInitialized = false;
    private float remainingTime;
	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update ()
    {
        bool noPlayer = PlayerMgr.Instance().PlayerCount() == 0 && !SceneLoader.Instance().CurrentScene().Contains("Menu");

        if (!countdownInitialized && noPlayer ) // start countdown
        {
            countdownInitialized = true;
            remainingTime = TimerBeforeDisconnection;
        }
        else if(countdownInitialized && noPlayer) // continue countdown
        {
            remainingTime -= Time.deltaTime;
        }
        else if(countdownInitialized && !noPlayer) // stop countdown
        {
            countdownInitialized = false;
        }

        if(countdownInitialized) // update countdown if necessary
        {
            disconnectBloc.gameObject.SetActive(true);
            countdown.text = ((int)remainingTime).ToString();
        }
        else // hide countdown
        {
            disconnectBloc.gameObject.SetActive(false);
        }

        if (countdownInitialized && remainingTime <= 0f) // Change scene if countdown expire
        {
            Utils.LoadScene("Menu");
        }
    }
}
