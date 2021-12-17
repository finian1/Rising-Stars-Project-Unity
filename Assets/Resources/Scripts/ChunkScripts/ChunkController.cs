using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class ChunkController : MonoBehaviour
{
    private int numOfCellsX;
    private int numOfCellsY;
    private int chunksPerCellX = 3;
    private int chunksPerCellY = 3;
    private float chunkSizeX = 10.0f;
    private float chunkSizeY = 10.0f;
    private ChunkClass[] chunkTypes =
    {
        new Chunk_Cubes(),
        //new Chunk_Cube_Moving(),
        new Chunk_Plain(),
        new Chunk_Room(),
        new Chunk_Void()
    };
    private Cell[] cells;
    public GameObject chunkBase;
    public GameObject playerObject;
    public GameObject boarderTrigger;
    public Board gameplayBoard;
    public Vector3 FPSTopRightPosition;
    public Vector3 FPSBottomLeftPosition;
    public Vector3 playerPositionOnMinimap;
    public GameObject playerMarker;
    public GameObject boarderObject;
    public float boarderThickness;
    public float boarderHeight;
    private GameObject[] boarders;

    //Variables to get scale from map to board
    private float XMapSize;
    private float YMapSize;
    private float scaleX;
    private float scaleY;
    private Vector3 boardMidpoint;

    private void FixedUpdate()
    {
        UpdateMinimapPosition();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawSphere(boardMidpoint, 10.0f);
    }

    public void DestroyCells()
    {
        if (cells != null)
        {
            foreach (Cell cell in cells)
            {
                cell.DestroyCell();
                Destroy(cell);
            }
        }
        
    }

    public void SetCellSizes(int X, int Y)
    {
        chunksPerCellX = X;
        chunksPerCellY = Y;
    }

    private void UpdateMinimapPosition()
    {
        Vector3 playerPos = playerObject.transform.position - FPSBottomLeftPosition;
        playerPos = new Vector3(playerPos.x * scaleX, 0, playerPos.z * scaleY);
        //Debug.Log(playerPos);
        playerPositionOnMinimap = new Vector3(gameplayBoard.boardBottomLeftPosition.x + playerPos.x, gameplayBoard.boardBottomLeftPosition.y + playerPos.z, -5.0f);
        playerMarker.transform.position = playerPositionOnMinimap;
        playerMarker.transform.rotation = Quaternion.Euler(180, 0, (playerObject.transform.rotation.eulerAngles.y - 90.0f));
    }
    /// <summary>
    /// Populate all arrays and spawn in the board
    /// </summary>
    /// <param name="widthX"></param>
    /// <param name="widthY"></param>
    public void PopulateArrays(int widthX, int widthY)
    {
        Cursor.lockState = CursorLockMode.Locked;
        cells = new Cell[widthX * widthY];
        numOfCellsX = widthX;
        numOfCellsY = widthY;
        for(int i = 0; i < cells.Length; i++)
        {
            cells[i] = new Cell();
            //Set up cell positions
            int currIndex = i;
            int cellColumn = i % widthX;
            int cellRow = 0;

            while (currIndex >= widthX)
            {
                currIndex -= widthX;
                cellRow++;
            }
            cells[i].SetID(i);
            cells[i].SetBoard(gameplayBoard);
            cells[i].SetController(this);
            cells[i].SetStartPosition(-cellColumn * chunksPerCellX * chunkSizeX, cellRow * chunksPerCellY * chunkSizeY);

            //Set chunk positions
            cells[i].cellChunks = new GameObject[chunksPerCellX * chunksPerCellY];
            for(int j = 0; j < cells[i].cellChunks.Length; j++)
            {
                currIndex = j;
                int chunkColumn = j % chunksPerCellX;
                int chunkRow = 0;

                while(currIndex >= chunksPerCellX)
                {
                    currIndex -= chunksPerCellX;
                    chunkRow++;
                }

                cells[i].cellChunks[j] = Instantiate(chunkBase, transform);
                cells[i].cellChunks[j].AddComponent(chunkTypes[Random.Range(0, chunkTypes.Length)].GetType());
                cells[i].cellChunks[j].GetComponent<ChunkClass>().playerObject = playerObject;
                cells[i].cellChunks[j].transform.localPosition = new Vector3(cells[i].GetStartX() + chunkColumn * chunkSizeX, 0,cells[i].GetStartY() + chunkRow * chunkSizeY);
                
            }
        }

        FindCornerPositions(widthX, widthY);
        XMapSize = Mathf.Abs(FPSTopRightPosition.x - FPSBottomLeftPosition.x);
        YMapSize = Mathf.Abs(FPSTopRightPosition.z - FPSBottomLeftPosition.z);
        scaleX = gameplayBoard.XBoardSize / XMapSize;
        scaleY = gameplayBoard.YBoardSize / YMapSize;

        boarderTrigger.transform.localScale = new Vector3(XMapSize, 50, YMapSize);
        
        boardMidpoint = Vector3.Lerp(FPSTopRightPosition, FPSBottomLeftPosition, 0.5f);
        boarderTrigger.transform.position = boardMidpoint;
        SetBoarders();
    }

    private void FindCornerPositions(int widthX, int widthY)
    {
        float topRightX = cells[widthX - 1].GetPositionX() + (chunkSizeX * chunksPerCellX) / 2;
        float topRightY = cells[widthX - 1].GetPositionY();
        float topRightZ = cells[widthX - 1].GetPositionZ() + (chunkSizeY * chunksPerCellY) / 2;
        FPSTopRightPosition = new Vector3(topRightX, topRightY, topRightZ);

        float bottomLeftX = cells[widthX * widthY - widthX].GetPositionX() - (chunkSizeX * chunksPerCellX) / 2;
        float bottomLeftY = cells[widthX * widthY - widthX].GetPositionY();
        float bottomLeftZ = cells[widthX * widthY - widthX].GetPositionZ() - (chunkSizeY * chunksPerCellY) / 2;
        FPSBottomLeftPosition = new Vector3(bottomLeftX, bottomLeftY, bottomLeftZ);
    }

    private void SetBoarders()
    {
        boarders = new GameObject[4];
        for (int i = 0; i < boarders.Length; i++)
        {
            boarders[i] = Instantiate(boarderObject, transform);

        }
        boarders[0].transform.position = new Vector3(boardMidpoint.x, boardMidpoint.y, boardMidpoint.z + YMapSize / 2);
        boarders[0].transform.localScale = new Vector3(XMapSize, boarderHeight, boarderThickness);

        boarders[1].transform.position = new Vector3(boardMidpoint.x, boardMidpoint.y, boardMidpoint.z - YMapSize / 2);
        boarders[1].transform.localScale = new Vector3(XMapSize, boarderHeight, boarderThickness);

        boarders[2].transform.position = new Vector3(boardMidpoint.x + XMapSize / 2, boardMidpoint.y, boardMidpoint.z);
        boarders[2].transform.rotation = Quaternion.Euler(0, 90, 0);
        boarders[2].transform.localScale = new Vector3(YMapSize, boarderHeight, boarderThickness);

        boarders[3].transform.position = new Vector3(boardMidpoint.x - XMapSize / 2, boardMidpoint.y, boardMidpoint.z);
        boarders[3].transform.rotation = Quaternion.Euler(0, 90, 0);
        boarders[3].transform.localScale = new Vector3(YMapSize, boarderHeight, boarderThickness);



    }

    public Vector3 GetCellPosition(int ID)
    {
        return cells[ID].GetCellPosition();
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(FPSTopRightPosition, 1);
        Gizmos.DrawSphere(FPSBottomLeftPosition, 1);
    }
    public void RevealCell(int index)
    {
        cells[index].RevealCell();
    }
    public float GetChunkSizeX()
    {
        return chunkSizeX;
    }
    public float GetChunkSizeY()
    {
        return chunkSizeY;
    }
    public int GetChunksPerCellX()
    {
        return chunksPerCellX;
    }
    public int GetChunksPerCellY()
    {
        return chunksPerCellY;
    }
    public void GetBoardDimensions(ref int X, ref int Y)
    {
        X = numOfCellsX;
        Y = numOfCellsY;
    }
}


