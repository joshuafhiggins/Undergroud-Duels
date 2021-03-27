using System.Collections;
using UnityEngine;
using Mirror;

public class Player : NetworkBehaviour
{
    [SyncVar]
    private bool _isDead = false;
    public bool isDead { get { return _isDead; } protected set { _isDead = value; } }
    
    public int maxHealth = 125;
    
    [SyncVar]
    public int currentHealth;

    [SerializeField]
    private Behaviour[] disbaledOnDeath;
    private bool[] wasEnabled;

    [SerializeField]
    private Collider[] collidersDisbaledOnDeath;

    public void Setup()
    {
        wasEnabled = new bool[disbaledOnDeath.Length];
        for (int i = 0; i < wasEnabled.Length; i++)
        {
            wasEnabled[i] = disbaledOnDeath[i].enabled;
        }

        SetDefaults();
    }

    public void SetDefaults()
    {
        isDead = false;
        currentHealth = maxHealth;

        for (int i = 0; i < disbaledOnDeath.Length; i++)
        {
            disbaledOnDeath[i].enabled = wasEnabled[i];
        }
        for (int i = 0; i < collidersDisbaledOnDeath.Length; i++)
        {
            collidersDisbaledOnDeath[i].enabled = true;
        }
    }

    [ClientRpc]
    public void RpcTakeDamage(int damage)
    {
        if (isDead)
            return;

        currentHealth -= damage;
        Debug.Log(gameObject.name + " now has " + currentHealth + " HP.");

        if(currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        isDead = true;

        for (int i = 0; i < disbaledOnDeath.Length; i++)
        {
            disbaledOnDeath[i].enabled = false;
        }
        for (int i = 0; i < collidersDisbaledOnDeath.Length; i++)
        {
            collidersDisbaledOnDeath[i].enabled = false;
        }

        Debug.Log(transform.name + " is DEAD!");

        StartCoroutine(Respawn());
    }

    private IEnumerator Respawn()
    {
        yield return new WaitForSeconds(GameManager.instance.matchSettings.respawnTime);

        SetDefaults();
        Transform startPos = NetworkManager.singleton.GetStartPosition();
        transform.position = startPos.position;
        transform.rotation = startPos.rotation;

        Debug.Log(transform.name + " has respawned!");
    }
}
