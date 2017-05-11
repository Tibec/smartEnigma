using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Player : MonoBehaviour {

    public NetworkConnection Socket { get; private set; }
    public bool Connected { get; set; }
    public Player(NetworkConnection conn)
    {
        Socket = conn;
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void ReceiveMessage(Message mess)
    {
        Debug.Log("I received the message :" + mess.Id);
    }
}
