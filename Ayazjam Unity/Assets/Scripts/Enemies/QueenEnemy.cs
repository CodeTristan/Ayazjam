using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QueenEnemy : EnemyBase
{
    public float UltimateSecondAttackDelay = 2f;
    public float UltimateAttackTimer = 5f;

    private void Start()
    {
        selectedMovePos = transform.position;
        CurrentHealth = MaxHealth;
        currentAttackTimer = AttackTimer;
        currentWalkTimer = WalkCoolDown;
        IsEvolved = true;
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
            StartCoroutine(Ultimate());
            currentAttackTimer = UltimateAttackTimer;
        }
    }

    public override IEnumerator Attack()
    {
        throw new System.NotImplementedException();
    }

    public override void Die()
    {
        //Maybe add a death animation
        Destroy(gameObject);
        TileManager.instance.enemiesOnBoard.Remove(this);
    }

    public override void Move()
    {
        throw new System.NotImplementedException();
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
        int random = Random.Range(0, 2);

        if (random == 0)
        {
            StartCoroutine(BishopUltimate());
        }
        else
        {
            StartCoroutine(RookUltimate());
        }
        yield return new WaitForSeconds(UltimateSecondAttackDelay);
    }


    private IEnumerator BishopUltimate()
    {
        currentAttackTimer = 10000;
        for (int i = 0; i < 8; i += 2)
        {
            for (int j = 0; j < 8; j += 2)
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
    private IEnumerator RookUltimate()
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

                Vector3 pos2 = TileManager.instance.BoardPositionToWorldPosition(new BoardPosition { XPos = 7 - j, YPos = 7 - yPos });
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
