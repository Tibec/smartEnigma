using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class EnigmaLevel : InteractableElementBehaviour
{
    [Serializable]
    public class DifficultyData { public Color color; public string name; }

    eDifficulty currentDiff = eDifficulty.Easy;
    UILabel diffText;
    public List<DifficultyData> diffData = new List<DifficultyData>();

    private void Awake()
    {
        GameObject go = GameObject.Find("LabelDifficulty");
        if (go != null)
            diffText = go.GetComponent<UILabel>();

        Assert.IsNotNull(diffText, "Cannot found the difficulty label !");
        UpdateText();
    }

    public override void OnInteraction(InteractableElement ie, Player p)
    {
        Lever l = ie as Lever;
        if(l != null)
        {
            if (currentDiff == eDifficulty.Easy)
                currentDiff = eDifficulty.Normal;
            else if (currentDiff == eDifficulty.Normal)
                currentDiff = eDifficulty.Hard;
            else // (currentDiff == eDifficulty.Hard)
            {
                currentDiff = eDifficulty.Easy;
                l.SetState(Lever.eLeverState.Left);
            }

            //OnDifficultyChanged(currentDiff);
            UpdateText();
            OnDifficultyChanged(currentDiff);
        }
    }

    private void UpdateText()
    {
        switch(currentDiff)
        {
            case eDifficulty.Easy:
                diffText.text = diffData[0].name;
                diffText.color = diffData[0].color;
                break;
            case eDifficulty.Normal:
                diffText.text = diffData[1].name;
                diffText.color = diffData[1].color;
                break;
            case eDifficulty.Hard:
                diffText.text = diffData[2].name;
                diffText.color = diffData[2].color;
                break;
        }
    }

    void Update()
    {

    }

    public delegate void DifficultyChangedEvent(eDifficulty newDiff);
    public event DifficultyChangedEvent OnDifficultyChanged;
}
