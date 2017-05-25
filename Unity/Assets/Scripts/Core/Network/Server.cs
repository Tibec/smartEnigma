using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using WebSocketSharp;
using WebSocketSharp.Server;


public class Server : NetworkServerSimple
{
    public Server()
        : base()
    {
        SetNetworkConnectionClass<Connection>();
    }

    public override void OnData(NetworkConnection conn, int receivedSize, int channelId)
    {
        string received = (char)channelId + Encoding.ASCII.GetString(this.messageBuffer, 0, receivedSize);
        MessageReader reader = new MessageReader(received);
        OnMessageReceived((Connection)conn, reader.GetMessage());
    }

    public override void OnConnectError(int connectionId, byte error)
    {
        base.OnConnectError(connectionId, error);
        Debug.Log("Erreur a la connexion :" + error);

    }

    public override void OnConnected(NetworkConnection conn)
    {
        // base.OnConnected(conn);
        Debug.Log("Connection received !");
        OnConnectionOpened((Connection)conn);
        /*
        string hello = "hello";
        conn.SendBytes(Encoding.ASCII.GetBytes(hello), hello.Length, 0);
        Debug.Log(conn.GetType().ToString());
        */
    }

    public override void OnDisconnected(NetworkConnection conn)
    {
        Debug.Log("Connection closed");
        OnConnectionClosed((Connection)conn);
    }

    public override void OnDataError(NetworkConnection conn, byte error)
    {
        
    }

    public override void OnDisconnectError(NetworkConnection conn, byte error)
    {
        
    }

    public delegate void ConnectionEvent(Connection conn);
    public event ConnectionEvent OnConnectionOpened;
    public event ConnectionEvent OnConnectionClosed;

    public delegate void MessageReceived(Connection conn, Message mess);
    public event MessageReceived OnMessageReceived;
}

public class Server2
{
    public WebSocketServer srv;

    public Server2()
    {

    }

    public bool Listen(int port)
    {
        srv = new WebSocketServer("ws://localhost:"+ port);
        srv.Log.Level = LogLevel.Trace;
        srv.Log.File = @"D:\Users\Benjamin\Documents\Projets\SmartEnigma\git\Unity\Temp\UnityVS_bin\Debug\web.log";
        srv.Start();
        srv.AddWebSocketService<InputService>("/");
    
        return true;
    }

    public void Start()
    {

    }
}

public class InputService : WebSocketBehavior
{
    protected override void OnMessage(MessageEventArgs e)
    {
        if (e.Data.StartsWith("100"))
        {
            Send(Encoding.ASCII.GetBytes("200 | eeeeeee"));
        }
    }

    protected override void OnOpen()
    {
        //Debug.Log(s.ID); 
    }
}