using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System;
using System.Text;
using UnityEngine.UI;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;

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
    public Server Server;
    public UILabel StatusPointer;
    public UILabel IPPointer;

    public int PlayerLimit;

    private PlayerMgr playerMgr;
    private List<Connection> lobby;

    void Start()
    {
        PlayerLimit = 4;
        Status = NetworkStatus.Offline;
        lobby = new List<Connection>();
    }

    private void UpdateUI()
    {
        if (StatusPointer == null)
        {
            GameObject go = GameObject.Find("Label - ServerState");
            if (go != null)
                StatusPointer = go.GetComponent<UILabel>();
        }
        if (IPPointer == null)
        {
            GameObject go = GameObject.Find("Label - HowToJoinURL");
            if (go != null)
                IPPointer = go.GetComponent<UILabel>();
        }

        if (StatusPointer != null)
        {
            bool online = Status == NetworkStatus.Online;

            if (online)
            {
                StatusPointer.color = new Color(0xA5, 0x00, 0x00, 0xFF);
                StatusPointer.text = "Off";
            }
            else
            {
                StatusPointer.color = new Color(0, 1, 0, 1);
                StatusPointer.text = "On";
                if (IPPointer != null)
                {
                    int port = Server.ListenedPort;
                    IPPointer.text = Network.player.ipAddress + ((port == 80) ? "" : ":" + port.ToString());
                }
            }
        }
    }

    private void Awake()
    {
        DontDestroyOnLoad(this);

        playerMgr = GetComponent<PlayerMgr>();
        if (playerMgr == null)
            Debug.Log("NetworkMgr: Cannot found the playermgr !");

        Server = GetComponent<Server>();
        Assert.IsNotNull(Server, "Cannot found a server !");

        SetupServer();
    }

    void Update()
    {
        UpdateUI();
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


        int port = 80;
        int tries = 0;
        while(!Server.Listen(port) && tries <= 10)
        {
            port = UnityEngine.Random.Range(100, 65536);
            ++tries;
        }
        if(tries > 10)
        {
            Debug.Log("SetupServer: An error happened while trying to start the server.");
            return;
        }

        Debug.Log("SetupServer: Server started on " + Network.player.ipAddress + ":"+port);
        Server.OnConnectionOpened += OnNewPlayer;
        Server.OnConnectionClosed += OnDeletedPlayer;
        Server.OnMessageReceived += OnNewMessage;

        Status = NetworkStatus.Online;
    }

    public void StopServer()
    {
        if (Status != NetworkStatus.Online)
        {
            Debug.Log("StopServer: Server already stopped !");
            return;
        }
        Status = NetworkStatus.Stopping;

        foreach(Connection c in Server.Clients)
        {
            if (c == null)
                continue;
            c.SendMessage(new KickMessage());
        }

        Server.Stop();

        Server.OnConnectionOpened -= OnNewPlayer;
        Server.OnConnectionClosed -= OnDeletedPlayer;
        Server.OnMessageReceived -= OnNewMessage;

        Debug.Log("StopServer: Server stopped !");
    }

    private void OnDeletedPlayer(string conn)
    {
        Debug.Log("Player disconnected");
        if(lobby.Contains(null))
        {
            Debug.Log("He has been removed from the lobby");
            lobby.Remove(null);
        }
        else
        {
            playerMgr.PlayerDisconnected(conn);
        }
    }

    private void OnNewPlayer(Connection conn)
    {
        Debug.Log("A new player from " + conn.Context.UserEndPoint + " has been added to the lobby");
        lobby.Add(conn);
    }

    private void OnNewMessage(Connection conn, Message mess)
    {
        if (lobby.Contains(conn))
        {
            if(mess is HelloAgainMessage) // handle reconnection
            {
                Player p = playerMgr.TryReconnect(((HelloAgainMessage)mess).Key);
                if (p != null)
                {
                    p.Socket = conn;
                    conn.SendMessage(new LoginSuccessMessage());
                    lobby.Remove(conn);
                    Debug.Log("Player " + p.Username + " successfully reconnected");
                }
                else
                    SendLoginError(conn, LoginErrors.InvalidKey);
            }
            else if(mess is HelloMessage) // new client
            { 
                if(playerMgr.Players.Count < PlayerLimit) // Check player limit
                {
                    if (playerMgr.Players.TrueForAll(p => p.Username != ((HelloMessage)mess).Username))  // Check player name
                    {
                        //TODO: Verifier si le jeu a démarrer
                        string key = playerMgr.AddPlayer(conn, ((HelloMessage)mess).Username);
                        lobby.Remove(conn);
                        conn.SendMessage(new LoginSuccessMessage(key));
                        Debug.Log("Player " + ((HelloMessage)mess).Username + " is now logged in");
                    }
                    else
                        SendLoginError(conn, LoginErrors.LoginAlreadyUsed);
                }
                else
                    SendLoginError(conn, LoginErrors.ServerFull);
            }
            else
            {
                Debug.Log("Received the message : " + mess + " during authentification. Ignored.");
            }
        }
        else
        {
            playerMgr.NewMessage(conn.ID, mess);
        }
    }

    private void SendLoginError(Connection conn, LoginErrors error)
    {
        Debug.LogError("Authentification refused. Reason : " + error.ToString());
        LoginErrorMessage err = new LoginErrorMessage();
        err.ErrorCode = error;
        conn.SendMessage(err);
        conn.Disconnect();
     }

    public void OnDestroy()
    {
        if(Status == NetworkStatus.Online && Server != null)
        {
            StopServer();
        }
    }
}


