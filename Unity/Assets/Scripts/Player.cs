using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Player : MonoBehaviour {

    public Connection Socket { get; set; }
    public string Username { get; set; }
    public bool Connected { get; set; }
    public string Key { get; set; }

    private SpriteRenderer sprite;

	// Use this for initialization
	void Start () {
        sprite = GetComponent<SpriteRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void ReceiveMessage(Message mess)
    {
        if(mess is ButtonPressedMessage)
        {
            ButtonPressedMessage m = (ButtonPressedMessage)mess;

            if (m.ButtonId == Buttons.A)
            {
                sprite.color = Random.ColorHSV();
            }
        }
        else if(mess is ButtonReleasedMessage)
        {

        }
    }
}
