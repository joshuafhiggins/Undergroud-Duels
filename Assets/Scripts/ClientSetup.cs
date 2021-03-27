using System.Collections.Generic;
using UnityEngine;
using Mirror;

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

        //Append the name of the Player with the Network ID, Regardless of being Local
        gameObject.name = "Player" + netId;
        
        //Register players into the manager
        /*Player player = GetComponent<Player>();
        string netID = GetComponent<NetworkIdentity>().netId.ToString();
        GameManager.RegisterPlayer(netID, player);*/
    }

    //Called when this object is destroyed, should only occur if the serevr Destroyed it, this should mean the localPlayer disconnected
    public void OnDisable()
    {
        //Renables Scene Camera
        if (SceneCamera != null)
        {
            SceneCamera.gameObject.SetActive(true);
        }

        //Unregister the player from the manager
        //GameManager.UnregisterPlayer(gameObject.name);
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
