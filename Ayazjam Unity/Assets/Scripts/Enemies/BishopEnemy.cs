using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class BishopEnemy : EnemyBase
{
    [Header("Bishop")]
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
        if (currentAttackTimer <= 0 && !IsEvolved && isActive)
        {
            StartCoroutine(Attack());
            currentAttackTimer = AttackTimer;
        }
        else if (currentWalkTimer <= 0 && !isAttacking && !IsEvolved && isActive) // add if on edge of map 
        {
            Move();
            currentWalkTimer = WalkCoolDown;
        }

        if (Vector3.Distance(transform.position, selectedMovePos) > 0.01f)
        {
            transform.position = Vector3.Lerp(transform.position, selectedMovePos, MoveAnimSpeed);
        }
        else if(animator.GetBool("IsMoving"))
        {
            transform.position = selectedMovePos;
            animator.SetBool("IsMoving",false);
        }

        if (currentAttackTimer <= 0 && IsEvolved && isActive)
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
        Destroy(gameObject);
        TileManager.instance.enemiesOnBoard.Remove(this);

    }

    public override void Move()
    {
        if (boardPosition.YPos == 0)
            return;

        animator.SetTrigger("MovingStarted");
        animator.SetBool("IsMoving", true);
        int moveDir = 1;
        List<BoardPosition> moveBoardPos = new List<BoardPosition>();
        if(boardPosition.XPos >=4 && boardPosition.YPos != 0)
        {
            moveBoardPos.Add(new BoardPosition { XPos = boardPosition.XPos - 1, YPos = boardPosition.YPos - 1 });
            for (int i = 1; i < 8; i++)
            {
                if (moveBoardPos[i - 1].XPos <= 0 || moveBoardPos[i - 1].XPos >= 7 || moveBoardPos[i - 1].YPos <= 0)
                    break;
                moveBoardPos.Add(new BoardPosition { XPos = moveBoardPos[i - 1].XPos - 1, YPos = moveBoardPos[i - 1].YPos - 1 });
            }
            moveDir = -1;
        }
        else
        {
            moveBoardPos.Add(new BoardPosition { XPos = boardPosition.XPos + 1, YPos = boardPosition.YPos - 1 });
            for (int i = 1; i < 8; i++)
            {
                if (moveBoardPos[i - 1].XPos <= 0 || moveBoardPos[i-1].XPos >= 7 || moveBoardPos[i - 1].YPos <= 0)
                    break;
                moveBoardPos.Add(new BoardPosition { XPos = moveBoardPos[i-1].XPos + 1, YPos = moveBoardPos[i-1].YPos - 1 });
            }
        }

        foreach (var boardpos in moveBoardPos)
        {
            Vector3 pos = TileManager.instance.BoardPositionToWorldPosition(boardpos);
            AttackTile attackTile = Instantiate(attackTilePrefab, pos, Quaternion.identity);
            attackTile.transform.eulerAngles = new Vector3(90, 0, 0);

            attackTile.Init(AttackTileDelay);
        }
        Vector3 movedPos = transform.position + new Vector3(MoveVector.x * moveDir * moveBoardPos.Count, 0, MoveVector.y * moveBoardPos.Count);
        
        boardPosition.XPos += moveDir * moveBoardPos.Count;
        boardPosition.YPos += -moveBoardPos.Count;
        if (boardPosition.YPos == 0)
        {
            IsEvolved = true;
        }

        TileManager.instance.CheckIfEvolvedEnemyAhead(this);
        StartCoroutine(moveNumerator(movedPos));
    }

    private IEnumerator moveNumerator(Vector3 Pos)
    {
        yield return new WaitForSeconds(AttackTileDelay - 0.2f);
        selectedMovePos = Pos;
    }

    public override void TakeDamage(int damage)
    {
        CurrentHealth -= damage;
        animator.SetTrigger("GotDamaged");

        if (CurrentHealth <= 0)
        {
            Die();
        }
    }

    public override IEnumerator Ultimate()
    {
        currentAttackTimer = 10000;
        for (int i = 0; i < 8; i+=2)
        {
            for(int j = 0; j < 8; j+=2)
            {
                Vector3 pos = TileManager.instance.BoardPositionToWorldPosition(new BoardPosition { XPos = i, YPos = j });
                AttackTile attackTile = Instantiate(attackTilePrefab, pos, Quaternion.identity);
                attackTile.transform.eulerAngles = new Vector3(90, 0, 0);

                attackTile.Init(AttackTileDelay);
            }
            
        }

        yield return new WaitForSeconds(UltimateSecondAttackDelay);

        for (int i = 1; i < 8; i += 2)
        {
            for (int j = 1; j < 8; j += 2)
            {
                Vector3 pos = TileManager.instance.BoardPositionToWorldPosition(new BoardPosition { XPos = i, YPos = j });
                AttackTile attackTile = Instantiate(attackTilePrefab, pos, Quaternion.identity);
                attackTile.transform.eulerAngles = new Vector3(90, 0, 0);

                attackTile.Init(AttackTileDelay);
            }

        }

        currentAttackTimer = UltimateAttackTimer;
    }
}
