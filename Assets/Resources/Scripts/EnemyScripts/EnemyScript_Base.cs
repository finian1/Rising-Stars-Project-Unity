using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyScript_Base : MonoBehaviour
{
    public enum EnemyStates
    {
        ReturningToNode,
        RandomPatrol,
        AgressivePatrol,
        AttackingPlayer
    }
    public EnemyStates currentState;

    [SerializeField] private float movementSpeed;
    [SerializeField] private Weapon_Base enemyWeapon;
    [SerializeField] private GameObject currentNodeTarget;
    [SerializeField] private float nodeSearchRange;
    [SerializeField] public GameObject player;
    [SerializeField] private List<GameObject> nodeMemory = new List<GameObject>();
    [SerializeField] private int navLayer = 7;
    [SerializeField] private int solidLayer = 6;

    private GameObject prevNode;
    private GameObject[] prevCheckDebug;
    private Collider[] fullPrevCheckDebug;

    private bool debug = true;


    private void Start()
    {
        currentNodeTarget = FindClosestNode();
    }
    private void Update()
    {
        if (currentNodeTarget != null)
        {
            MoveTowards(currentNodeTarget);
            Debug.DrawLine(transform.position, currentNodeTarget.transform.position, Color.green);
            if (transform.position == currentNodeTarget.transform.position)
            {
                if (player != null)
                {
                    currentNodeTarget = PickAgressiveNode();
                    if (currentNodeTarget != null)
                    {
                        nodeMemory.Add(currentNodeTarget);
                    }
                }
            }
        }
        else
        {
            currentNodeTarget = FindClosestNode();
        }
        if (debug)
        {
            DebugThis();
        }
    }

    private void DebugThis()
    {
        //Draw node memory
        if (nodeMemory.Count > 0)
        {
            for (int i = 0; i < nodeMemory.Count - 1; i++)
            {
                Debug.DrawLine(nodeMemory[i].transform.position, nodeMemory[i + 1].transform.position, Color.green);
            }
        }
        //Draw all possible nodes
        if (fullPrevCheckDebug != null)
        {
            for (int i = 0; i < fullPrevCheckDebug.Length; i++)
            {
                Debug.DrawLine(prevNode.transform.position, fullPrevCheckDebug[i].transform.position, Color.blue);
            }
        }
        //Draw all selected candidates
        if (prevCheckDebug != null)
        {
            for (int i = 0; i < prevCheckDebug.Length; i++)
            {
                Debug.DrawLine(prevNode.transform.position, prevCheckDebug[i].transform.position, Color.red);
            }
        }
    }

    public GameObject FindClosestNode()
    {
        int layerMask = 1 << navLayer;
        Collider[] itemsToCheck = Physics.OverlapSphere(transform.position, nodeSearchRange, layerMask);
        int solidLayerMask = 1 << solidLayer;
        itemsToCheck = itemsToCheck.OrderBy(c => (transform.position - c.transform.position).magnitude).ToArray();
        foreach(Collider item in itemsToCheck)
        {
            if (!Physics.Raycast(transform.position, item.transform.position - transform.position, (transform.position - item.transform.position).magnitude, solidLayerMask))
            {
                return item.gameObject;
            }
        }
        return null;
    }

    private void GetPossibleNodes(ref List<GameObject> possibleNodes)
    {
        int layerMask = 1 << navLayer;
        int solidLayerMask = 1 << solidLayer;
        Collider[] itemsToCheck = Physics.OverlapSphere(transform.position, nodeSearchRange, layerMask);
        fullPrevCheckDebug = itemsToCheck;

        foreach (Collider item in itemsToCheck)
        {
            if (!Physics.Raycast(transform.position, item.transform.position - transform.position, (transform.position - item.transform.position).magnitude, solidLayerMask)
                && item.gameObject != currentNodeTarget
                && !nodeMemory.Contains(item.gameObject))
            {
                possibleNodes.Add(item.gameObject);
            }
        }
    }

    private GameObject PickAgressiveNode()
    {
        
        List<GameObject> possibleNodes = new List<GameObject>();
        GetPossibleNodes(ref possibleNodes);
        if (possibleNodes.Count > 0)
        {
            prevNode = currentNodeTarget;
            float currentShortestDist = 1000.0f;
            GameObject currNode = null;
            prevCheckDebug = possibleNodes.ToArray();
            Debug.Log("---NEW AGRESSIVE NODE---");
            foreach (GameObject node in possibleNodes)
            {
                float nodeDistToPlayer = (node.transform.position - player.transform.position).magnitude;
                Debug.Log("Node distance to player: " + nodeDistToPlayer);

                if (nodeDistToPlayer < currentShortestDist)
                {
                    currNode = node;
                    currentShortestDist = nodeDistToPlayer;
                }
            }
            return currNode;
        }
        else
        {
            nodeMemory.Clear();
            return null;
        }
        
        
    }

    private GameObject PickRandomNode()
    {

        List<GameObject> possibleNodes = new List<GameObject>();
        GetPossibleNodes(ref possibleNodes);
        if (possibleNodes.Count > 0)
        {
            return possibleNodes[Random.Range(0, possibleNodes.Count)];
        }
        else
        {
            nodeMemory.Clear();
            return null;
        }

    }
    
    private void MoveTowards(GameObject node)
    {
        transform.position = Vector3.MoveTowards(transform.position, node.transform.position, movementSpeed * Time.deltaTime);
    }

}
