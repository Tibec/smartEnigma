using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerMgr : MonoBehaviour {

    public List<Player> Players;

	void Start ()
    {
        DontDestroyOnLoad(this);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void AddPlayer(NetworkConnection conn)
    {
        Player p = new Player(conn);
        Players.Add(p);
    }

    public void PlayerDisconnected(NetworkConnection conn)
    {
        GetPlayerByConnection(conn).Connected = false;
    }

    public void NewMessage(NetworkConnection conn, Message mess)
    {
        GetPlayerByConnection(conn).ReceiveMessage(mess);
    }

    private Player GetPlayerByConnection(NetworkConnection conn)
    {
        foreach (Player p in Players)
        {
            if (p.Socket.connectionId == conn.connectionId)
            {
                return p;
            }
        }
        return null;
    }
}
