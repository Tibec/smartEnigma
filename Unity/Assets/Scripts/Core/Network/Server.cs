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
    private Queue<Action> pendingCalls;
    public List<Connection> Clients;
    public int ListenedPort { get; private set; }
    void Start()
    {
        Clients = new List<Connection>();
        pendingCalls = new Queue<Action>();
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
        Queue<Action> copy = new Queue<Action>(pendingCalls);
        pendingCalls.Clear();

        while(copy.Count > 0)
        {
            Action a = copy.Dequeue();
            try
            {
                a.Invoke();
            }
            catch(Exception e)
            {
                Debug.LogError("An error happened while trying to process the websocket server event queue :" + e.Message);
            }
        }
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
        ListenedPort = port;
        return srv.IsListening;
    }

    private void ConnectionClosed(Connection conn)
    {
        Clients.Remove(conn);
        pendingCalls.Enqueue(() => OnConnectionClosed(conn.ID));
    }

    private void ConnectionOpened(Connection conn)
    {
        pendingCalls.Enqueue(() => OnConnectionOpened(conn));
        Clients.Add(conn);
    }

    private void MessageReceived(Connection conn, Message mess)
    {
        pendingCalls.Enqueue(() => OnMessageReceived(conn, mess));
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

