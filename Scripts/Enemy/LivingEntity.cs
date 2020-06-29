using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LivingEntity : MonoBehaviour 
{
    public float startingHealth = 100f; //초기체력
    public float health { get; protected set; } //현재 채력
    public bool dead { get; protected set; } //사망상태

    public event Action OnDeath; //

    private const float minTimeBetDamaged = 0.1f; //공격과 공격사이 최소 대기시간
    private float lastDamagedTime; 

    protected bool IsInvulnerable
    {
        get
        {
            if (Time.time >= lastDamagedTime + minTimeBetDamaged) return false;

            return true;
        }
        
    }

    protected virtual void onEnable()
    {
        dead = false;
        health = startingHealth;
    }
    public virtual void RestoreHealth(float newHealth)
    {
        if (dead) return;

        health += newHealth;
    }
    public virtual void Die()
    {
        if (OnDeath != null) OnDeath();
        dead = true;
    }
}
