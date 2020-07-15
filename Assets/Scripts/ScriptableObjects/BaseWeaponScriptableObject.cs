using System;
using UnityEngine;

[CreateAssetMenu(menuName ="ScriptableObjects/WeaponSettings", fileName ="WeaponSettings")]
public class BaseWeaponScriptableObject : ScriptableObject
{
    public BaseWeapon prefab = null;

    [Space]
    public int damage = 100;
    public float speed = 100;
    public float fireRateDelay = 0.33f;
}
