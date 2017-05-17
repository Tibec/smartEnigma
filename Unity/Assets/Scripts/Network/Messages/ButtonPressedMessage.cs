using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Buttons
{
    A = 1, 
    B = 2,
    Joystick = 3, 
    DPadUp = 4,
    DPadDown = 5,
    DPadLeft = 6,
    DPadRight = 7,
}

public class ButtonPressedMessage : Message {

    public Buttons ButtonId { get; set; }
    public int Extra1 { get; set; }
    public int Extra2 { get; set; }

    public ButtonPressedMessage()
    {
        Id = 110;
    }

    public override void Deserialize(string data)
    {
        string[] splittedData = data.Split(';');
        ButtonId = (Buttons)int.Parse(splittedData[0]);
        Extra1 = int.Parse(splittedData[1]);
        Extra2 = int.Parse(splittedData[2]);
    }
}
