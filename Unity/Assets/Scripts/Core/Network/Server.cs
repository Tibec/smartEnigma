using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

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

    public delegate void ConnectionEvent(Connection conn);
    public event ConnectionEvent OnConnectionOpened;
    public event ConnectionEvent OnConnectionClosed;

    public delegate void MessageReceived(Connection conn, Message mess);
    public event MessageReceived OnMessageReceived;
}