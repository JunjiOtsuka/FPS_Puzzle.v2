public class GunProjectileBase : GunBase
{
    /*
    Inherits
    int m_Damage { get; set; }
    IElementType m_ElementType { get; set; }
    IGunType m_GunType { get; set; }
    */

    public GunProjectileBase(int m_Damage, IElementType m_ElementType)
    {
        this.m_Damage = m_Damage;
        this.m_ElementType = m_ElementType;
    }
}
