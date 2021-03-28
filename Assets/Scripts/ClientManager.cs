using System.Collections.Generic;
using UnityEngine;

//Manages Client instances for ease of access from other scripts
public class ClientManager : MonoBehaviour
{
    //Signton of the ClientManager
    public static ClientManager instance;
    
    //Dictionary of all players by their ID and Instance
    private static Dictionary<string, Client> clients = new Dictionary<string, Client>();
    
    //Name prefixing
    private const string CLIENT_ID_PREFIX = "Player";

    //Instantiate a signleton of the ClientManager
    void Awake()
    {
        if (instance != null)
            Debug.LogError("More than one ClientManager in scene!");
        else
            instance = this;
    }

    //Register a client
    public static void RegisterClient(string netID, Client client)
    {
        string clientID = CLIENT_ID_PREFIX + netID;
        clients.Add(clientID, client);
        client.gameObject.name = clientID;
    }

    //Unregister a client
    public static void UnregisterClient(string clientID)
    {
        clients.Remove(clientID);
    }

    //Get a client
    public static Client GetClient(string clientID)
    {
        return clients[clientID];
    }

}
