using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Status_Base : MonoBehaviour
{
    public string buffName;
    [SerializeField] protected Color collectibleColour;
    [SerializeField] protected float durationOfEffect;
    [SerializeField] protected float strengthOfEffect;
    [SerializeField] protected GameObject internalIcon;

    private float buffTimer;
    private bool isActive = false;
    protected GameObject playerObject;
    protected PlayerController playerController;

    private void Start()
    {
        GetComponent<MeshRenderer>().material.color = collectibleColour;

    }

    private void Update()
    {
        if (isActive)
        {
            Tick();
        }
    }

    public virtual void StartBuff(GameObject player)
    {
        MeshRenderer renderer = GetComponent<MeshRenderer>();
        if(renderer != null)
        {
            renderer.enabled = false;
        }
        BoxCollider collider = GetComponent<BoxCollider>();
        if(collider != null)
        {
            collider.enabled = false;
        }
        if(internalIcon != null)
        {
            internalIcon.SetActive(false);
        }

        playerObject = player;
        playerController = playerObject.GetComponent<PlayerController>();
        isActive = true;
    }

    public virtual void Tick()
    {
        buffTimer += Time.deltaTime;
        if(buffTimer >= durationOfEffect)
        {
            EndBuff();
        }
    }

    protected virtual void EndBuff()
    {
        isActive = false;
        Destroy(transform.parent.gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            StartBuff(other.gameObject);
        }
    }
}
