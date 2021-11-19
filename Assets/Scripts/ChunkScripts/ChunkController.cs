using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkController : MonoBehaviour
{
    private int chunksPerCellX;
    private int chunksPerCellY;
    private ChunkClass[] chunkTypes =
    {
        new Chunk_Cubes(),
        new Chunk_Cube_Moving(),
        new Chunk_Plain(),
        new Chunk_Room()
    };
    private Cell[] cells;

    

}


public class Cell
{
    private int cellID;
    private GameObject[] cellChunks;
}