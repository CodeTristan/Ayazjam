using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyBase : MonoBehaviour
{
    public BoardPosition boardPosition;
    public int MaxHealth;
    public int CurrentHealth;
    public int Damage = 1;
    public int Speed = 1;
    public float AttackRange = 1;
    public float AttackTimer = 1;
    public float WalkCoolDown = 1;
    public float MoveAnimSpeed = 0.15f;
    public float AttackTileDelay = 1;

    public bool IsEvolved;
    public bool isActive;
    public Vector2 MoveVector;

    protected float currentAttackTimer;
    protected float currentWalkTimer;
    protected bool isAttacking;
    public Vector3 selectedMovePos;


    [SerializeField] protected AttackTile attackTilePrefab;
    [SerializeField] protected Animator animator;

    public abstract void TakeDamage(int damage);
    public abstract IEnumerator Attack();
    public abstract void Move();
    public abstract void Die();
    public abstract IEnumerator Ultimate();


    public void ResetMovementTimer()
    {
        currentAttackTimer = AttackTimer;
        currentWalkTimer = WalkCoolDown;
    }

    public void CalculateBoardPosition(Vector3 movedVector)
    {
        int XtilesMoved = 0;
        int YtilesMoved = 0;
        if (MoveVector.x != 0)
        {
            XtilesMoved = (int)(movedVector.x / Math.Abs(MoveVector.x));
        }

        if(MoveVector.y != 0)
        {
            YtilesMoved = (int)(movedVector.z / Math.Abs(MoveVector.y));
        }

        boardPosition.XPos += XtilesMoved;
        boardPosition.YPos += YtilesMoved;

        if(boardPosition.YPos == 0)
        {
            IsEvolved = true;
        }
    }


    public void Init(BoardPosition boardPosition)
    {
        this.boardPosition = boardPosition;
        currentAttackTimer = AttackTimer;
        currentWalkTimer = WalkCoolDown;
    }

}
