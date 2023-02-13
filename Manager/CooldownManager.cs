using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CooldownManager
{
    public float CDTimer = 0;
    public float MaxCDTimer = 5f;
    public bool CDStarted;
    public bool CDEnded;

    public void SetCDTimer(float NewMaxCDTimer)
    {
        MaxCDTimer = NewMaxCDTimer;
        CDStarted = true;
        CDEnded = false;
    }

    public void EndCDTimer()
    {
        CDEnded = true;
    }

    public void ResetCDTimer()
    {
        //reset timer here
        CDTimer = 0;
        CDStarted = false;
        CDEnded = false;
    }

    public bool bCDEnd()
    {
        return CDEnded;
    }

    public void StartCDTimer(CooldownManager m_CooldownManager)
    {
        //if cd hasnt started return.
        if (!m_CooldownManager.CDStarted) return;
        
        //if cd started increment timer
        m_CooldownManager.CDTimer += Time.deltaTime;

        if (m_CooldownManager.CDTimer >= m_CooldownManager.MaxCDTimer)
        {
            m_CooldownManager.EndCDTimer();
            Debug.Log("Cooldown ended");
        }

        // if (CDEnded)
        // {
        //     m_CooldownManager.DoReset();
        //     Debug.Log("Cooldown Reset");
        // }
    }
}
