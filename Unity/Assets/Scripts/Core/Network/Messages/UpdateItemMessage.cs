using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateItemMessage : Message
{

    public int ItemId { get; set; }
    public string Description { get; set; }
    public string Name { get; set; }

    public UpdateItemMessage()
    {
        Id = 220;
    }

    public UpdateItemMessage(int itemId, string description, string name)
        : this()
    {
        ItemId = itemId;
        Description = description;
        Name = name;
    }

    public override string Serialize()
    {
        return string.Join(";", new string[] { ItemId.ToString(), Description, Name });
    }
}
