using UnityEngine;
using Mirror;

public class PlayerShoot : NetworkBehaviour
{
    private const string PLAYER_TAG = "Player";

    public PlayerWeapon weapon;
    public Camera camera;
    public LayerMask mask;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonDown("Fire1"))
        {
            Shoot();
        }
    }

    [Client]
    void Shoot()
    {
        RaycastHit hit;
        if(Physics.Raycast(camera.transform.position, camera.transform.forward, out hit, weapon.range, mask))
        {
            if (hit.collider.tag == PLAYER_TAG)
            {
                CmdPlayerShot(hit.collider.name, weapon.damage);
            }
        }
    }

    [Command]
    void CmdPlayerShot(string id, int damage)
    {
        //Prefab parents don't have colliders idiot.
        Debug.Log(id + " has been shot.");
        Player player = GameManager.GetPlayer(id);
        player.RpcTakeDamage(damage);
    }
}
