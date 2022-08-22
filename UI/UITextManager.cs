using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UITextManager : MonoBehaviour
{
    public GameObject switchInteractText;
    public GameObject cubeInteractText;
    public GameObject grapplingInteractText;
    public GameObject wallrunText;
    public GameObject walljumpText;

    void Update()
    {
        if (PlayerInteract.IsInteracting) 
        {
            switchInteractText.SetActive(true);
        } 
        else 
        {
            switchInteractText.SetActive(false);
        }
        if (Box.IsInteracting) 
        {
            cubeInteractText.SetActive(true);
        } 
        else 
        {
            cubeInteractText.SetActive(false);
        }
        if (GrapplingHook.CanGrapple) 
        {
            grapplingInteractText.SetActive(true);
        }
        else 
        {
            grapplingInteractText.SetActive(false);
        }
        if (PlayerStateManager.checkWallRunState(WallRunState.ABLE) && !PlayerStateManager.IsWallRunning) 
        {
            wallrunText.SetActive(true);
        }
        else
        {
            wallrunText.SetActive(false);
        }
        if (PlayerStateManager.CanWallJump) 
        {
            walljumpText.SetActive(true);
        }
        else 
        {
            walljumpText.SetActive(false);
        }
    }
}
