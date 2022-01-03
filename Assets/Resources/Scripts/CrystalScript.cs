using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalScript : MonoBehaviour
{
    [SerializeField] private float launchSpeed = 5.0f;
    [SerializeField] public int crystalCost = 10;
    [SerializeField] private AudioClip collectionClip;
    [SerializeField] private AudioClip collisionClip;
    // Start is called before the first frame update
    void Start()
    {
        gameObject.transform.rotation = Random.rotation;
        GetComponent<Rigidbody>().AddForce(transform.forward * launchSpeed);
    }

    private void OnDestroy()
    {
        if (collectionClip != null)
        {
            AudioSource.PlayClipAtPoint(collectionClip, gameObject.transform.position, 0.2f);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Solid"))
        {
            if (collisionClip != null)
            {
                AudioSource.PlayClipAtPoint(collisionClip, gameObject.transform.position, 0.2f);
            }
        }
    }

}
