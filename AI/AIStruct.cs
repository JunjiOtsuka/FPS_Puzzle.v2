using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct AIStruct
{
    public float health;
    public float damage;
    public float speed;
    public float angSpeed;
    public float acceleration;
    public float reward; 

    public AIStruct(float health, float damage, float speed, float angSpeed, float acceleration, float reward)
    {
        this.health = health;
        this.damage = damage;
        this.speed = speed;
        this.angSpeed = angSpeed;
        this.acceleration = acceleration;
        this.reward = reward;
    }
} 