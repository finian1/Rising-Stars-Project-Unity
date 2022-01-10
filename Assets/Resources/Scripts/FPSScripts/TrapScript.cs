using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapScript : MonoBehaviour
{
    [SerializeField] private GameObject[] trapBoarders = new GameObject[4];
    [SerializeField] private float trapRevealTime;
    [SerializeField] private float boarderThickness = 0.5f;
    [SerializeField] private float boarderHeight = 50.0f;

    void Start()
    {
        CleanupScript.objectCache.Add(this.gameObject);
    }

    private void OnDestroy()
    {
        CleanupScript.objectCache.Remove(this.gameObject);
        foreach(GameObject boarder in trapBoarders)
        {
            Destroy(boarder);
        }
    }

    public void ActivateTrap(Cell cell)
    {
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
            new Vector3(boarderThickness, boarderHeight, cellSizeZ*3),
            new Vector3(boarderThickness, boarderHeight, cellSizeZ*3),
            new Vector3(cellSizeX*3, boarderHeight, boarderThickness),
            new Vector3(cellSizeX*3, boarderHeight, boarderThickness),
        };


        for (int i = 0; i < trapBoarders.Length; i++)
        {
            trapBoarders[i].transform.position = boarderPos[i];
            trapBoarders[i].transform.localScale = boarderScale[i];
        }
        StartCoroutine(FadeObjectsIn());
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
