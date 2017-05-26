using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class PlayerMgr : MonoBehaviour {

    [Serializable]
    public struct PointerData { public Sprite icon; public Color color; }

    public List<Player> Players;
    public List<Connection> PlayersConn;
    public List<RuntimeAnimatorController> PlayersForms;
    public GameObject PlayerPrefab;
    public UI2DSprite PlayerPointerPrefab;
    public UIPanel PlayerActionPrefab;
    public List<PointerData> PlayerPointerIcon;

    private UIRoot root;

    void Awake ()
    {
        DontDestroyOnLoad(this);
        SceneManager.activeSceneChanged += OnSceneChange;
        root = GetComponentInChildren<UIRoot>();
        Assert.IsNotNull(root, "There must be an UIRoot inside the gamemgr component !");
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public static PlayerMgr Instance()
    {
        return FindObjectOfType<PlayerMgr>();
    }

    private void OnSceneChange(Scene previousScene, Scene newScene)
    {
        /*
        int i = 1;
        foreach(Player p in Players)
        {
            Vector3 newPos = GetSpawnPoint(i++);
         //   SceneManager.MoveGameObjectToScene(p.gameObject, newScene);
            p.transform.position.Set(0,0,0);
        }
        */
    }

    public string AddPlayer(Connection conn, string username)
    {
        Vector3 spawn = SpawnManager.GetSpawnPoint(Players.Count + 1);
        Player p = Instantiate(PlayerPrefab, new Vector3(spawn.x,spawn.y,-1), new Quaternion()).GetComponent<Player>();
		p.transform.parent = gameObject.transform;

       // p.Coloration = UnityEngine.Random.ColorHSV();
        p.Socket = conn;
        p.SocketID = conn.ID;
        p.Username = username;
        p.Key = Utils.RandomString(20);
        p.GetComponent<Animator>().runtimeAnimatorController = PlayersForms[Players.Count];
        Players.Add(p);

        InstantiatePlayerUI(p);

        return p.Key;
    }

    private void InstantiatePlayerUI(Player p)
    {
        GameObject actionGO = NGUITools.AddChild(p.gameObject, PlayerActionPrefab.gameObject);
        actionGO.transform.localPosition = new Vector3(0, 1.8f, 0);
    }

    public void RemovePlayer(string username)
    {
        foreach(Player p in Players)
        {
            if(p.Username == username)
            {
                p.Socket.SendMessage(new KickMessage());
                p.Socket.Disconnect();
                Destroy(p.gameObject);
                Players.Remove(p);
                return;
            }
        }
    }

    public void PlayerDisconnected(string conn)
    {
        Player p = GetPlayerByConnectionId(conn);
        if(p!=null)
            p.Connected = false;
    }

    public void NewMessage(string conn, Message mess)
    {
        GetPlayerByConnectionId(conn).ReceiveMessage(mess);
    }

    private Player GetPlayerByConnectionId(string id)
    {
        foreach (Player p in Players)
        {
            if (p.SocketID == id)
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
