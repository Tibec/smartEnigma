using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonReleasedMessage : Message {

    public Buttons ButtonId { get; set; }

    public ButtonReleasedMessage()
    {
        Id = 111;
    }

    public override void Deserialize(string data)
    {
        ButtonId = (Buttons)int.Parse(data);
    }
}
