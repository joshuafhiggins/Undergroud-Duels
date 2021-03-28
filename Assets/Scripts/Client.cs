using System.Collections;
using UnityEngine;
using Mirror;

//A container for Clients, general information to go here to have general access to
public class Client : NetworkBehaviour
{
    //Public varibles can't have set protection
    [SyncVar]
    private bool _isDead = false;
    public bool isDead { get { return _isDead; } protected set { _isDead = value; } }

    //Health that can be set by other classes
    public int maxHealth = 125;
    [SyncVar]
    public int currentHealth;

    //Varibles used for Setup and SetDefaults
    [SerializeField]
    private Behaviour[] disbaledOnDeath;
    private bool[] wasEnabled;

    //Sets up the defaults to use for SetDefaults
    public void Setup()
    {
        wasEnabled = new bool[disbaledOnDeath.Length];
        for (int i = 0; i < wasEnabled.Length; i++)
        {
            wasEnabled[i] = disbaledOnDeath[i].enabled;
        }

        SetDefaults();
    }

    //Sets the defaults of the compenets that were changed on death
    public void SetDefaults()
    {
        isDead = false;
        currentHealth = maxHealth;

        for (int i = 0; i < disbaledOnDeath.Length; i++)
        {
            disbaledOnDeath[i].enabled = wasEnabled[i];
        }
    }

    //Kills client from other scripts
    //Note: was originally private from before the rewrite
    public void Die()
    {
        //Sets the varible
        isDead = true;

        //Changes what we want to false
        for (int i = 0; i < disbaledOnDeath.Length; i++)
        {
            disbaledOnDeath[i].enabled = false;
        }

        //Log it
        Debug.Log(transform.name + " is DEAD!");

        //Start respawn timer
        StartCoroutine(Respawn());
    }

    //Logic handling respawning
    private IEnumerator Respawn()
    {
        //Respawn after for X seconds
        yield return new WaitForSeconds(3f);

        //Reset back to defaults
        SetDefaults();
        
        //Get a start position from the NetworkManager and set our position and rotation accordingly
        Transform startPos = NetworkManager.singleton.GetStartPosition();
        transform.position = startPos.position;
        transform.rotation = startPos.rotation;

        Debug.Log(transform.name + " has respawned!");
    }
}
