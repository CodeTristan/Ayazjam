using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PawnEnemy : EnemyBase
{

    private void Start()
    {
        
    }

    private void Update()
    {
        currentAttackTimer -= Time.deltaTime;
        currentWalkTimer -= Time.deltaTime;

        if (currentAttackTimer <= 0)
        {
            Attack();
            currentAttackTimer = AttackCooldown;
        }
        else if(currentWalkTimer <= 0) // add if on edge of map 
        {
            Move();
            currentWalkTimer = WalkCoolDown;
        }
    }

    public override void Attack()
    {
        Debug.Log("Attack");
    }

    public override void Die()
    {
        throw new System.NotImplementedException();
    }

    public override void Move()
    {
        //Later check if pawn is on edge of map
        transform.position += new Vector3(MoveVector.x, 0 ,MoveVector.y) * Speed;
    }

    public override void TakeDamage(int damage)
    {
        throw new System.NotImplementedException();
    }

    public override void Ultimate()
    {
        throw new System.NotImplementedException();
    }
}
