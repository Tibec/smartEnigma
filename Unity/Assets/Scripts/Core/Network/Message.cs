using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Message {

	public int Id { get; protected set; }

    public virtual void Deserialize(string data)
    {

    }

    public virtual string Serialize()
    {
        return "";
    }

    public string Pack()
    {
        return Id + "|" + Serialize();
    }
}
