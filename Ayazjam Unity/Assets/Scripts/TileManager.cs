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

    private Vector3 Target;

    [SerializeField] private GameObject tilePrefab;
    public void Init()
    {
        instance = this;
        enemiesOnBoard = new List<EnemyBase>();
        GenerateTiles();
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
    }

    public void ShiftTileUp()
    {
        if (currentTileIndex == (TileCount+1) * 10.25f - 5)
        {
            return;
        }
        getEnemiesOnBoard();

        foreach (EnemyBase enemy in enemiesOnBoard)
        {
            if(enemy.boardPosition.YPos != 0)
            {
                enemy.boardPosition.YPos -= 1;
                enemy.selectedMovePos = enemy.transform.position + new Vector3(0, 0, -1.25f);
                enemy.ResetMovementTimer();

            }

        }

        Target = boardHolder.transform.position + new Vector3(0, 0, -1.25f);
        currentTileIndex++;
    }

    public void ShiftTileDown()
    {
        if (currentTileIndex == 0)
            return;
        getEnemiesOnBoard();

        foreach (EnemyBase enemy in enemiesOnBoard)
        {
            if(enemy.boardPosition.YPos == 7)
            { 
                //enemy can move iptal
            }
            enemy.boardPosition.YPos += 1;
            enemy.selectedMovePos = enemy.transform.position + new Vector3(0, 0, 1.25f);
            enemy.ResetMovementTimer();
        }

        Target = boardHolder.transform.position + new Vector3(0, 0, 1.25f);
        currentTileIndex--;
    }

    private void getEnemiesOnBoard()
    {
        enemiesOnBoard.Clear();
        Collider[] colliders = Physics.OverlapBox(transform.position, collisionBoxSize);
        foreach (Collider collider in colliders)
        {
            Debug.Log(collider.gameObject.name);
            if(collider.gameObject.layer == LayerMask.NameToLayer("Enemy"))
            {
                enemiesOnBoard.Add(collider.transform.parent.GetComponent<EnemyBase>());
            }
        }
    }

    public Vector3 BoardPositionToWorldPosition(BoardPosition boardPosition)
    {
        return bottomRight.transform.position + new Vector3(boardPosition.XPos * 1.30f, 0, boardPosition.YPos * 1.25f);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, collisionBoxSize);
    }
}
