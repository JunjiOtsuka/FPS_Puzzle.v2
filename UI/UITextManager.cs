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
        //Show toggle switch UI interaction
        if (PlayerInteract.IsInteracting) 
        {
            switchInteractText.SetActive(true);
        } 
        else 
        {
            switchInteractText.SetActive(false);
        }
        //Show box UI interaction
        if (Box.IsInteracting) 
        {
            cubeInteractText.SetActive(true);
        } 
        else 
        {
            cubeInteractText.SetActive(false);
        }
        //Show grapple UI interaction
        if (GrapplingHook.CanGrapple) 
        {
            grapplingInteractText.SetActive(true);
        }
        else 
        {
            grapplingInteractText.SetActive(false);
        }
        //Show wall run UI
        if (PlayerStateManager.checkWallRunState(WallRunState.ABLE) && !PlayerStateManager.IsWallRunning) 
        {
            wallrunText.SetActive(true);
        }
        else
        {
            wallrunText.SetActive(false);
        }
        //Show wall jump UI
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
