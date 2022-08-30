using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiftStateManager : MonoBehaviour
{
    public enum LiftState
    {
        NONE,
        ONLIFT,
        UP,
        SIDE
    }
    public static LiftState liftState;
}
