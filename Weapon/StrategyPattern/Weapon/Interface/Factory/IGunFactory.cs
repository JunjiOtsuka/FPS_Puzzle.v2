public interface IGunFactory
{
    IGunBase CreateGun(int Damage, IElementType newElementType);
}

public abstract class GunFactory : IGunFactory
{
    public abstract IGunBase CreateGun(int Damage, IElementType newElementType);
}

public class HitScanFactory : GunFactory
{
    public override IGunBase CreateGun(int Damage, IElementType newElementType)
    {
        return new GunHitScan(Damage, newElementType);
    }
}

public class ProjectileFactory : GunFactory
{
    public override IGunBase CreateGun(int Damage, IElementType newElementType)
    {
        return new GunProjectileBase(Damage, newElementType);
    }
}

public interface IGunBase
{
    // IList<string> Toppings { get; }
    int m_Damage { get; set; }
    IElementType m_ElementType { get; set; }
    IGunType m_GunType { get; set; }
    // void Bake();
}

