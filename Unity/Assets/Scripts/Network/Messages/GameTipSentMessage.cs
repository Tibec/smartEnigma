using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTipSentMessage : Message {

    public string Source { get; set; }
    public string Content { get; set; }

    public GameTipSentMessage()
    {
        Id = 210;
    }

    public GameTipSentMessage(string source, string content)
        : this()
    {
        Source = source;
        Content = content;
    }

    public override string Serialize()
    {
        return Source + ";" + Content;
    }
}
