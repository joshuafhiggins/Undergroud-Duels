using System.Collections.Generic;
using UnityEngine;
using Mirror;

//Manages the clients' guns
public class GunManager : NetworkBehaviour
{
    public List<Gun> Weapons;
    
    [SyncVar]
    private Gun currentWeapon;

    //On game start the client equips the first listed weapon in Weapons
    void Start()
    {
        EquipWeapon(Weapons[0]);
    }

    //Gets the client's current active weapon
    public Gun GetCurrentWeapon()
    {
        return currentWeapon;
    }

    //Equips a new weapon based on the parameter, although should only be equiping weapons from the list
    void EquipWeapon(Gun weapon)
    {
        currentWeapon = weapon;
    }
}
