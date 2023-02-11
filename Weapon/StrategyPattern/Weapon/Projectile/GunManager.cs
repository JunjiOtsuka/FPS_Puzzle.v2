using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunManager : MonoBehaviour
{
    public int m_Damage;
    public IWeaponBase m_Gun;
    public ElementEnum m_ElementEnum;
    public IElementType m_Element;
    public WeaponEnum m_WeaponEnum;
    public IWeaponType m_Weapon;

    public void Start()
    {
        CreateNewWeapon();
    }

    public void CreateNewWeapon()
    {
        UpdateWeapon(m_ElementEnum, m_WeaponEnum); //gets the element and weapon interface
        InstantiateWeaponBase();                                        //instantiate weapon with parameters from inspector.
        Debug.Log(m_Gun.m_ElementType?.DoAilement());
        Debug.Log(m_Gun.m_WeaponType?.DoDamage());
    }

    public void UpdateWeapon(ElementEnum element, WeaponEnum weapon)
    {
        m_Element = WeaponManager.UpdateElement(element);
        m_Weapon = WeaponManager.UpdateWeaponType(weapon);
    }

    public void InstantiateWeaponBase()
    {
        m_Gun = new SpearWithAbstractFactory(new SpearFactory()).OrderWeapon(m_Damage, m_Element, m_Weapon);
        // m_Trident = new SpearType(m_Damage, m_Element);             //default weapon type as pierce
        // m_Trident = new SpearType(m_Damage, m_Element, m_Weapon);
    }
}
