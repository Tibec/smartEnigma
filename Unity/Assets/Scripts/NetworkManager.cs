using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System;
using System.Text;
using UnityEngine.UI;

public enum NetworkStatus
{
    Offline = 0,
    Starting = 1,
    Online = 2,
    Stopping = 3 
}

public class NetworkManager : MonoBehaviour
{

    public NetworkStatus Status;
    public MyServer Server { get; private set; }
    public UILabel StatusPointer;

    public int PlayerLimit;

    private PlayerMgr playerMgr;

    public NetworkManager()
    {
        PlayerLimit = 4;
        Server = new MyServer();
        Status = NetworkStatus.Offline;

    }

    private void Awake()
    {
        DontDestroyOnLoad(this);

        if (StatusPointer != null)
        {
            StatusPointer.color = new Color(1, 0, 0, 1);
            StatusPointer.text = "Off";
        }

        playerMgr = GetComponent<PlayerMgr>();
        if (playerMgr == null)
            Debug.Log("NetworkMgr: Cannot found the playermgr !");
    }

    void Update()
    {
        Server.Update();
    }

    // Create a server and listen on a port
    public void SetupServer()
    {
        if (Status != NetworkStatus.Offline)
        {
            Debug.Log("SetupServer: Server already started !");
            return;
        }
        Status = NetworkStatus.Starting;

        Server.useWebSockets = true;
        if (Server.Listen("localhost", 80))
        {
            Debug.Log("SetupServer: Server started on " + "localhost:80");
            Server.OnConnectionOpened += OnNewPlayer;
            Server.OnConnectionClosed += OnDeletedPlayer;
            Server.OnMessageReceived += OnNewMessage;

            Status = NetworkStatus.Online;

            if (StatusPointer != null)
            {
                StatusPointer.color = new Color(0, 1, 0, 1);
                StatusPointer.text = "On";
            }
        }
        else
        {
            Debug.Log("SetupServer: An error happened while trying to start the server.");
        }
    }

    public void StopServer()
    {
        if (Status != NetworkStatus.Online)
        {
            Debug.Log("StopServer: Server already stopped !");
            return;
        }
        Status = NetworkStatus.Stopping;

        Server.Stop();

        Server.OnConnectionOpened -= OnNewPlayer;
        Server.OnConnectionClosed -= OnDeletedPlayer;
        Server.OnMessageReceived -= OnNewMessage;

        Debug.Log("StopServer: Server stopped !");

        Status = NetworkStatus.Offline;
        if (StatusPointer != null)
        {
            StatusPointer.color = new Color(1, 0, 0, 1);
            StatusPointer.text = "Off";
        }

    }

    private void OnDeletedPlayer(NetworkConnection conn)
    {
        Debug.Log("Player disconnected " + conn.address);
    }

    private void OnNewPlayer(NetworkConnection conn)
    {
        Debug.Log("A new player from " + conn.address);
        playerMgr.AddPlayer(conn);
    }

    private void OnNewMessage(NetworkConnection conn, Message mess)
    {
        playerMgr.NewMessage(conn, mess);
    }
}


