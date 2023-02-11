using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WeaponAbstractFactory
{
    private readonly IWeaponFactory _factory;

    protected WeaponAbstractFactory(IWeaponFactory factory)
    {
        _factory = factory;
    }

    public IWeaponBase OrderWeapon(int Damage, IElementType newElementType, IWeaponType newWeaponType)
    {
        IWeaponBase weapon = _factory.CreateWeapon(Damage, newElementType, newWeaponType);
        // weapon.Bake();
        return weapon;
    }
}

public class SpearWithAbstractFactory : WeaponAbstractFactory
{
    public SpearWithAbstractFactory() : this(new SpearFactory()) {    }                        //default uses Spear Factory
    public SpearWithAbstractFactory(IWeaponFactory weaponFactory) : base(weaponFactory) {   } //non default
}