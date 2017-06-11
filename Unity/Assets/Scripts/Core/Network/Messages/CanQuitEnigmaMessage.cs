using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanQuitEnigmaMessage : Message {

    public bool CanQuit { get; set; }

    public CanQuitEnigmaMessage()
    {
        Id = 211;
    }

    public CanQuitEnigmaMessage(bool canQuit)
        : this()
    {
        CanQuit = canQuit;
    }

    public override string Serialize()
    {
        return CanQuit ? "1" : "0";
    }
}
