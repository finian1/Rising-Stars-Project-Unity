using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkController : MonoBehaviour
{
    private int chunksPerCellX = 3;
    private int chunksPerCellY = 3;
    private float chunkSizeX = 10.0f;
    private float chunkSizeY = 10.0f;
    private ChunkClass[] chunkTypes =
    {
        new Chunk_Cubes(),
        new Chunk_Cube_Moving(),
        new Chunk_Plain(),
        new Chunk_Room(),
        new Chunk_Void()
    };
    private Cell[] cells;
    public GameObject chunkBase;
    public GameObject[] chunkObjects;
    public GameObject playerObject;
    public Board gameplayBoard;

    public void PopulateArrays(int widthX, int widthY)
    {
        cells = new Cell[widthX * widthY];
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
                cells[i].cellChunks[j].transform.localPosition = new Vector3(cells[i].GetX() + chunkColumn * chunkSizeX, 0,cells[i].GetY() + chunkRow * chunkSizeY);
                
            }
        }
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
}


public class Cell : MonoBehaviour
{
    private float triggerHeight = 20.0f;
    private int cellID;
    private float cellStartPosX;
    private float cellStartPosZ;
    private ChunkController controller;
    private GameObject cellTrigger;
    private Board gameplayBoard;
    public GameObject[] cellChunks;

    public float GetX()
    {
        return cellStartPosX;
    }
    public float GetY()
    {
        return cellStartPosZ;
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

    public void SetStartPosition(float X, float Z)
    {
        cellStartPosX = X;
        cellStartPosZ = Z;
        cellTrigger = new GameObject();
        cellTrigger.transform.SetParent(controller.transform);
        cellTrigger.AddComponent<BoxCollider>().isTrigger = true;
        Rigidbody rigidBody = cellTrigger.AddComponent<Rigidbody>();
        rigidBody.useGravity = false;
        rigidBody.constraints = RigidbodyConstraints.FreezeAll;
        cellTrigger.AddComponent<CellTriggerScript>().thisCell = this;
        float posShiftX = (controller.GetChunkSizeX() * controller.GetChunksPerCellX())/2 - (controller.GetChunkSizeX()/2);
        float posShiftZ = (controller.GetChunkSizeY() * controller.GetChunksPerCellY()) / 2 - (controller.GetChunkSizeY() / 2); ;
        cellTrigger.transform.localPosition = new Vector3(X + posShiftX, 0 , Z + posShiftZ);
        cellTrigger.GetComponent<BoxCollider>().size = new Vector3((controller.GetChunkSizeX() * controller.GetChunksPerCellX()), triggerHeight,(controller.GetChunkSizeY() * controller.GetChunksPerCellY()));
    }

    

    public void TriggerEntered(Collider other)
    {        if (other.CompareTag("Player"))
        {
            bool isDanger = gameplayBoard.CheckIfDanger(cellID);
            foreach (GameObject chunk in cellChunks)
            {
                ChunkClass chunkClass = chunk.GetComponent<ChunkClass>();

                chunkClass.SpawnObjects();
                if (isDanger)
                {
                    chunkClass.SetObstacleColours(new Color(1, 0, 0));
                }
                chunkClass.RevealObstacles();
                cellTrigger.SetActive(false);
            }
        }
    }

    
}