using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RookEnemy : EnemyBase
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

        if (currentAttackTimer <= 0 && IsEvolved && isActive)
        {
            currentAttackTimer = UltimateAttackTimer;
            StartCoroutine(Ultimate());
        }
    }
    public override IEnumerator Attack()
    {
        isAttacking = true;
        int rightTiles = 7 - boardPosition.XPos;
        int leftTiles = boardPosition.XPos;

        int upTiles = 7 - boardPosition.YPos;
        int downTiles = boardPosition.YPos;

        for (int i = 1; i <= rightTiles; i++)
        {
            Vector3 pos = TileManager.instance.BoardPositionToWorldPosition(new BoardPosition { XPos = boardPosition.XPos + i, YPos = boardPosition.YPos });
            AttackTile attackTile = Instantiate(attackTilePrefab, pos, Quaternion.identity);
            attackTile.transform.eulerAngles = new Vector3(90, 0, 0);
            attackTile.Init(AttackTileDelay);
        }
        for (int i = 1; i <= leftTiles; i++)
        {
            Vector3 pos = TileManager.instance.BoardPositionToWorldPosition(new BoardPosition { XPos = boardPosition.XPos - i, YPos = boardPosition.YPos });
            AttackTile attackTile = Instantiate(attackTilePrefab, pos, Quaternion.identity);
            attackTile.transform.eulerAngles = new Vector3(90, 0, 0);
            attackTile.Init(AttackTileDelay);
        }
        for (int i = 1; i <= upTiles; i++)
        {
            Vector3 pos = TileManager.instance.BoardPositionToWorldPosition(new BoardPosition { XPos = boardPosition.XPos, YPos = boardPosition.YPos + i });
            AttackTile attackTile = Instantiate(attackTilePrefab, pos, Quaternion.identity);
            attackTile.transform.eulerAngles = new Vector3(90, 0, 0);
            attackTile.Init(AttackTileDelay);
        }
        for (int i = 1; i <= downTiles; i++)
        {
            Vector3 pos = TileManager.instance.BoardPositionToWorldPosition(new BoardPosition { XPos = boardPosition.XPos, YPos = boardPosition.YPos - i });
            AttackTile attackTile = Instantiate(attackTilePrefab, pos, Quaternion.identity);
            attackTile.transform.eulerAngles = new Vector3(90, 0, 0);
            attackTile.Init(AttackTileDelay);
        }

        yield return new WaitForSeconds(AttackTileDelay + 0.5f);
        isAttacking = false;

    }

    public override void Die()
    {
        Destroy(gameObject);
        TileManager.instance.enemiesOnBoard.Remove(this);
        TileManager.instance.allEnemies.Remove(this);
        TileManager.instance.getEnemiesOnBoard();

    }

    public override void Move()
    {
        int xPower = 1;
        int yPower = 1;
        if (boardPosition.XPos == 0 && MoveVector.x < 0)
        {
            xPower = 0;
        }
        else if (boardPosition.XPos == 7 && MoveVector.x > 0)
        {
            xPower = 0;
        }

        if (boardPosition.YPos == 0)
        {
            xPower = 0;
            yPower = 0;
        }
        if(boardPosition.YPos < Speed)
        {
            Speed = boardPosition.YPos;
        }

        Vector3 moveVector = new Vector3(MoveVector.x * xPower, 0, MoveVector.y * yPower) * Speed;
        selectedMovePos = transform.position + moveVector;
        CalculateBoardPosition(moveVector);
        animator.SetTrigger("IsMoving");
        TileManager.instance.CheckIfEvolvedEnemyAhead(this);
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
        int xPos = 0;
        int yPos = 0;
        currentAttackTimer = 10000;

        for (int i = 0; i < 4; i++)
        {
            for (int j = xPos; j < 4; j++)
            {
                Vector3 pos = TileManager.instance.BoardPositionToWorldPosition(new BoardPosition { XPos = j, YPos = yPos });
                AttackTile attackTile = Instantiate(attackTilePrefab, pos, Quaternion.identity);
                attackTile.transform.eulerAngles = new Vector3(90, 0, 0);
                attackTile.Init(AttackTileDelay);

                Vector3 pos2 = TileManager.instance.BoardPositionToWorldPosition(new BoardPosition { XPos = 7-j, YPos = 7-yPos });
                AttackTile attackTile2 = Instantiate(attackTilePrefab, pos2, Quaternion.identity);
                attackTile2.transform.eulerAngles = new Vector3(90, 0, 0);
                attackTile2.Init(AttackTileDelay);
            }
            xPos++;
            yPos++;

            yield return new WaitForSeconds(UltimateSecondAttackDelay);
        }

        xPos = 7;
        yPos = 0;
        for (int i = 7; i > 3; i--)
        {
            for (int j = xPos; j > 3; j--)
            {
                Vector3 pos = TileManager.instance.BoardPositionToWorldPosition(new BoardPosition { XPos = j, YPos = yPos });
                AttackTile attackTile = Instantiate(attackTilePrefab, pos, Quaternion.identity);
                attackTile.transform.eulerAngles = new Vector3(90, 0, 0);
                attackTile.Init(AttackTileDelay);

                Vector3 pos2 = TileManager.instance.BoardPositionToWorldPosition(new BoardPosition { XPos = 7 - j, YPos = 7 - yPos });
                AttackTile attackTile2 = Instantiate(attackTilePrefab, pos2, Quaternion.identity);
                attackTile2.transform.eulerAngles = new Vector3(90, 0, 0);
                attackTile2.Init(AttackTileDelay);
            }
            xPos--;
            yPos++;

            yield return new WaitForSeconds(UltimateSecondAttackDelay);
        }

        currentAttackTimer = UltimateAttackTimer;
    }
}
