using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavNodeScript : MonoBehaviour
{
    public List<GameObject> connectedNodes;
    public float maxLinkDistance;
    int layerMask = ~(1 << 7);
    bool debugLines = false;

    private void Start()
    {
        
    }

    private void Update()
    {
        if (debugLines)
        {
            foreach (GameObject node in connectedNodes)
            {
                Debug.DrawLine(transform.position, node.transform.position, Color.red);
            }
        }
    }

    public GameObject PickRandomNode()
    {
        if (connectedNodes.Count > 0)
        {
            return connectedNodes[Random.Range(0, connectedNodes.Count)];
        }
        return null;
    }
        
    public void LinkNode()
    {
        Collider[] potentialNodes = Physics.OverlapSphere(transform.position, maxLinkDistance, ~layerMask);
        foreach(Collider collider in potentialNodes)
        {
            float currDist = (collider.transform.position - transform.position).magnitude;
            Debug.Log(collider.gameObject.tag);
            Debug.Log(currDist);
                
            if (!Physics.Raycast(transform.position, collider.transform.position-transform.position, currDist, layerMask))
            {
                NavNodeScript otherNodeScript = collider.GetComponent<NavNodeScript>();
                if (!connectedNodes.Contains(collider.gameObject))
                {
                    connectedNodes.Add(collider.gameObject);
                }
                if (!otherNodeScript.connectedNodes.Contains(gameObject))
                {
                    otherNodeScript.connectedNodes.Add(gameObject);
                }
            }
            
        }
    }

}
