using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelloMessage : Message {

    public string Username { get; set; }

    public HelloMessage()
    {
        Id = 100;
    }

    public override void Deserialize(string data)
    {
        Username = data;
    }
}
