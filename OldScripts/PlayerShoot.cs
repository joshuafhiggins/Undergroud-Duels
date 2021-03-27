using UnityEngine;
using Mirror;

[RequireComponent (typeof (WeaponManager))]
public class PlayerShoot : NetworkBehaviour
{
    private PlayerWeapon currentWeapon;

    public Camera camera;
    public LayerMask mask;
    public WeaponManager WeaponManager;

    void Start()
    {
        WeaponManager = GetComponent<WeaponManager>();
    }

    void Update()
    {
        currentWeapon = WeaponManager.GetCurrentWeapon();
        if(Input.GetButtonDown("Fire1"))
        {
            CmdShoot();
        }
    }

    [Command]
    void CmdShoot()
    {
        RaycastHit hit;
        if(Physics.Raycast(camera.transform.position, camera.transform.forward, out hit, currentWeapon.range, mask))
        {
            if (hit.collider.tag == "Player")
                RpcPlayerShot(hit.collider.name, currentWeapon.damage);
        }
    }

    [ClientRpc]
    void RpcPlayerShot(string id, int damage)
    {
        Debug.Log(id + " has been shot.");
        Player player = GameManager.GetPlayer(id);
        player.currentHealth = player.currentHealth - currentWeapon.damage;
    }
}