public class Cell : MonoBehaviour
{
    private int cellsX;
    private int cellsY;
    private Color hiddenColor = Color.white;
    private Color shownColor = Color.cyan;
    private float triggerHeight = 50.0f;
    private int cellID;
    private float cellStartPosX;
    private float cellStartPosZ;
    private ChunkController controller;
    private GameObject cellTrigger;
    private Board gameplayBoard;
    private bool revealed = false;
    private float cellSizeX;
    private float cellSizeZ;
    public GameObject[] cellChunks;
    Vector2Int[] _neighbours;

    public void DestroyCell()
    {
        Destroy(cellTrigger);
        foreach(GameObject chunk in cellChunks)
        {
            Destroy(chunk);
        }
    }

    public float GetCellSizeX()
    {
        return cellSizeX;
    }

    public float GetCellSizeZ()
    {
        return cellSizeZ;
    }

    public float GetStartX()
    {
        return cellStartPosX;
    }

    public float GetPositionX()
    {
        return cellTrigger.transform.position.x;
    }
    public float GetStartY()
    {
        return cellStartPosZ;
    }
    public Vector3 GetCellPosition()
    {
        return cellTrigger.transform.position;
    }

    public float GetPositionY()
    {
        return cellTrigger.transform.position.y;
    }

    public float GetPositionZ()
    {
        return cellTrigger.transform.position.z;
    }

