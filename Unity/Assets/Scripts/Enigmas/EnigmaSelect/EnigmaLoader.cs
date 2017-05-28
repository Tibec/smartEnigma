using System.Xml;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using System.Xml.Serialization;
using System;

public enum eDifficulty { Easy, Normal, Hard }


public class EnigmaLoader : MonoBehaviour {

    public List<EnigmaMetadata> Enigmas = new List<EnigmaMetadata>();
    public EnigmaMetadata[] VisibleEnigmas = new EnigmaMetadata[3];


    public UILabel currentPageLabel;
    public UILabel maxPageLabel;

    public Sprite emptyImage;

    UIPanel enigma1Panel;
    UIPanel enigma2Panel;
    UIPanel enigma3Panel;

    EnigmaLevel level;
    EnigmaSwitch switcher;

    int currentPage = 0;
    eDifficulty currentDifficulty = eDifficulty.Easy;

	// Use this for initialization
	void Awake () {
        level = GetComponentInChildren<EnigmaLevel>();
        switcher = GetComponentInChildren<EnigmaSwitch>();

        level.OnDifficultyChanged += DifficultyChanged;
        switcher.RequestNextPage += NextPage;

        enigma1Panel = GameObject.Find("Enigma1").GetComponent<UIPanel>();
        enigma2Panel = GameObject.Find("Enigma2").GetComponent<UIPanel>();
        enigma3Panel = GameObject.Find("Enigma3").GetComponent<UIPanel>();

        UpdateEnigmas();
    }

    private void UpdateEnigmas()
    {
        ComputeVisibleEnigma();
        UpdateUI();
    }

    private void UpdateUI()
    {
        SetEnigma(VisibleEnigmas[0], enigma1Panel);
        SetEnigma(VisibleEnigmas[1], enigma2Panel);
        SetEnigma(VisibleEnigmas[2], enigma3Panel);

        // update page count somewhere

    }

    private void SetEnigma(EnigmaMetadata data, UIPanel enigmaPanel)
    {
        UILabel description, title;
        UI2DSprite preview;

        try
        {
            description = enigmaPanel.transform.FindChild("Description").GetComponent<UILabel>();
            title = enigmaPanel.transform.FindChild("Title").GetComponent<UILabel>();
            preview = enigmaPanel.transform.FindChild("Preview").GetComponent<UI2DSprite>();
        }
        catch(Exception e)
        {
            Debug.LogError("Error while trying to retrieve enigmapanel components");
            return;
        }

        if (data == null)
        {
            title.text = "Vide";
            preview.sprite2D = emptyImage;
            description.text = "";
        }
        else
        {
            title.text = data.Name;
            preview.sprite2D = data.Picture;
            description.text = data.Description;
        }
    }

    private void ComputeVisibleEnigma()
    {
        int filled = 0;
        int ignored = 0;
        int toIgnore = currentPage * 3 * Enigmas.FindAll((e) => e.Difficulty == currentDifficulty).Count;
        foreach(EnigmaMetadata e in Enigmas)
        {
            if (filled >= 3)
                break;

            if (e.Difficulty == currentDifficulty)
            {
                if (ignored < toIgnore)
                {
                    ++ignored;
                    continue;
                }
                else
                {
                    VisibleEnigmas[filled++] = e;
                }
            }
        }

        while(filled < 3)
        {
            VisibleEnigmas[filled++] = null;
        }
    }

    private void NextPage()
    {
        ++currentPage;

        if (currentPage > Enigmas.FindAll((e) => e.Difficulty == currentDifficulty).Count / 3)
            currentPage = 0;
        UpdateEnigmas();
    }

    private void DifficultyChanged(eDifficulty newDiff)
    {
        currentPage = 0;
        currentDifficulty = newDiff;
        UpdateEnigmas();
    }

    [Serializable]
    public class EnigmaMetadata
    {
        public string Name; 
        public string Description; 
        public Sprite Picture; 
        [Scene]
        public string Scene; 
        public eDifficulty Difficulty; 
    }
}


