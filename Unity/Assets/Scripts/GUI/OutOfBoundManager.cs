using Com.LuisPedroFonseca.ProCamera2D;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OutOfBoundManager : MonoBehaviour {

    List<GameObject> playersGO;
    ProCamera2D camMgr;
    // Use this for initialization
    void Start() {
        playersGO = new List<GameObject>(4);
        playersGO.Add(transform.FindChild("Player1").gameObject);
        playersGO.Add(transform.FindChild("Player2").gameObject);
        playersGO.Add(transform.FindChild("Player3").gameObject);
        playersGO.Add(transform.FindChild("Player4").gameObject);

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void Awake()
    {
		camMgr = FindObjectOfType<ProCamera2D>();
    }

    private void OnSceneLoaded(Scene previous, LoadSceneMode lsm)
    {
        Awake();
        
    }

    // Update is called once per frame
    void Update () {
        List<Player> p = PlayerMgr.Instance().Players;

        if (camMgr == null)
            Awake();

        for (int i = 0; i < 4 ; ++i)
        {
            if(i >= p.Count)
            {
                playersGO[i].SetActive(false);
            }
            else
            {
                Player pp = p[i];
                Camera cam = camMgr.GetComponent<Camera>();
                bool outofbound = false;
                Vector3 pos = cam.WorldToViewportPoint(pp.transform.position);
                if (pos.x <= 0)
                {
                    outofbound = true;
                    pos.x = 0.1f;
                }
                if (pos.x >= 1)
                {
                    outofbound = true;
                    pos.x = 0.9f;
                }
                if (pos.y <= 0)
                {
                    outofbound = true;
                    pos.y = 0.1f;
                }
                if (pos.y >= 1)
                {
                    outofbound = true;
                    pos.y = 0.9f;
                }

                if(outofbound)
                { 
                    // add vector3.up because player's origin is at their foot
                    Vector3 point = (pp.transform.position + Vector3.up - playersGO[i].transform.position);
                    float angle = (Mathf.Atan2(point.y, point.x) * 180 / Mathf.PI) % 360 + 90;
                    UI2DSprite arrow = playersGO[i].transform.FindChild("Arrow").GetComponent<UI2DSprite>();
                    Camera uiCam = PlayerMgr.Instance().GetComponentInChildren<Camera>();
                    pos = uiCam.ViewportToWorldPoint(pos);

                    arrow.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
                    playersGO[i].transform.position = pos;
                    playersGO[i].SetActive(true);
                    arrow.enabled = true;
                }
                else
                {
                    playersGO[i].SetActive(false);
                    playersGO[i].transform.FindChild("Arrow").GetComponent<UI2DSprite>().enabled = false;
                }

            }

        }
	}
}
