using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bow : MonoBehaviour
{
    public int m_Damage;
    public int m_ProjectileSpeed;
    public Projectile m_Projectile;
    public ElementEnum m_ElementEnum;
    public IElementType m_ElementType;
    public WeaponEnum m_WeaponEnum;
    public IWeaponType m_WeaponType;

    // Start is called before the first frame update
    void Start()
    {
        Create();
    }

    public void Create()
    {
        m_Projectile = new Projectile(m_ElementEnum, m_WeaponEnum);
        m_Projectile.Create();        //gets the element and weapon interface
        Debug.Log(m_Projectile.m_ElementType?.DoAilement());
        Debug.Log(m_Projectile.m_WeaponType?.DoDamage());
    }
}