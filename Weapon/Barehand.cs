using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barehand : MonoBehaviour
{
    public void EquipWeapon()
    {
        CurrentlyEquipped.LeftClickWeaponAction = LeftClick;
        CurrentlyEquipped.RightClickWeaponAction = RightClick;
        WeaponStateManager.WeaponState = WeaponState.BAREHAND;
    }

    public void LeftClick()
    {
        if (PlayerMovementV2.leftClick.WasPerformedThisFrame()) {
            Debug.Log("LeftBareHand");
        }
    }

    public void RightClick()
    {
        if (PlayerMovementV2.rightClick.WasPerformedThisFrame()) {
            Debug.Log("RightBareHand");
        }
    }
}
