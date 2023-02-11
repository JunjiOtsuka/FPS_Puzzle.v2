public class SpearType : WeaponMeleeType 
{
    public SpearType(int Damage, IElementType newElementType) 
    {
        this.m_Damage = Damage;                //Set damage amount from inspector
        this.m_ElementType = newElementType;   //Set element type       "
        this.m_WeaponType = new Pierce();      //Set default weapon type        "
    }
    
    public SpearType(int Damage, IElementType newElementType, IWeaponType newWeaponType) 
    {
        this.m_Damage = Damage;                //Set damage amount from inspector
        this.m_ElementType = newElementType;   //Set element type       "
        this.m_WeaponType = newWeaponType;      //Set default weapon type        "
    }
}