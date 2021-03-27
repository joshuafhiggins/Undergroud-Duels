using UnityEngine;
using Mirror;

public class WeaponManager : NetworkBehaviour
{
    public PlayerWeapon PrimaryWeapon;
    public PlayerWeapon SecondaryWeapon;
    public PlayerWeapon MeleeWeapon;
    
    private PlayerWeapon currentWeapon;

    // Start is called before the first frame update
    void Start()
    {
        EquipWeapon(PrimaryWeapon);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public PlayerWeapon GetCurrentWeapon()
    {
        return currentWeapon;
    }

    void EquipWeapon(PlayerWeapon weapon)
    {
        currentWeapon = weapon;
    }
}
