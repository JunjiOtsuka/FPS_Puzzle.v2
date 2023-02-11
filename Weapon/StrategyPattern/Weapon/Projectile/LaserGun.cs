using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserGun : MonoBehaviour
{
    public int m_Damage;
    public HitScan m_HitScan;
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
        m_HitScan = new HitScan(m_ElementEnum, m_WeaponEnum);
        m_HitScan.Create();        //gets the element and weapon interface
        Debug.Log(m_HitScan.m_ElementType?.DoAilement());
        Debug.Log(m_HitScan.m_WeaponType?.DoDamage());
    }
}