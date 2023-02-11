public class GunBase : IGunBase
{
    public int m_Damage { get; set; }
    public IElementType m_ElementType { get; set; }
    public IGunType m_GunType { get; set; }
}
