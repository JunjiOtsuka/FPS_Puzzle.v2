public abstract class WeaponBase : IWeaponBase
{
    // protected Pizza(IList<string> ingredients)
    // {
    //     Toppings = ingredients;
    // }
    // IList<string> Toppings { get; }
    public int m_Damage { get; set; }
    public IElementType m_ElementType { get; set; }
    public IWeaponType m_WeaponType { get; set; }
    // public abstract void Bake();
}