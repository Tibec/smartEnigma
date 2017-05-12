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
    public UILabel StatusPointer;

    public int PlayerLimit;

    private PlayerMgr playerMgr;
    private List<Connection> lobby;

    public NetworkManager()
    {
        PlayerLimit = 4;
        Server = new Server();
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
        //playerMgr.AddPlayer(conn);
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
        LoginErrorMessage err = new LoginErrorMessage();
        err.ErrorCode = error;
        conn.SendMessage(err);
        conn.Disconnect();
    }
}


