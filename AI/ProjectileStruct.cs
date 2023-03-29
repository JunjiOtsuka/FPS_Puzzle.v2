using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct ProjectileStruct
{
    public float projectileSpeed;
    public float projectileDmg;

    public ProjectileStruct(float speed, float dmg)
    {
        projectileSpeed = speed;
        projectileDmg = dmg;
    }
}
