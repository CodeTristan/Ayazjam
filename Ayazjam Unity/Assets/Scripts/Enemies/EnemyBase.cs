using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyBase : MonoBehaviour
{
    public int MaxHealth;
    public int CurrentHealth;
    public int Damage = 1;
    public int Speed = 1;
    public float AttackRange = 1;
    public float AttackCooldown = 1;
    public float AttackTimer = 1;
    public float WalkCoolDown = 1;
    public bool IsEvolved;
    public Vector2 MoveVector;

    protected float currentAttackTimer;
    protected float currentWalkTimer;

    public abstract void TakeDamage(int damage);
    public abstract void Attack();
    public abstract void Move();
    public abstract void Die();
    public abstract void Ultimate();
}
