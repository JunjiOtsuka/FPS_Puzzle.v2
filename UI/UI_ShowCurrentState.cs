using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_ShowCurrentState : MonoBehaviour
{
    public Text GOText;

    void Update()
    {
        GOText.text = $"PlayerState:{PlayerStateManager.state} \nWallState:{WallDetection.state} \nWallRunState:{PlayerStateManager.WRState} \nGroundState:{GroundDetector.state} \nJumpState:{PlayerStateManager.JumpState}";
    }
}
