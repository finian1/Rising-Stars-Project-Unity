using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon_Base : MonoBehaviour
{
    [Header("Base Weapon Settings")]
    [SerializeField] protected string weaponNickname;
    [SerializeField] protected Transform firingOrigin;
    [SerializeField] protected float shotDamage;
    [SerializeField] protected float range;
    [SerializeField] protected float innacuracy;
    [SerializeField] protected float fireRate;
    [SerializeField] protected Camera playerCamera;
    [SerializeField] protected float shotLifetime;
    [SerializeField] protected float shotWidth;
    [SerializeField] protected Material shotMat;
    [SerializeField] protected AudioClip shotSound;
    [SerializeField] protected float crystalMultiplier;

    protected string weaponSoundName;

    


    protected float currentShotTimer;
    protected bool isFiring;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    //Loads the weapon sound based on the current weapon type
    private void LoadWeaponSound()
    {
        shotSound = Resources.Load<AudioClip>("Sounds/WeaponSounds/" + weaponSoundName);
    }

    public virtual void init(WeaponStatHolderBase stats)
    {
        LoadWeaponSound();
        weaponNickname = stats.weaponNickname;
        shotDamage = stats.shotDamage;
        range = stats.range;
        innacuracy = stats.innacuracy;
        fireRate = stats.fireRate;
        shotLifetime = stats.shotLifetime;
        shotWidth = stats.shotWidth;
        firingOrigin = transform.GetChild(0).transform;
        crystalMultiplier = stats.crysMultiplier;
    }

    public void SetupBase(Camera playerCam, Material shotMaterial)
    {
        playerCamera = playerCam;
        shotMat = shotMaterial;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        currentShotTimer += Time.deltaTime;
    }

    public virtual void FireWeapon(bool isFiredByPlayer, float damageScale = 1.0f, float fireRateScale = 1.0f)
    {

    }


    public virtual void StopFiring()
    {

    }

    public void FireHitscanShot(bool isPlayer, ref GameObject hitObject)
    {

        Vector3 tempFireDirection = new Vector3();
        int layerMask = 1 << 6;
        int playerMask = 1 << 3;
        int enemyMask = 1 << 8;
        int multiMask = layerMask | playerMask | enemyMask;
        float innacuracyScaled = innacuracy / 360;
        if (isPlayer)
        {
            //Get point that camera is looking at
            RaycastHit cameraHit;
            if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.TransformDirection(Vector3.forward), out cameraHit, Mathf.Infinity, layerMask | enemyMask)){
                //Debug.DrawRay(playerCamera.transform.position, playerCamera.transform.TransformDirection(Vector3.forward) * cameraHit.distance, Color.yellow);
                Debug.Log("Hit");
                tempFireDirection = (cameraHit.point - firingOrigin.transform.position).normalized;
                Debug.DrawRay(firingOrigin.transform.position, tempFireDirection * cameraHit.distance, Color.yellow);
            }
            else
            {
                tempFireDirection = playerCamera.transform.TransformDirection(Vector3.forward);
            }


            //Get directional vector from firingOrigin to the point
        }
        else
        {
            tempFireDirection = gameObject.transform.TransformDirection(Vector3.forward);

        }
        
        //Shift firing direction by random amount based on innacuracy
        tempFireDirection += new Vector3(Random.Range(-innacuracyScaled, innacuracyScaled), Random.Range(-innacuracyScaled, innacuracyScaled), Random.Range(-innacuracyScaled, innacuracyScaled));

        //Shoot a raycast out in firing direction and set hitObject to the object hit
        RaycastHit fireHit;
        
        if(Physics.Raycast(firingOrigin.transform.position, tempFireDirection, out fireHit, range, multiMask))
        {
            DrawLine(firingOrigin.transform.position, firingOrigin.transform.position + (tempFireDirection * fireHit.distance), Color.red);
            hitObject = fireHit.collider.gameObject;
            hitObject.SendMessage("TakeDamage", shotDamage, SendMessageOptions.DontRequireReceiver);
        }
        else
        {
            DrawLine(firingOrigin.transform.position, firingOrigin.transform.position + (tempFireDirection * range), Color.red);
        }
    }

    

    private void DrawLine(Vector3 start, Vector3 end, Color color)
    {
        if (shotSound != null)
        {
            AudioSource.PlayClipAtPoint(shotSound, transform.position, 0.1f);
        }
        GameObject myLine = new GameObject();
        myLine.transform.position = start;
        myLine.AddComponent<LineRenderer>();
        LineRenderer lr = myLine.GetComponent<LineRenderer>();
        lr.material = shotMat;
        lr.startColor = color;
        lr.endColor = color;
        lr.startWidth = shotWidth;
        lr.endWidth = shotWidth;
        lr.SetPosition(0, start);
        lr.SetPosition(1, end);
        myLine.AddComponent<DestructionScript>();
        myLine.GetComponent<DestructionScript>().Destruct(shotLifetime);
    }

}
