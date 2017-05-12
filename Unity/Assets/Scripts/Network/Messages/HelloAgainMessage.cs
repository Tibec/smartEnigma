using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelloAgainMessage : Message {

    public string Key { get; set; }

    public HelloAgainMessage()
    {
        Id = 101;
    }

    public override void Deserialize(string data)
    {
        Key = data;
    }
}
