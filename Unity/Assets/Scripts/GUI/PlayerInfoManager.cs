using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfoManager : MonoBehaviour {

    List<GameObject> playersGO;

    // Use this for initialization
    void Start() {
        playersGO = new List<GameObject>(4);
        playersGO.Add(transform.FindChild("Player1").gameObject);
        playersGO.Add(transform.FindChild("Player2").gameObject);
        playersGO.Add(transform.FindChild("Player3").gameObject);
        playersGO.Add(transform.FindChild("Player4").gameObject);
    }

    // Update is called once per frame
    void Update () {
        List<Player> p = PlayerMgr.Instance().Players;
        for (int i = 0; i < 4 ; ++i)
        {
            if(i >= p.Count)
            {
                playersGO[i].SetActive(false);
            }
            else
            {
                Player pp = p[i];
                playersGO[i].SetActive(true);
                UILabel pname = playersGO[i].GetComponentInChildren<UILabel>();
                pname.text = pp.Username;
            }

        }
	}
}
