using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfoManager : MonoBehaviour {

    List<GameObject> playersGO;
    List<UILabel> playersLabel;
    // Use this for initialization
    void Start() {
        playersGO = new List<GameObject>(4);
        playersGO.Add(transform.FindChild("Layout1/Player1").gameObject);
        playersGO.Add(transform.FindChild("Layout1/Player2").gameObject);
        playersGO.Add(transform.FindChild("Layout2/Player3").gameObject);
        playersGO.Add(transform.FindChild("Layout2/Player4").gameObject);

        playersLabel = new List<UILabel>();
        playersLabel.Add(null);
        playersLabel.Add(null);
        playersLabel.Add(null);
        playersLabel.Add(null);
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
                if(playersLabel[i] == null)
                    playersLabel[i] = playersGO[i].GetComponentInChildren<UILabel>();
                UILabel pname = playersLabel[i];
                pname.text = pp.Username;
            }

        }
	}
}
