using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PawnEnemy : EnemyBase
{
    [Header("Pawn")]
    [SerializeField] private Vector2 AttackVector;
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
        
    }

    public override void Die()
    {
        throw new System.NotImplementedException();
    }

    public override void Move()
    {
        int xPower = 1;
        int yPower = 1;
        if (OnLeftEdge && MoveVector.x < 0)
        {
            xPower = 0;
        }
        else if (OnRightEdge && MoveVector.x > 0)
        {
            xPower = 0;
        }
        else if (OnBottomEdge)
        {
            xPower = 0;
            yPower = 0;
        }

        transform.position += new Vector3(MoveVector.x * xPower, 0 ,MoveVector.y * yPower) * Speed;
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
