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
                    nodeMemory.Add(currentNodeTarget);
                }
            }
        }
        else
        {
            currentNodeTarget = FindClosestNode();
        }
    }

    public GameObject FindClosestNode()
    {
        int layerMask = ~(1 << 7);
        Collider[] itemsToCheck = Physics.OverlapSphere(transform.position, nodeSearchRange, ~layerMask);
        itemsToCheck = itemsToCheck.OrderBy(c => (transform.position - c.transform.position).magnitude).ToArray();
        foreach(Collider item in itemsToCheck)
        {
            if (!Physics.Raycast(transform.position, item.transform.position - transform.position, (transform.position - item.transform.position).magnitude, layerMask))
            {
                return item.gameObject;
            }
        }
        return null;
    }

    private void GetPossibleNodes(ref List<GameObject> possibleNodes)
    {
        int layerMask = ~(1 << 7);
        Collider[] itemsToCheck = Physics.OverlapSphere(transform.position, nodeSearchRange, ~layerMask);
        foreach (Collider item in itemsToCheck)
        {
            if (!Physics.Raycast(transform.position, item.transform.position - transform.position, (transform.position - item.transform.position).magnitude, layerMask)
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
            float currentShortestDist = 1000.0f;
            GameObject currNode = null;
            foreach (GameObject node in possibleNodes)
            {
                float nodeDistToPlayer = (node.transform.position - player.transform.position).magnitude;
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
