using UnityEngine;

public class WeaponStateManager : MonoBehaviour
{
    public static WeaponState WeaponState;

    void Update()
    {
        switch (WeaponState)
        {
            case WeaponState.BAREHAND: 
                break;
            case WeaponState.GRAPPLINGHOOK: 
                break;
        }
    }
}
