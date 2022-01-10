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

    [SerializeField] protected float movementSpeed;
    [SerializeField] protected WeaponStatHolderBase weaponStatHolder;
    [SerializeField] protected Weapon_Base enemyWeapon;
    [SerializeField] protected GameObject currentNodeTarget;
    [SerializeField] protected float nodeSearchRange;
    [SerializeField] public GameObject player;
    [SerializeField] protected List<GameObject> nodeMemory = new List<GameObject>();
    [SerializeField] protected int navLayer = 7;
    [SerializeField] protected int solidLayer = 6;
    [SerializeField] protected float enemyHealth = 10.0f;
    [SerializeField] protected GameObject scoreCrystalPrefab;
    [SerializeField] int numOfScoreCrystals = 5;
    [SerializeField] AudioClip destructionSound;

    private GameObject prevNode;
    private GameObject[] prevCheckDebug;
    private Collider[] fullPrevCheckDebug;

    private bool debug = true;

    private bool dead = false;

    private void OnDestroy()
    {
        CleanupScript.objectCache.Remove(this.gameObject);
        
        
    }

    public void DestroyEnemy()
    {
        if (destructionSound != null)
        {
            AudioSource.PlayClipAtPoint(destructionSound, transform.position, 1.0f);
        }
        Color thisColor = GetComponent<MeshRenderer>().material.color;
        if (scoreCrystalPrefab != null)
        {
            for (int i = 0; i < (float)numOfScoreCrystals * player.GetComponent<PlayerController>().currentCrystalMultiplier; i++)
            {
                GameObject crystalTemp = Instantiate(scoreCrystalPrefab, transform.position, transform.rotation);

                crystalTemp.GetComponent<MeshRenderer>().material.color = new Color(thisColor.r, thisColor.g, thisColor.b, crystalTemp.GetComponent<MeshRenderer>().material.color.a);
            }
        }
        Destroy(this.gameObject);
    }

    protected bool CanSeePlayer()
    {
        int layerMask = 1 << 6;
        int playerMask = 1 << 3;
        int enemyMask = 1 << 8;
        int multiMask = layerMask | playerMask | enemyMask;
        RaycastHit hit;
        if (Physics.Raycast(transform.position, player.transform.position-transform.position, out hit, Mathf.Infinity, multiMask))
        {
            if (hit.collider.gameObject.CompareTag("Player"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }


    private void Start()
    {
        CleanupScript.objectCache.Add(this.gameObject);
        currentNodeTarget = FindClosestNode();
        CreateWeaponStats();
    }
    protected virtual void Update()
    {
        if (debug)
        {
            DebugThis();
        }
    }

    private void TakeDamage(float val)
    {
        enemyHealth -= val;

        
        if(enemyHealth <= 0 && !dead)
        {
            PlayerStats.points += (int)PlayerStats.difficulty*10;
            dead = true;
            DestroyEnemy();
        }
    }

    protected void CreateWeaponStats()
    {
        if (enemyWeapon != null)
        {
            weaponStatHolder = new WeaponStatHolderBase();
            if (enemyWeapon.GetType() == typeof(Weapon_AssRifle))
            {
                CreateAssaultRifle(ref weaponStatHolder, PlayerStats.difficulty);
            }
            if(enemyWeapon.GetType() == typeof(Weapon_Shotgun))
            {
                CreateShotGun(ref weaponStatHolder, PlayerStats.difficulty);
            }
            if (enemyWeapon.GetType() == typeof(Weapon_MiniGun))
            {
                CreateMiniGun(ref weaponStatHolder, PlayerStats.difficulty);
            }
            enemyWeapon.init(weaponStatHolder);
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

    protected void GetPossibleNodes(ref List<GameObject> possibleNodes)
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

    protected GameObject PickAgressiveNode()
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

    protected GameObject PickRandomNode()
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

    protected void FireAtPlayer()
    {
        enemyWeapon.gameObject.transform.LookAt(player.transform.position, Vector3.up);
        enemyWeapon.FireWeapon(false);
    }

    protected void MoveTowards(GameObject node)
    {
        transform.position = Vector3.MoveTowards(transform.position, node.transform.position, movementSpeed * Time.deltaTime);
    }

    private void SetBaseStats(ref WeaponStatHolderBase wep, float difficulty,
        float minInaccuracy, float maxInaccuracy,
        float minRange, float maxRange,
        float minDamage, float maxDamage,
        float minFireRate, float maxFireRate,
        float shotWidth,
        float shotLifetime)
    {
        wep.shotDamage = Mathf.Clamp(minDamage + difficulty, minDamage, maxDamage);
        wep.range = Mathf.Clamp(minRange + difficulty, minRange, maxRange);
        wep.innacuracy = Mathf.Clamp(maxInaccuracy - difficulty, minInaccuracy, maxInaccuracy);
        wep.fireRate = Mathf.Clamp(minFireRate + difficulty, minFireRate, maxFireRate);

        wep.shotWidth = shotWidth;
        wep.shotLifetime = shotLifetime;
        
    }

    protected void CreateAssaultRifle(ref WeaponStatHolderBase wep, float difficulty)
    {
        wep.weaponType = typeof(Weapon_AssRifle);

        float minInaccuracy = 5;
        float maxInaccuracy = 90;

        float minRange = 20;
        float maxRange = 50;

        float minDamage = 5;
        float maxDamage = 20;

        float minFireRate = 4;
        float maxFireRate = 8;

        float shotWidth = 0.1f;
        float shotLifetime = 0.2f;

        SetBaseStats(ref wep, difficulty, 
            minInaccuracy, maxInaccuracy,
            minRange, maxRange,
            minDamage, maxDamage,
            minFireRate, maxFireRate,
            shotWidth,
            shotLifetime);
    }

    protected void CreateShotGun(ref WeaponStatHolderBase wep, float difficulty)
    {
        wep.weaponType = typeof(Weapon_Shotgun);

        float minInaccuracy = 30;
        float maxInaccuracy = 180;

        float minRange = 5;
        float maxRange = 20;

        float minDamage = 1;
        float maxDamage = 10;

        float minFireRate = 0.2f;
        float maxFireRate = 3;

        float shotWidth = 0.1f;
        float shotLifetime = 0.2f;

        int minShotCount = 5;
        int maxShotCount = 10;

        SetBaseStats(ref wep, difficulty,
            minInaccuracy, maxInaccuracy,
            minRange, maxRange,
            minDamage, maxDamage,
            minFireRate, maxFireRate,
            shotWidth,
            shotLifetime);

        wep.shotCount = (int)Mathf.Clamp(minShotCount + difficulty, minShotCount, maxShotCount);
    }

    protected void CreateMiniGun(ref WeaponStatHolderBase wep, float difficulty)
    {
        wep.weaponType = typeof(Weapon_Shotgun);

        float minInaccuracy = 30;
        float maxInaccuracy = 90;

        float minRange = 10;
        float maxRange = 50;

        float minDamage = 2;
        float maxDamage = 10;

        float minFireRate = 5;
        float maxFireRate = 10;

        float shotWidth = 0.1f;
        float shotLifetime = 0.2f;

        float minStartFireRate = 1;
        float maxStartFireRate = 4;

        float minTimeToRev = 4;
        float maxTimeToRev = 10;

        SetBaseStats(ref wep, difficulty,
            minInaccuracy, maxInaccuracy,
            minRange, maxRange,
            minDamage, maxDamage,
            minFireRate, maxFireRate,
            shotWidth,
            shotLifetime);

        wep.timeToRevUp = (int) Mathf.Clamp(maxTimeToRev - difficulty, minTimeToRev, maxTimeToRev);
        wep.startFireRate = (int) Mathf.Clamp(minStartFireRate + difficulty, minStartFireRate, maxStartFireRate);

    }

}
