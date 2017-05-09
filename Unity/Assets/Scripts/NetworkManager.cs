using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System;
using System.Text;
using UnityEngine.UI;

public class NetworkManager : MonoBehaviour
{

    public bool serverOn = false;
    List<NetworkClient> myClient;

	// UI elements
	public Text serverStatus;
	public Text players;


    private void Awake()
    {
        DontDestroyOnLoad(this);
    }

    void Update()
    {

    }

    // Create a server and listen on a port
    public void SetupServer()
    {
		if (!serverOn) {
			NetworkServer.SetNetworkConnectionClass<MyConnection> ();
			NetworkServer.useWebSockets = true;
			if (NetworkServer.Listen (1337)) {
				serverOn = true;
				Debug.Log ("Ecoute localhost:1337");

				// Changing server status displayed state
				serverStatus.color = Color.green;
				serverStatus.text = "On";
			} else
				Debug.Log ("Couldn't open server :/");
			NetworkServer.RegisterHandler (MsgType.Connect, OnNewClient);
		}
    }

    // client function
    public void OnNewClient(NetworkMessage netMsg)
    {
        Debug.Log("Connection received !");
        string hello = "hello";
        netMsg.conn.SendBytes(Encoding.ASCII.GetBytes(hello), hello.Length, 0);
        Debug.Log(netMsg.conn.GetType().ToString());
    }

}

class MyConnection : NetworkConnection
{
    public override void TransportRecieve(byte[] bytes, int numBytes, int channelId)
    {
        Debug.Log("Données recu :");
        Debug.Log(Encoding.ASCII.GetString(bytes));
    }

}