﻿using CreativeSpore.SmartColliders;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour {
    public bool relocated = false;
    public int frameWaited = 0;
	// Use this for initialization
	void Awake ()
    {
        int i = 1;
        foreach (Player p in PlayerMgr.Instance().Players)
        {
            SmartPlatformCollider collider = p.GetComponent<SmartPlatformCollider>();
            PlayerController controller = p.GetComponent<PlayerController>();

            collider.enabled = false;
            controller.enabled = false;

            p.transform.position = GetSpawnPoint(i++);

            collider.enabled = true;
            controller.enabled = true;
        }
    }

    // Update is called once per frame
    void FixedUpdate () {
		if(!relocated /*&& frameWaited > 4*/)
        {
            relocated = true;

        }
        ++frameWaited;
    }


    static public Vector3 GetSpawnPoint(int playerID)
    {
        GameObject go = GameObject.Find("SpawnArea");
        Transform[] ts = go.GetComponentsInChildren<Transform>();
        foreach (Transform t in ts)
        {
            if (t.name == "Spawn" + playerID)
                return t.position;
        }
        return Vector3.zero;
    }

}