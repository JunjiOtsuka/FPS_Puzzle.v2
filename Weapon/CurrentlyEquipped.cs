using System;
using UnityEngine;

public class CurrentlyEquipped : MonoBehaviour
{
    public static Action currentlyEquippedWeapon;
    public static Action LeftClickWeaponAction;
    public static Action RightClickWeaponAction;
    public Barehand _Barehand;
    public GrapplingHook _GrapplingHook;
    public GameObject GunRenderer;

    private void Start()
    {
        if (WeaponStateManager.WeaponState == WeaponState.BAREHAND) EnableBareHand();
        if (WeaponStateManager.WeaponState == WeaponState.GRAPPLINGHOOK) EnableGrapple();
    }

    private void Update()
    {
        if (PlayerMovementV2.leftClick.WasPerformedThisFrame()) {
            UpdateLeftClick();
        }
        if (PlayerMovementV2.rightClick.WasPerformedThisFrame()) {
            UpdateRightClick();
        }
    }

    public void EnableBareHand() 
    {
        _Barehand.EquipWeapon();
        GunRenderer.SetActive(false);
    }

    public void EnableGrapple() 
    {
        _GrapplingHook.EquipWeapon();
        GunRenderer.SetActive(true);
    }

    // private void UpdateCurrentEquippedWeapon()
    // {
    //     currentlyEquippedWeapon?.Invoke();
    // }

    private void UpdateLeftClick()
    {
        LeftClickWeaponAction?.Invoke();
    }

    private void UpdateRightClick()
    {
        RightClickWeaponAction?.Invoke();
    }
}
