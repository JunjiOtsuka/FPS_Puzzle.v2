using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GunAbstractFactory
{
    private readonly IGunFactory _factory;

    protected GunAbstractFactory(IGunFactory factory)
    {
        _factory = factory;
    }

    public IGunBase OrderGun(int Damage, IElementType newElementType)
    {
        IGunBase Gun = _factory.CreateGun(Damage, newElementType);
        // Gun.Bake();
        return Gun;
    }
}

public class HitScanWithAbstractFactory : GunAbstractFactory
{
    public HitScanWithAbstractFactory() : this(new HitScanFactory()) {   }             //default uses HitScan Factory
    public HitScanWithAbstractFactory(IGunFactory GunFactory) : base(GunFactory) {   } //non default
}

public class ProjectileWithAbstractFactory : GunAbstractFactory
{
    public ProjectileWithAbstractFactory() : this(new ProjectileFactory()) {   }          //default uses Projectile Factory
    public ProjectileWithAbstractFactory(IGunFactory GunFactory) : base(GunFactory) {   } //non default
}