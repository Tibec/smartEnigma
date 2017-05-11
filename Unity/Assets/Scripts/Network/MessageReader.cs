using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using System;
using System.Linq;

public class MessageReader {

    private string rawData;
    private int messageId;
    private string messageData;

    private List<string> data;
	public MessageReader(string message)
    {
        rawData = message;
        Parse();
    }

    private void Parse()
    {
        string[] msg = rawData.Split('|');
        if(msg.Length != 2)
            throw new Exception("The message received is not valid : "+rawData);
        messageId = int.Parse(msg[0]);
        messageData = msg[1];
    }

    public void PrintData()
    {
        Debug.Log(rawData);
    }

    public Message GetMessage()
    {
        foreach (Type type in Assembly.GetExecutingAssembly().GetTypes().Where(myType => myType.IsClass && !myType.IsAbstract && myType.IsSubclassOf(typeof(Message))))
        {
            Message m = (Message)Activator.CreateInstance(type);
            if(m.Id == messageId)
            {
                m.Deserialize(messageData);
                return m;
            }
        }
        throw new Exception("Cannot found an implementation of the message : " + messageId);
    }
}
