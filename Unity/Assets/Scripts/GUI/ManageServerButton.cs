using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManageServerButton : MonoBehaviour
{
    public NetworkManager network;
    private UILabel button;

    public void Awake()
    {
        button = GetComponentInChildren<UILabel>();
        if (button == null)
            Debug.Log("ManageServerButton: Cannot retrieve the button");

        UpdateButton();
    }

    public void Update()
    {
        UpdateButton();
    }

    private void UpdateButton()
    {
        if(network.Status == NetworkStatus.Offline)
        {
            button.text = "Démarrer";
        }
        else if(network.Status == NetworkStatus.Online)
        {
            button.text = "Arrêter";
        }
    }

    public void OnClick()
    {
        if (network.Status == NetworkStatus.Starting || network.Status == NetworkStatus.Stopping)
            return;

        if (network.Status == NetworkStatus.Online)
        {
            network.StopServer();
        }
        else if (network.Status == NetworkStatus.Offline)
        {
            network.SetupServer();
        }

    }
}
