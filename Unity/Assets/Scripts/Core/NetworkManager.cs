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
    public Server Server { get; private set; }
    public Server2 Server2 { get; private set; }
    public UILabel StatusPointer;
    public UILabel IPPointer;

    public int PlayerLimit;

    private PlayerMgr playerMgr;
    private List<Connection> lobby;

    public NetworkManager()
    {
        PlayerLimit = 4;
        Server = new Server();
        Server2 = new Server2();
        Status = NetworkStatus.Offline;
        lobby = new List<Connection>();
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

		SetupServer ();
    }

    void FixedUpdate()
    {
        Server.Update();
        Server.UpdateConnections();

        foreach(Connection c in Server.connections)
        {
            if (c == null) continue;
            if (c.queuedForDisconnection == 0)
                c.Disconnect();
            if (c.queuedForDisconnection > 0)
                --c.queuedForDisconnection;
        }

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

        Server2.Listen(82);

        Server.useWebSockets = true;
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

        if (StatusPointer != null)
        {
            StatusPointer.color = new Color(0, 1, 0, 1);
            StatusPointer.text = "On";
        }
        if(IPPointer != null)
        {
            IPPointer.color = Color.red;
            IPPointer.text = Network.player.ipAddress + ((port == 80) ? "" : ":" + port.ToString());
        }
    }

    private void OnDisconnectError(NetworkConnection arg1, byte arg2)
    {
        
    }

    public void StopServer()
    {
        if (Status != NetworkStatus.Online)
        {
            Debug.Log("StopServer: Server already stopped !");
            return;
        }
        Status = NetworkStatus.Stopping;

        foreach(Connection c in Server.connections)
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

        Status = NetworkStatus.Offline;
        if (StatusPointer != null)
        {
            StatusPointer.color = new Color(1, 0, 0, 1);
            StatusPointer.text = "Off";
        }
    }

    private void OnDeletedPlayer(Connection conn)
    {
        Debug.Log("Player disconnected");
        if(lobby.Contains(conn))
        {
            Debug.Log("He has been removed from the lobby");
            lobby.Remove(conn);
        }
        else
        {
            playerMgr.PlayerDisconnected(conn);
        }
    }

    private void OnNewPlayer(Connection conn)
    {
        Debug.Log("A new player from " + conn.address + " has been added to the lobby");
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
            playerMgr.NewMessage(conn, mess);
        }
    }

    private void SendLoginError(Connection conn, LoginErrors error)
    {
        Debug.LogError("Authentification refused. Reason : " + error.ToString());
        LoginErrorMessage err = new LoginErrorMessage();
        err.ErrorCode = error;
        conn.SendMessage(err);
        conn.SoftDisconnect();
     }

    public void OnDestroy()
    {
        if(Status == NetworkStatus.Online)
        {
            StopServer();
        }
    }
}


