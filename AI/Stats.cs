using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Stats : MonoBehaviour, IDoDamage
{
    //Monster Data
    public AIStruct m_Data;
    public string name;
    public float health;
    public float damage;
    public float speed;         //base speed
    public float angSpeed;
    public float acceleration;
    public float reward; 

    //current health
    public float currentHealth;
    public float speedModifier;

    public IElementType type;

    public CooldownManager m_CooldownManager = new CooldownManager();

    public bool bFrozen;

    void Start()
    {
        currentHealth = health;
    }

    void Update()
    {
        //when monster health reaches 0 deactivate GO from scene
        if (currentHealth <= 0)
        {
            //increase coin
            TowerManager.Coins += reward;
            //deactivate
            transform.gameObject.SetActive(false);
        }

        if (type != null)
        {
            // Debug.Log(type.DoAilement());
            type.DoAilement();
        }

        if (bFrozen)
        {
            DoFreeze();
            // GetComponent<NavMeshAgent>().velocity = Vector3.zero;
            Debug.Log(speedModifier);
            GetComponent<NavMeshAgent>().speed = speedModifier;
        }
    }

    public void SetMonsterData()
    {
        m_Data = new AIStruct(health, damage, speed, angSpeed, acceleration, reward);
        MonsterData.data.Add(name, m_Data);
    }

    public void DoDamage(float dmg)
    {
        currentHealth -= dmg;
    }

    //check if there is any bugs here

    //Added 3/27/2023
    public void DoIceAilments(GameObject go, float modifier)
    {
        speedModifier = speed * modifier;
        bFrozen = true;
    }

    public void DoFreeze()
    {
        m_CooldownManager.SetCDTimer(1.0f);
        m_CooldownManager.StartCDTimer(m_CooldownManager);

        if (m_CooldownManager.bCDEnd())
        {
            bFrozen = false;
            m_CooldownManager.ResetCDTimer();
        }
    }
}
