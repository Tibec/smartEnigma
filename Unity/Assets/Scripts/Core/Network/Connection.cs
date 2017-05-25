using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Timers;
using UnityEngine;
using UnityEngine.Networking;

public class Connection : NetworkConnection
{
    public int queuedForDisconnection = -1;

    public void SendMessage(Message mess)
    {
        string message = mess.Pack();
        SendBytes(Encoding.ASCII.GetBytes(message), message.Length, 0);
    }

    public void SoftDisconnect()
    {
        queuedForDisconnection = 10;
    }
}
