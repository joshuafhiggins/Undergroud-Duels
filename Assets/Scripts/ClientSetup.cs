using System.Collections.Generic;
using UnityEngine;
using Mirror;

//Sets up Clients on connect to define wether they are localPlayers or remotePlayers
[RequireComponent(typeof(Client))]
public class ClientSetup : NetworkBehaviour
{
    public List<Behaviour> DisableForNonLocalPlayers;
    public int remotePlayerLayer = 12;
    Camera SceneCamera;

    //Called when the NetworkManager Instantiates the player, local or NOT local
    void Start()
    {
        //If we are NOT the local player...
        if (!isLocalPlayer)
        {
            //Disble movement and other things
            for (int i = 0; i < DisableForNonLocalPlayers.Count; i++)
            {
                DisableForNonLocalPlayers[i].enabled = false;
            }

            //Set the gameobjects in the player to the remotePlayer layer, 12, so they are rendered
            //The default layer for players is localPlayer, 11
            SetLayerRecursively(gameObject, 12);
        }
        //If we ARE the local player...
        else
        {
            //Disable the Scene Camera so the localPlayer's camera takes priority
            SceneCamera = Camera.main;
            if (SceneCamera != null)
            {
                Camera.main.gameObject.SetActive(false);
            }
        }

        GetComponent<Client>().Setup();
    }

    //Register players into the manager and sets their names with their IDs
    public override void OnStartClient()
    {
        base.OnStartClient();
        Client client = GetComponent<Client>();
        string netID = GetComponent<NetworkIdentity>().netId.ToString();
        ClientManager.RegisterClient(netID, client);
    }

    //Called when this client disconnects from the server
    public override void OnStopClient()
    {
        //Renables Scene Camera if it was the localPlayer that disconnected
        if (SceneCamera != null && isLocalPlayer)
        {
            SceneCamera.gameObject.SetActive(true);
        }

        //Unregister the player from the manager
        ClientManager.UnregisterClient(gameObject.name);

        //SHould be called after this interaction so we can see if isLocalPlayer is true or not
        base.OnStopClient();
    }

   

    //Common function to set layers recursivley
    void SetLayerRecursively(GameObject obj, int newLayer)
    {
        if (null == obj)
        {
            return;
        }

        obj.layer = newLayer;

        foreach (Transform child in obj.transform)
        {
            if (null == child)
            {
                continue;
            }
            SetLayerRecursively(child.gameObject, newLayer);
        }
    }
}
