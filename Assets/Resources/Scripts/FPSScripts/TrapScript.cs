using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapScript : MonoBehaviour
{
    [SerializeField] private GameObject[] trapBoarders = new GameObject[4];
    [SerializeField] private float trapRevealTime;
    [SerializeField] private float boarderThickness = 0.5f;
    [SerializeField] private float boarderHeight = 50.0f;
    [SerializeField] private float trapSize = 3.0f;
    [SerializeField] private float timeForTrap = 40.0f;
    [SerializeField] private float timeBetweenWaves = 10.0f;
    [SerializeField] private float trapEnemySpawnChances = 2.0f;

    private Cell[] neighbourCells;
    private Cell cellLink;

    void Start()
    {
        CleanupScript.objectCache.Add(this.gameObject);
    }

    private void OnDestroy()
    {
        PlayerStats.isInTrap = false;


        CleanupScript.objectCache.Remove(this.gameObject);
        foreach(GameObject boarder in trapBoarders)
        {
            Destroy(boarder);
        }
    }

    public void DeactivateTrap()
    {
        PlayerStats.isInTrap = false;
        foreach (Cell neighbourCell in neighbourCells)
        {
            neighbourCell.GetTriggerScript().TriggerIfPlayer();
            neighbourCell.SetToDefaultColour();
        }
    }

    public void ActivateTrap(Cell cell)
    {
        cellLink = cell;
        PlayerStats.isInTrap = true;
        float cellSizeX = cell.GetCellSizeX();
        float cellSizeZ = cell.GetCellSizeZ();

        Vector3[] boarderPos = new Vector3[4]
        {
            new Vector3(transform.position.x + cellSizeX*1.5f, transform.position.y, transform.position.z),
            new Vector3(transform.position.x - cellSizeX*1.5f, transform.position.y, transform.position.z),
            new Vector3(transform.position.x, transform.position.y, transform.position.z + cellSizeZ*1.5f),
            new Vector3(transform.position.x, transform.position.y, transform.position.z - cellSizeZ*1.5f),
        };
        Vector3[] boarderScale = new Vector3[4]
        {
            new Vector3(boarderThickness, boarderHeight, cellSizeZ*trapSize),
            new Vector3(boarderThickness, boarderHeight, cellSizeZ*trapSize),
            new Vector3(cellSizeX*trapSize, boarderHeight, boarderThickness),
            new Vector3(cellSizeX*trapSize, boarderHeight, boarderThickness),
        };


        for (int i = 0; i < trapBoarders.Length; i++)
        {
            trapBoarders[i].transform.position = boarderPos[i];
            trapBoarders[i].transform.localScale = boarderScale[i];
        }
        neighbourCells = cell.GetNeighbourCells();
        foreach(Cell neighbourCell in neighbourCells)
        {  
             neighbourCell.SetToDangerColour();
        }


        StartCoroutine(FadeObjectsIn());
        StartCoroutine(SpawnWaves(timeBetweenWaves));
        StartCoroutine(DeactivateByTimer(timeForTrap));
    }

    private void SpawnWave()
    {
        foreach (Cell neighbourCell in neighbourCells)
        {
            neighbourCell.SpawnEnemies(trapEnemySpawnChances);
        }
    }

    private IEnumerator SpawnWaves(float timeBetween)
    {
        float timer = 0.0f;
        while (true)
        {
            timer -= Time.deltaTime;
            if(timer <= 0.0f)
            {
                SpawnWave();
                timer = timeBetween;
            }
            yield return null;
        }
    }

    private IEnumerator DeactivateByTimer(float timeToDeactivate)
    {
        float timer = 0.0f;

        while(timer < timeToDeactivate)
        {
            timer += Time.deltaTime;
            yield return null;
        }
        DeactivateTrap();
        Destroy(gameObject);
    }

    private IEnumerator FadeObjectsIn()
    {
        float currentOpacity = 0.0f;
        

        while (currentOpacity < 1.0f)
        {
            foreach(GameObject boarder in trapBoarders)
            {
                Color boarderColor = boarder.GetComponent<Renderer>().material.color;
                boarder.GetComponent<Renderer>().material.color = new Color(boarderColor.r, boarderColor.g, boarderColor.b, currentOpacity);
            }
            currentOpacity += Time.deltaTime / trapRevealTime;
            yield return null;
        }
        
    }
}
