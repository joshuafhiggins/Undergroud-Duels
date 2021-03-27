using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region Match Tracking
    public static GameManager instance;
    public MatchSettings matchSettings;

    private void Awake()
    {
        if (instance != null)
            Debug.LogError("More than one GameManager in scene!");
        else
            instance = this;
    }
    #endregion

    #region Player Tracking

    private const string PLAYER_ID_PREFIX = "Player ";
    private static Dictionary<string, Player> players = new Dictionary<string, Player>();

    public static void RegisterPlayer(string netID, Player player)
    {
        string playerID = PLAYER_ID_PREFIX + netID;
        players.Add(playerID, player);
        player.gameObject.name = playerID;
    }

    public static void UnregisterPlayer(string playerID)
    {
        players.Remove(playerID);
    }

    public static Player GetPlayer(string playerID)
    {
        return players[playerID];
    }

    #endregion
}
