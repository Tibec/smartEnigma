using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateItemMessage : Message
{

    public int ItemId { get; set; }

    public UpdateItemMessage()
    {
        Id = 220;
    }

    public UpdateItemMessage(int itemId)
        : this()
    {
        ItemId = itemId;
    }

    public override string Serialize()
    {
        return ItemId.ToString();
    }
}
