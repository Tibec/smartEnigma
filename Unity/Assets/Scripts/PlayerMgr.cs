using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerMgr : MonoBehaviour {

    public List<Player> Players;
    public List<RuntimeAnimatorController> PlayersForms;
    public GameObject PlayerPrefab;

	void Start ()
    {
        DontDestroyOnLoad(this);
	}
	
	// Update is called once per frame
	void Update () {
		
	}


    public string AddPlayer(Connection conn, string username)
    {
        Player p = Instantiate(PlayerPrefab, new Vector3(-1+ Players.Count,0,0), new Quaternion()).GetComponent<Player>();
        p.Coloration = UnityEngine.Random.ColorHSV();
        p.Socket = conn;
        p.Username = username;
        p.Key = Utils.RandomString(20);
        p.GetComponent<Animator>().runtimeAnimatorController = PlayersForms[Players.Count];
        Players.Add(p);


        return p.Key;
    }

    public void PlayerDisconnected(Connection conn)
    {
        GetPlayerByConnection(conn).Connected = false;
    }

    public void NewMessage(Connection conn, Message mess)
    {
        GetPlayerByConnection(conn).ReceiveMessage(mess);
    }

    private Player GetPlayerByConnection(Connection conn)
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

    public Player TryReconnect(string key)
    {
        return Players.Find(p => p.Key == key);
    }
}
