public class HitScan
{
    public string m_WeaponTypeName = "HitScan";
    public int m_Damage;
    public IGunBase m_HitScan;
    public ElementEnum m_ElementEnum;
    public IElementType m_ElementType;
    public WeaponEnum m_WeaponEnum;
    public IWeaponType m_WeaponType;

    public HitScan(ElementEnum m_ElementEnum, WeaponEnum m_WeaponEnum)
    {  
        this.m_ElementEnum = m_ElementEnum;
        this.m_WeaponEnum = m_WeaponEnum;
    }

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
        m_HitScan = new HitScanWithAbstractFactory(new HitScanFactory()).OrderGun(m_Damage, m_ElementType);
    }
}