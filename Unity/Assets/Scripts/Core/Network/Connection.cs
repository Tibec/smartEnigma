using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Timers;
using UnityEngine;
using WebSocketSharp.Server;

public class Connection : WebSocketBehavior
{
    private Server server;

    public void SetServer(Server server)
    {
        this.server = server;
    }

    protected override void OnClose(CloseEventArgs e)
    {
        OnConnectionClosed(this);
    }

    protected override void OnError(ErrorEventArgs e)
    {
        Log.Error("An error happened with connection '" + ID + "' : " + e.Message);
    }

    protected override void OnMessage(MessageEventArgs e)
    {
        try
        {
            MessageReader reader = new MessageReader(e.Data);
            OnMessageReceived(this, reader.GetMessage());
        }
        catch (Exception ex)
        {
            Debug.LogError("An error happened while trying to read message : " + ex.Message);
        }
    }

    protected override void OnOpen()
    {
        OnConnectionOpened(this);
    }

    public void SendMessage(Message mess)
    {
        Send(mess.Pack());
        Debug.Log("Envoi de : " + mess.Pack());
    }

    public void Disconnect()
    {
        Context.WebSocket.Close();
    }

    public delegate void ConnectionEvent(Connection conn);
    public event ConnectionEvent OnConnectionOpened;
    public event ConnectionEvent OnConnectionClosed;

    public delegate void MessageReceived(Connection conn, Message mess);
    public event MessageReceived OnMessageReceived;
}