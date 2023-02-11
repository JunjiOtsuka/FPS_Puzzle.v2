using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IWeaponFactory
{
    IWeaponBase CreateWeapon(int Damage, IElementType newElementType, IWeaponType newWeaponType);
    // IPizza CreatePizza(IList<string> ingredients);
}

public abstract class WeaponFactory : IWeaponFactory
{
    public abstract IWeaponBase CreateWeapon(int Damage, IElementType newElementType, IWeaponType newWeaponType);
}

public class SpearFactory : WeaponFactory
{
    public override IWeaponBase CreateWeapon(int Damage, IElementType newElementType, IWeaponType newWeaponType)
    {
        return new SpearType(Damage, newElementType, newWeaponType);
    }
}

/*
    Add more factories here



*/

public interface IWeaponBase
{
    // IList<string> Toppings { get; }
    int m_Damage { get; set; }
    IElementType m_ElementType { get; set; }
    IWeaponType m_WeaponType { get; set; }
    // void Bake();
}

