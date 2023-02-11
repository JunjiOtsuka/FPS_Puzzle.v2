using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public static IElementType UpdateElement(ElementEnum m_ElementEnum)
    {
        switch(m_ElementEnum)
        {
            case (ElementEnum.Normal):
                return new Normal();
            case (ElementEnum.Fire):
                return new Fire();
            case (ElementEnum.Ice):
                return new Ice();
            case (ElementEnum.Thunder):
                return new Thunder();
        }
        return null;
    }

    public static IWeaponType UpdateWeaponType(WeaponEnum m_WeaponEnum)
    {
        switch(m_WeaponEnum)
        {
            case (WeaponEnum.Slash):
                return new Slash();
            case (WeaponEnum.Blunt):
                return new Blunt();
            case (WeaponEnum.Pierce):
                return new Pierce();
        }
        return null;
    }

    public static void UpdateWeapon(ElementEnum element, IElementType IElement, WeaponEnum weapon, IWeaponType IWeapon)
    {
        IElement = WeaponManager.UpdateElement(element);
        IWeapon = WeaponManager.UpdateWeaponType(weapon);
    }
}
