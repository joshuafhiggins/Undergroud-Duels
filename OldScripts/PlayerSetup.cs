using UnityEngine;
using Mirror;

[RequireComponent(typeof(Player))]
public class PlayerSetup : NetworkBehaviour
{
    private const string PLAYER_ID_PREFIX = "Player ";

    public GameObject childThatNeedsAName;
    public Behaviour[] componetsToDisable;
    public int remoteLayer = 12;
    
    Camera sceneCamera;

    private void Start()
    {
        if(!isLocalPlayer)
        {
            DisableComponets();
            AssignRemoteLayer();
        }
        else
        {
            sceneCamera = Camera.main;
            if (sceneCamera != null)
            {
                Camera.main.gameObject.SetActive(false);
            }
        }

        GetComponent<Player>().Setup();
    }

    public override void OnStartClient()
    {
        base.OnStartClient();

        string netID = GetComponent<NetworkIdentity>().netId.ToString();
        Player player = GetComponent<Player>();

        childThatNeedsAName.name = PLAYER_ID_PREFIX + netID;
        GameManager.RegisterPlayer(netID, player);
    }

    private void AssignRemoteLayer()
    {
        SetLayerRecursively(gameObject, 12);
    }

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

    private void DisableComponets()
    {
        for (int i = 0; i < componetsToDisable.Length; i++)
        {
            componetsToDisable[i].enabled = false;
        }
    }

    private void OnDisable()
    {
        if(sceneCamera != null)
        {
            sceneCamera.gameObject.SetActive(true);
        }

        GameManager.UnregisterPlayer(gameObject.name);
    }
}
