using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class Connection : NetworkConnection
{
    public void SendMessage(Message mess)
    {
        string message = mess.Pack();
        SendBytes(Encoding.ASCII.GetBytes(message), message.Length, 0);
    }
}
