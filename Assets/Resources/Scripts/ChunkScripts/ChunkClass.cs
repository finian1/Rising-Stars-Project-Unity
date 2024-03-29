using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkClass : MonoBehaviour
{
    enum ChunkType
    {
        CUBE,
        CUBE_MOVING,
        PLAIN,
        ROOM
    }

    public GameObject[] obstacleArray;
    public GameObject playerObject;
    private int cellID;
    private int chunkPosX;
    private int chunkPosY;
    private ChunkType chunkType;
    protected float chunkSizeX = 10.0f;
    protected float chunkSizeY = 10.0f;
    private Color currentChunkColour;
    private Color newChunkColour;

    private bool fadingColour = false;
    private float colourChangeSpeed = 2.0f;
    private float currentColourFade = 0.0f;

    protected int finishedObstacles = 0;
    private bool activatedNavPoints = false;

    public List<GameObject> nodes = new List<GameObject>();

    private float enemySpawnChance = 2.0f;
    private bool spawnedEnemies = false;
    private float pickupSpawnChance = 10.0f;
    private bool spawnedPickups = false;

    private bool gotNodes = false;

    public MapController mapController;
    public Game gameController;

    private float safeDistToPlayer = 10.0f;
    private float spawnSizeCheck = 1.0f;
    private int maxSpawnsPerChunk = 2;

    private void Awake()
    {
        currentChunkColour = Color.white;
        SetObstacleColours(currentChunkColour);
    }
    private void Update()
    {
        if (fadingColour)
        {
            Color tempColor = Color.Lerp(currentChunkColour, newChunkColour, currentColourFade);
            SetObstacleColours(tempColor);
            //Debug.Log("Setting colour to: " + tempColor);
            currentColourFade += Time.deltaTime * colourChangeSpeed;
            //Debug.Log(currentColourFade);
            if (currentColourFade >= 1.0f)
            {
                //Debug.Log("Succesfully faded.");
                SetObstacleColours(newChunkColour);
                currentChunkColour = newChunkColour;
                fadingColour = false;
                currentColourFade = 0.0f;
            }
        }

        if (obstacleArray != null)
        {
            //Prepare for enemy spawns
            if (finishedObstacles >= obstacleArray.Length && !spawnedEnemies && gameController.IsGameInProgress())
            {
                SpawnEnemies(enemySpawnChance);
            }
            if (finishedObstacles >= obstacleArray.Length && !spawnedPickups && gameController.IsGameInProgress())
            {
                SpawnPickups();
            }
        }
        
    }

    public void SpawnEnemies(float spawnChance)
    {
        if (!gotNodes)
        {
            GetAllAccessibleNodes();
        }
        if (nodes.Count != 0)
        {
            int tempSpawns = 0;
            foreach(GameObject node in nodes)
            {
                if(Vector3.Distance(node.transform.position, playerObject.transform.position) > safeDistToPlayer && tempSpawns < maxSpawnsPerChunk)
                {
                    if (Random.Range(0, 100) < spawnChance)
                    {
                        tempSpawns++;
                        GameObject[] enemyPrefabs = mapController.enemyPrefabs;
                        GameObject temp = Instantiate(enemyPrefabs[Random.Range(0, enemyPrefabs.Length)], node.transform.position, transform.rotation);
                        temp.GetComponent<EnemyScript_Base>().player = playerObject;
                    }
                }
            }
        }
        spawnedEnemies = true;
    }

    private void SpawnPickups()
    {
        if (!gotNodes)
        {
            GetAllAccessibleNodes();
        }
        if (nodes.Count != 0)
        {
            if (Random.Range(0, 100) < pickupSpawnChance)
            {
                GameObject[] pickupPrefabs = mapController.pickupPrefabs;

                float pickupRayStart = 20.0f;
                float spawnHeight = 1.0f;
                Vector3 randNodePos = nodes[Random.Range(0, nodes.Count)].transform.position;
                Vector3 rayStartPos = new Vector3(randNodePos.x, randNodePos.y + pickupRayStart, randNodePos.z);
                RaycastHit hit;
                Physics.Raycast(rayStartPos, Vector3.down, out hit, Mathf.Infinity, ~(1 | 7));

                GameObject temp = Instantiate(pickupPrefabs[Random.Range(0, pickupPrefabs.Length)], new Vector3(hit.point.x, hit.point.y + spawnHeight, hit.point.z), transform.rotation);
            }
        }
        spawnedPickups = true;
    }

    //Gets all nodes within the chunk that are not overlapping any objects
    public void GetAllAccessibleNodes()
    {
        nodes.Clear();
        foreach (Transform obj in transform)
        {
            if (obj.CompareTag("Navigation") && Physics.OverlapSphere(obj.transform.position, spawnSizeCheck).Length == 0)
            {
                nodes.Add(obj.gameObject);
            }
            else
            {
                ObstacleScript temp = obj.GetComponent<ObstacleScript>();
                if (temp != null)
                {
                    nodes.AddRange(temp.GetNodes());
                }
            }
        }
    }

    //No longer used, initially linked all nodes together
    public void ActivateNodes()
    {
        foreach (GameObject obstacle in obstacleArray)
        {
            if (obstacle != null)
            {
                obstacle.GetComponent<ObstacleScript>().ActivateNodes();
            }
        }
        foreach(Transform node in transform)
        {
            if (node.CompareTag("Navigation"))
            {
                node.GetComponent<NavNodeScript>().LinkNode();
            }
        }
    }
    //Used to keep track of how many obstacles in the chunk have finished revealing
    public void FinishObstacleReveal()
    {
        finishedObstacles++;
    }
    //Fade the entire chunk to a new colour
    public void FadeToColour(Color newColour)
    {
        //Debug.Log("Trying to fade!");
        newChunkColour = newColour;
        fadingColour = true;
    }
    //Set all obstacles within the chunk to a certain colour
    public void SetObstacleColours(Color newColour)
    {
        foreach(Transform child in this.transform)
        {
            if (child.GetComponent<Renderer>() != null)
            {
                child.GetComponent<Renderer>().material.color = newColour;
            }
            if(child.childCount > 0)
            {
                SetObstacleColours(child, newColour);
            }
        }
    }
    public void SetObstacleColours(Transform objectToColor, Color newColour)
    {
        foreach (Transform child in objectToColor)
        {
            if (child.GetComponent<Renderer>() != null)
            {
                child.GetComponent<Renderer>().material.color = newColour;
            }
            if (child.childCount > 0)
            {
                SetObstacleColours(child, newColour);
            }
        }
    }

    public virtual void SpawnObjects() {

        

    }
    public void RevealObstacles()
    {
        if (obstacleArray != null)
        {
            for (int i = 0; i < obstacleArray.Length; i++)
            {
                if (obstacleArray[i] != null)
                {

                    obstacleArray[i].GetComponent<ObstacleScript>().BeginReveal(playerObject);
                }
            }
        }
    }


}
