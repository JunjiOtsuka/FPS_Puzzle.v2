using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trident : MonoBehaviour
{
    public int m_Damage;
    public Spear m_Trident;
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
        // m_Trident = new Spear(m_Damage, m_ElementType, m_WeaponType, m_ElementEnum, m_WeaponEnum);
        m_Trident = new Spear(m_ElementEnum, m_WeaponEnum);
        m_Trident.Create();        //gets the element and weapon interface
        Debug.Log(m_Trident.m_ElementType?.DoAilement());
        Debug.Log(m_Trident.m_WeaponType?.DoDamage());
    }
}