    public void SetID(int id)
    {
        cellID = id;
    }
    public void SetController(ChunkController cont)
    {
        controller = cont;
    }
    public void SetBoard(Board gameBoard)
    {
        gameplayBoard = gameBoard;
    }

    public void SetCellColour(Color newColour)
    {
        foreach (GameObject chunk in cellChunks)
        {
            ChunkClass chunkClass = chunk.GetComponent<ChunkClass>();
            chunkClass.FadeToColour(newColour);
        }
    }

    public void SetStartPosition(float X, float Z)
    {
        float posShiftX = (controller.GetChunkSizeX() * controller.GetChunksPerCellX()) / 2 - (controller.GetChunkSizeX() / 2);
        float posShiftZ = (controller.GetChunkSizeY() * controller.GetChunksPerCellY()) / 2 - (controller.GetChunkSizeY() / 2);
        cellStartPosX = X;
        cellStartPosZ = Z;

        cellSizeX = (controller.GetChunkSizeX() * controller.GetChunksPerCellX());
        cellSizeZ = (controller.GetChunkSizeY() * controller.GetChunksPerCellY());

        cellTrigger = new GameObject();
        cellTrigger.transform.SetParent(controller.transform);
        cellTrigger.transform.localPosition = new Vector3(X + posShiftX, triggerHeight/2, Z + posShiftZ);
        cellTrigger.AddComponent<BoxCollider>().isTrigger = true;
        cellTrigger.AddComponent<CellTriggerScript>().thisCell = this;
        cellTrigger.GetComponent<BoxCollider>().size = new Vector3(cellSizeX, triggerHeight, cellSizeZ);
        
        Rigidbody rigidBody = cellTrigger.AddComponent<Rigidbody>();
        rigidBody.useGravity = false;
        rigidBody.constraints = RigidbodyConstraints.FreezeAll;
        
        
        
        controller.GetBoardDimensions(ref cellsX, ref cellsY);
        _neighbours = new Vector2Int[8]
        {
            new Vector2Int(-cellsX - 1, -1),
            new Vector2Int(-cellsX, -1),
            new Vector2Int(-cellsX + 1, -1),
            new Vector2Int(-1, 0),
            new Vector2Int(1, 0),
            new Vector2Int(cellsX - 1, 1),
            new Vector2Int(cellsX, 1 ),
            new Vector2Int(cellsX + 1, 1)
        };

    }

    public void RevealCell()
    {
        if (!revealed)
        {
         
            foreach (GameObject chunk in cellChunks)
            {
                ChunkClass chunkClass = chunk.GetComponent<ChunkClass>();

                chunkClass.SpawnObjects();

                chunkClass.SetObstacleColours(hiddenColor);
                
                chunkClass.RevealObstacles();
                revealed = true;
                
            }
        }
    }

    public void TriggerEntered(Collider other)
    {        
        if (other.CompareTag("Player"))
        {
            bool isDanger = gameplayBoard.CheckIfDanger(cellID);
            RevealCell();
            SetCellColour(shownColor);
            int boxRow = cellID / cellsX;
            for (int count = 0; count < _neighbours.Length; ++count)
            {
                int neighbourIndex = cellID + _neighbours[count].x;
                int expectedRow = boxRow + _neighbours[count].y;
                int neighbourRow = neighbourIndex / cellsX;
                if (expectedRow == neighbourRow && neighbourIndex >= 0 && neighbourIndex < cellsX*cellsY)
                {
                    controller.RevealCell(neighbourIndex);
                }
            }
            gameplayBoard.ClickBox(cellID);
            cellTrigger.SetActive(false);


        }
    }

    
}