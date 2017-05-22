using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerMgr : MonoBehaviour {

    public List<Player> Players;
    public List<RuntimeAnimatorController> PlayersForms;
    public GameObject PlayerPrefab;
    public UILabel PlayerNamePrefab;
    public UILabel PlayerInteractPrefab;

    void Start ()
    {
        DontDestroyOnLoad(this);
	}
	
	// Update is called once per frame
	void Update () {
		
	}


    public string AddPlayer(Connection conn, string username)
    {
        Player p = Instantiate(PlayerPrefab, new Vector3(-1+ Players.Count,0,-1), new Quaternion()).GetComponent<Player>();
		p.transform.parent = gameObject.transform;

       // p.Coloration = UnityEngine.Random.ColorHSV();
        p.Socket = conn;
        p.Username = username;
        p.Key = Utils.RandomString(20);
        p.GetComponent<Animator>().runtimeAnimatorController = PlayersForms[Players.Count];
        Players.Add(p);

        UILabel i = (UILabel)Instantiate(PlayerNamePrefab);
        i.text = username;
        i.SetAnchor(p.transform.FindChild("NameAnchor").gameObject, 0, 0, 0, 50);

        UILabel i2 = (UILabel)Instantiate(PlayerNamePrefab);
        i2.SetAnchor(p.transform.FindChild("ActionAnchor").gameObject, 120, 0, 0, 50);
        i2.enabled = false;
        p.InteractionLabel = i2;
        // i.SetDimensions

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
