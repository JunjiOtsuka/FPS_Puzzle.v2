using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponCategory { PROJECTILE, MELEE };
public enum WeaponBulletCategory { HIT_SCAN, PROJECTILE, MELEE };

[CreateAssetMenu(menuName = "Weapon")]
public class WeaponDescription : ScriptableObject
{
    public string Name;
    public string Description;
    public int Damage;
    public WeaponCategory WeaponCategory;
    public WeaponBulletCategory BulletCategory;
}
