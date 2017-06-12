using Com.LuisPedroFonseca.ProCamera2D;
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

    public Player[] Players;
    public List<RuntimeAnimatorController> PlayersForms;
    public GameObject PlayerPrefab;
    public UIPanel PlayerActionPrefab;
    public List<PointerData> PlayerPointerIcon;

    private UIRoot root;

    void Awake ()
    {
        DontDestroyOnLoad(this);
        SceneManager.activeSceneChanged += OnSceneChange;
        root = GetComponentInChildren<UIRoot>();
        Assert.IsNotNull(root, "There must be an UIRoot inside the gamemgr component !");
        Players = new Player[4];
    }

    // Update is called once per frame
    void Update () {
		
	}

    public int PlayerCount()
    {
        int res = 0;
        for (int i = 0; i < Players.Length; ++i)
        {
            if (Players[i] != null)
                ++res;
        }
        return res;

    }

    public bool ServerFull()
    {
        for(int i=0;i<Players.Length;++i)
        {
            if (Players[i] == null)
                return false;
        }
        return true;
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
        int pid = ComputePlayerId();
        Vector3 spawn = SpawnManager.GetSpawnPoint(pid + 1);
        Player p = Instantiate(PlayerPrefab, new Vector3(spawn.x,spawn.y,-1), new Quaternion()).GetComponent<Player>();
		p.transform.parent = gameObject.transform;

       // p.Coloration = UnityEngine.Random.ColorHSV();
        p.Socket = conn;
        p.SocketID = conn.ID;
        p.Username = username;
        p.Key = Utils.RandomString(20);
        p.GetComponent<Animator>().runtimeAnimatorController = PlayersForms[pid];
        Players[pid] = p;

        ProCamera2D.Instance.AddCameraTarget(p.transform);

        InstantiatePlayerUI(p);

        return p.Key;
    }

    private int ComputePlayerId()
    {
        for(int i=0;i<Players.Length; ++i)
        {
            if (Players[i] == null)
                return i;
        }
        return 0;
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
                Players[Array.IndexOf(Players, p)] = null;
                Destroy(p.gameObject);
                return;
            }
        }
    }

    public void PlayerDisconnected(string conn)
    {
        Player p = GetPlayerByConnectionId(conn);
        if (p != null)
        {
            Players[Array.IndexOf(Players, p)] = null;
            Destroy(p.gameObject);
        }
    }

    public void UpdatePlayerStatus(bool canQuit)
    {
        foreach (Player p in Players)
            p.Socket.SendMessage(new CanQuitEnigmaMessage(canQuit));
    }

    public void NewMessage(string conn, Message mess)
    {
        if(mess is QuitEnigmaMessage)
        {
            SceneLoader.Instance().LoadScene("EnigmaSelect");
        }
        else
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
        return Array.Find( Players, p => p.Key == key);
    }
}
