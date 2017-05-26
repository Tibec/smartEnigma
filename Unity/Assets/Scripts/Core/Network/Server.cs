using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using WebSocketSharp;
using WebSocketSharp.Server;

public class Server : MonoBehaviour
{
    private WebSocketServer srv;
    private List<Action> pendingCalls;
    public List<Connection> Clients;

    void Start()
    {
        Clients = new List<Connection>();
        pendingCalls = new List<Action>();
    }

    private void OnDestroy()
    {
        foreach (Connection c in Clients)
        {
            if (c == null)
                continue;
            c.SendMessage(new KickMessage());
        }
        srv.Stop();
    }

    private void Update()
    {
        foreach(Action a in pendingCalls)
        {
            a.Invoke();
        }
        pendingCalls.Clear();
    }

    public bool Listen(int port)
    {
        srv = new WebSocketServer(IPAddress.Any, port);

        srv.Start();
        srv.AddWebSocketService<Connection>("/", (s) => {
            s.SetServer(this);
            s.OnConnectionOpened += ConnectionOpened;
            s.OnConnectionClosed += ConnectionClosed;
            s.OnMessageReceived += MessageReceived;
        });
    
        return srv.IsListening;
    }

    private void ConnectionClosed(Connection conn)
    {
        Clients.Remove(conn);
        pendingCalls.Add(() => OnConnectionClosed(conn.ID));
    }

    private void ConnectionOpened(Connection conn)
    {
        pendingCalls.Add(() => OnConnectionOpened(conn));
        Clients.Add(conn);
    }

    private void MessageReceived(Connection conn, Message mess)
    {
        pendingCalls.Add(() => OnMessageReceived(conn, mess));
    }

    public void Stop()
    {
        if (srv != null)
            srv.Stop();
    }

    public delegate void ConnectionEvent(Connection conn);
    public event ConnectionEvent OnConnectionOpened;
    public delegate void ConnectionClosedEvent(string conn);
    public event ConnectionClosedEvent OnConnectionClosed;

    public delegate void MessageEvent(Connection conn, Message mess);
    public event MessageEvent OnMessageReceived;
}

