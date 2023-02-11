public class Spear
{
    public string m_WeaponTypeName = "Spear";
    public int m_Damage;
    public IWeaponBase m_Spear;
    public ElementEnum m_ElementEnum;
    public IElementType m_ElementType;
    public WeaponEnum m_WeaponEnum;
    public IWeaponType m_WeaponType;

    public Spear(ElementEnum m_ElementEnum, WeaponEnum m_WeaponEnum)
    {  
        this.m_ElementEnum = m_ElementEnum;
        this.m_WeaponEnum = m_WeaponEnum;
    }
    
    // public Spear(int m_Damage, IElementType m_ElementType, IWeaponType m_WeaponType, ElementEnum m_ElementEnum, WeaponEnum m_WeaponEnum) : base(m_Damage, m_ElementType, m_WeaponType)
    // {  
    //     this.m_ElementEnum = m_ElementEnum;
    //     this.m_WeaponEnum = m_WeaponEnum;
    // }

    public void Create()
    {
        UpdateWeapon(m_ElementEnum, m_WeaponEnum); //gets the element and weapon interface
        InstantiateWeaponBase();                   //instantiate weapon with parameters from inspector.
    }

    public void UpdateWeapon(ElementEnum element, WeaponEnum weapon)
    {
        m_ElementType = WeaponManager.UpdateElement(element);
        m_WeaponType = WeaponManager.UpdateWeaponType(weapon);
    }

    public void InstantiateWeaponBase()
    {
        m_Spear = new SpearWithAbstractFactory(new SpearFactory()).OrderWeapon(m_Damage, m_ElementType, m_WeaponType);
        // m_Trident = new SpearType(m_Damage, m_Element);             //default weapon type as pierce
        // m_Trident = new SpearType(m_Damage, m_Element, m_Weapon);
    }
}
