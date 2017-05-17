using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoginSuccessMessage : Message {

    public string Key { get; set; }

    public LoginSuccessMessage()
    {
        Id = 200;
    }

    public LoginSuccessMessage(string key)
        : this()
    {
        Key = key;
    }

    public override string Serialize()
    {
        return Key;
    }
}
