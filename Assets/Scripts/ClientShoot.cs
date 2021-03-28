using UnityEngine;
using Mirror;

[RequireComponent(typeof(GunManager))]
public class ClientShoot : NetworkBehaviour
{
    public Camera camera;
    public LayerMask mask;
    public GunManager weaponManager;

    private Gun currentWeapon;

    // Start is called before the first frame update
    void Start()
    {
        weaponManager = GetComponent<GunManager>();
    }

    //Checks for if the client is shooting
    void Update()
    {
        //TODO: should propbably fix this redundency
        currentWeapon = weaponManager.GetCurrentWeapon();
        if (Input.GetButtonDown("Fire1"))
        {
            CmdShoot();
        }
    }

    //Tells the server we shot
    //TODO: add lag compensation which may lead to another TODO on Server Authoritive Movement
    [Command]
    void CmdShoot()
    {
        RaycastHit hit;
        if (Physics.Raycast(camera.transform.position, camera.transform.forward, out hit, currentWeapon.range, mask))
        {
            if (hit.collider.tag == "Player")
                RpcPlayerShot(hit.collider.name, currentWeapon.damage);
        }
    }

    //Tells all clients which client was shot and for how much, clients are manage death
    [ClientRpc]
    void RpcPlayerShot(string id, int damage)
    {
        Debug.Log(id + " has been shot.");
        Client client = ClientManager.GetClient(id);
        client.currentHealth = client.currentHealth - damage;
        
        if (client.currentHealth <= 0)
        {
            if (!client.isDead)
                client.Die();
            else
                return;
        }
    }
}
