using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PawnEnemy : EnemyBase
{
    [Header("Pawn")]
    [SerializeField] private Vector2 AttackVector;
    public float AttackTileDelay = 1;
    public float MoveAnimSpeed = 0.2f;

    private Vector3 selectedMove;
    
    private void Start()
    {
        selectedMove = transform.position;
        CurrentHealth = MaxHealth;
        currentAttackTimer = AttackTimer;
        currentWalkTimer = WalkCoolDown;
    }

    private void Update()
    {
        if(!isAttacking)
        {
            currentAttackTimer -= Time.deltaTime;
            currentWalkTimer -= Time.deltaTime;
        }
        if (currentAttackTimer <= 0 && !IsEvolved)
        {
            StartCoroutine(Attack());
            currentAttackTimer = AttackTimer;
        }
        else if(currentWalkTimer <= 0 && !isAttacking) // add if on edge of map 
        {
            Move();
            currentWalkTimer = WalkCoolDown;
        }

        if(Vector3.Distance(transform.position,selectedMove) > 0.01f)
        {
            transform.position = Vector3.Lerp(transform.position, selectedMove, MoveAnimSpeed);
        }
    }

    public override IEnumerator Attack()
    {
        bool attackLeft = boardPosition.XPos > 0;
        bool attackRight = boardPosition.XPos < 7;
        isAttacking = true;
        if (attackLeft)
        {
            Vector3 pos = TileManager.instance.BoardPositionToWorldPosition(new BoardPosition { XPos = boardPosition.XPos - 1, YPos = boardPosition.YPos - 1 });
            AttackTile attackTile = Instantiate(attackTilePrefab, pos, Quaternion.identity);    
            attackTile.transform.eulerAngles = new Vector3(90,0,0);
            attackTile.Init(AttackTileDelay);
            Debug.Log("LeftPos: " + pos);
        }
        if (attackRight)
        {
            Vector3 pos = TileManager.instance.BoardPositionToWorldPosition(new BoardPosition { XPos = boardPosition.XPos + 1, YPos = boardPosition.YPos - 1 });
            AttackTile attackTile = Instantiate(attackTilePrefab, pos, Quaternion.identity);
            attackTile.transform.eulerAngles = new Vector3(90, 0, 0);

            attackTile.Init(AttackTileDelay);
            Debug.Log("RightPos: " + pos);
        }
        yield return new WaitForSeconds(AttackTileDelay + 0.5f);
        isAttacking = false;
    }

    public override void Die()
    {
        //Maybe add a death animation
        Destroy(gameObject);
    }

    public override void Move()
    {
        int xPower = 1;
        int yPower = 1;
        if (boardPosition.XPos == 0 && MoveVector.x < 0)
        {
            xPower = 0;
        }
        else if (boardPosition.XPos == 0 && MoveVector.x > 0)
        {
            xPower = 0;
        }

        if (boardPosition.YPos == 0)
        {
            xPower = 0;
            yPower = 0;
        }


        Vector3 moveVector = new Vector3(MoveVector.x * xPower, 0, MoveVector.y * yPower) * Speed;
        //transform.position += moveVector;
        selectedMove = transform.position + moveVector;
        CalculateBoardPosition(moveVector);

    }

    public override void TakeDamage(int damage)
    {
        CurrentHealth -= damage;
        animator.SetTrigger("GotDamaged");

        if(CurrentHealth <= 0)
        {
            Die();
        }
    }

    public override void Ultimate()
    {
        throw new System.NotImplementedException();
    }
}
