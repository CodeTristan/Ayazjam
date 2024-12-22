using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BoardPosition
{
    [Range(0,7)]
    public int XPos;

    [Range(0,7)]
    public int YPos;
}
public class TileManager : MonoBehaviour
{
    public static TileManager instance;

    [SerializeField] private GameObject bottomRight;
    [SerializeField] private Transform boardHolder;

    public int TileCount;
    public Vector3 collisionBoxSize;
    public Vector2 GridMoveSize;
    public float moveSpeed; 

    public BoardPosition playerBoardPos;
    public int currentTileIndex = 0;

    public List<EnemyBase> enemiesOnBoard;
    public List<EnemyBase> allEnemies;

    private Vector3 Target;

    [SerializeField] private GameObject tilePrefab;
    public void Init()
    {
        instance = this;
        enemiesOnBoard = new List<EnemyBase>();
        allEnemies = new List<EnemyBase>();

        allEnemies.AddRange(FindObjectsOfType<EnemyBase>());
        GenerateTiles();
        getEnemiesOnBoard();
    }

    private void GenerateTiles()
    {
        for (int i = 0; i < TileCount; i++)
        {
            GameObject tile = Instantiate(tilePrefab, new Vector3(0, 0, i * 10.25f), Quaternion.identity);  
            tile.transform.eulerAngles = new Vector3(90, 0, 0);
            tile.transform.SetParent(boardHolder);
        }
    }

    private void Update()
    {
        if(Vector3.Distance(boardHolder.transform.position, Target) > 0.01f)
        {
            boardHolder.transform.position = Vector3.MoveTowards(boardHolder.position,Target,moveSpeed * Time.deltaTime);
        }
        else
        {
            boardHolder.transform.position = Target;
            Debug.Log(boardHolder.transform.position.z / GridMoveSize.y);
        }
    }

    public void ShiftTileUp()
    {
        if (currentTileIndex >= (TileCount-1) * 10.25f - 9)
        {
            return;
        }
        Target = boardHolder.transform.position + new Vector3(0, 0, -GridMoveSize.y);
        currentTileIndex++;

        getEnemiesOnBoard();

        foreach (EnemyBase enemy in allEnemies)
        {
            enemy.isActive = false;
            
            if (!enemy.IsEvolved && (enemy.IsEvolved && enemy.GetType() == typeof(QueenEnemy)))
            {
                if (enemiesOnBoard.Contains(enemy))
                {
                    enemy.boardPosition.YPos -= 1;
                    CheckIfEvolvedEnemyAhead(enemy);
                }
                enemy.selectedMovePos = enemy.transform.position + new Vector3(0, 0, -GridMoveSize.y);
                enemy.ResetMovementTimer();
            }

        }

        foreach (EnemyBase enemy in enemiesOnBoard)
        {
            enemy.isActive = true;
        }

        
    }

    public void ShiftTileDown()
    {
        if (currentTileIndex == 0)
            return;
        Target = boardHolder.transform.position + new Vector3(0, 0, GridMoveSize.y);
        currentTileIndex--;

        getEnemiesOnBoard();

        foreach (EnemyBase enemy in allEnemies)
        {
            if (!enemy.IsEvolved || (enemy.IsEvolved && enemy.GetType() == typeof(QueenEnemy)))
            {
                if (enemiesOnBoard.Contains(enemy))
                {
                    enemy.boardPosition.YPos += 1;
                }
                enemy.selectedMovePos = enemy.transform.position + new Vector3(0, 0, GridMoveSize.y);
                enemy.ResetMovementTimer();
            }

        }

        foreach (EnemyBase enemy in enemiesOnBoard)
        {
            enemy.isActive = true;
        }

    }

    public void getEnemiesOnBoard()
    {
        foreach (var item in enemiesOnBoard)
        {
            item.isActive = false;
        }
        enemiesOnBoard.Clear();
        Collider[] colliders = Physics.OverlapBox(transform.position, collisionBoxSize);
        foreach (Collider collider in colliders)
        {
            if(collider.gameObject.layer == LayerMask.NameToLayer("Enemy"))
            {
                enemiesOnBoard.Add(collider.transform.parent.GetComponent<EnemyBase>());
            }
        }
        foreach (var item in enemiesOnBoard)
        {
            item.isActive = true;
        }
    }

    public bool AnyEnemyPresent(BoardPosition position)
    {
        foreach (EnemyBase enemy in enemiesOnBoard)
        {
            if (enemy.boardPosition.XPos == position.XPos && enemy.boardPosition.YPos == position.YPos)
            {
                return true;
            }
        }

        return position.YPos == playerBoardPos.YPos && position.XPos == playerBoardPos.XPos;
    }

    public void CheckIfEvolvedEnemyAhead(EnemyBase current)
    {
        if (!current.isActive)
            return;

        if(current.boardPosition.YPos == 0)
        {
            current.IsEvolved = true;
            return;
        }
        foreach (EnemyBase enemy in enemiesOnBoard)
        {
            if(current.boardPosition.YPos - 1 == enemy.boardPosition.YPos && enemy.IsEvolved
                &&current.boardPosition.XPos == enemy.boardPosition.XPos)
            {
                current.IsEvolved = true;
            }
        }
    }

    public Vector3 BoardPositionToWorldPosition(BoardPosition boardPosition)
    {
        return bottomRight.transform.position + new Vector3(boardPosition.XPos * GridMoveSize.x, 0, boardPosition.YPos * GridMoveSize.y);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, collisionBoxSize);
    }
}
