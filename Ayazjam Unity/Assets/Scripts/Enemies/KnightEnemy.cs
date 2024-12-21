using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnightEnemy : EnemyBase
{
    public float UltimateSecondAttackDelay = 2f;
    public float UltimateAttackTimer = 5f;

    private void Start()
    {
        selectedMovePos = transform.position;
        CurrentHealth = MaxHealth;
        currentAttackTimer = AttackTimer;
        currentWalkTimer = WalkCoolDown;
    }

    private void Update()
    {
        if (!isAttacking)
        {
            currentAttackTimer -= Time.deltaTime;
            currentWalkTimer -= Time.deltaTime;
        }
        if (currentAttackTimer <= 0 && !IsEvolved)
        {
            StartCoroutine(Attack());
            currentAttackTimer = AttackTimer;
        }
        else if (currentWalkTimer <= 0 && !isAttacking && !IsEvolved) // add if on edge of map 
        {
            Move();
            currentWalkTimer = WalkCoolDown;
        }

        if (Vector3.Distance(transform.position, selectedMovePos) > 0.01f)
        {
            transform.position = Vector3.Lerp(transform.position, selectedMovePos, MoveAnimSpeed);
        }

        if (currentAttackTimer <= 0 && IsEvolved)
        {
            currentAttackTimer = UltimateAttackTimer;
            StartCoroutine(Ultimate());
        }
    }
    public override IEnumerator Attack()
    {
        yield return new WaitForSeconds(0.5f);
    }

    public override void Die()
    {
        throw new System.NotImplementedException();
    }

    public override void Move()
    {
        
    }

    public override void TakeDamage(int damage)
    {
        throw new System.NotImplementedException();
    }

    public override IEnumerator Ultimate()
    {
        throw new System.NotImplementedException();
    }
}
