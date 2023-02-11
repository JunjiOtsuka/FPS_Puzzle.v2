public class Pierce : IWeaponType
{
    public override string DoDamage()
    {
        //ignore armor
        return "Pierce Type";
    }
}