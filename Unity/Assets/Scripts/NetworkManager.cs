using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System;
using System.Text;

public class NetworkManager : MonoBehaviour
{

    public bool isAtStartup = true;
    List<NetworkClient> myClient;

    private void Awake()
    {
        DontDestroyOnLoad(this);
    }

    void Update()
    {
        if (isAtStartup)
        {
            if (Input.GetKeyDown(KeyCode.S))
            {
                SetupServer();
            }
        }
    }

    void OnGUI()
    {
        if (isAtStartup)
        {
            GUI.Label(new Rect(2, 10, 150, 100), "Appuyer S pour démarrer le serveur");

        }
    }

    // Create a server and listen on a port
    public void SetupServer()
    {
        isAtStartup = false;
        NetworkServer.SetNetworkConnectionClass<MyConnection>();
        NetworkServer.useWebSockets = true;
        if (NetworkServer.Listen(1337))
            Debug.Log("Ecoute localhost:1337");
        else
            Debug.Log("Couldn't open server :/");
        NetworkServer.RegisterHandler(MsgType.Connect, OnNewClient);

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