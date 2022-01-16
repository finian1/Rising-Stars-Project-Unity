﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public Game gameController;
    private KeyCode mapKey = KeyCode.M;
    public GameObject playerCamera;
    public float maxVerticalRotation = 0;
    private float currentVerticalRotation = 0;

    public float rotationSpeed;
    public float movementSpeed;
    private float startSpeed;
    public float jumpForce;
    private float startJump;
    public float maxCameraMovement;

    public bool allowPlayerMovement = true;

    private Rigidbody rb;
    private CharacterController characterController;

    //private CharacterController controller;
    [SerializeField] private Vector3 playerVelocity;
    [SerializeField] private Vector3 currentForceVelocity;
    [SerializeField] private Material matToChange;
    private bool groundedPlayer;
    /*private float playerSpeed = 10.0f;
    private float jumpHeight = 1.0f;*/
    private float gravityValue = -20f;
    public KeyCode jumpKey = KeyCode.Space;
    private bool doubleJumped = false;

    private Vector3 spawnPosition;

    public GameObject weapon;
    public float weaponDamageScale = 1.0f;
    private float startWeaponDamageScale;
    public float weaponFireRateScale = 1.0f;
    private float startWeaponFireRateScale;
    public Material shotMaterial;
    private Weapon_Base currentWeaponScript;
    private float decell = 0.25f;
    public float currentCrystalMultiplier = 1.0f;
  
    void Start()
    {
        startJump = jumpForce;
        startSpeed = movementSpeed;
        startWeaponDamageScale = weaponDamageScale;
        startWeaponFireRateScale = weaponFireRateScale;


        startSpeed = movementSpeed;
        rb = GetComponent<Rigidbody>();
        characterController = GetComponent<CharacterController>();
        
        spawnPosition = gameObject.transform.position;
        //Cursor.lockState = CursorLockMode.Locked;
    }

    public void ResetMovement()
    {
        movementSpeed = startSpeed;
    }
    public void ResetJumpHeight()
    {
        jumpForce = startJump;
    }
    public void ResetWeaponDamage()
    {
        weaponDamageScale = startWeaponDamageScale;
    }
    public void ResetWeaponFireRate()
    {
        weaponFireRateScale = startWeaponFireRateScale;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Crystal"))
        {
            PlayerStats.currency += other.gameObject.GetComponent<CrystalScript>().crystalCost;
            other.gameObject.GetComponent<CrystalScript>().CollectItem();
        }
    }

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        allowPlayerMovement = true;
        EquipWeapon(PlayerStats.primaryWeapon);
    }
    private void Update()
    {
        if (Input.GetKey(mapKey) && allowPlayerMovement)
        {
            allowPlayerMovement = false;
            playerCamera.SetActive(false);
        }else if(!Input.GetKey(mapKey) && !allowPlayerMovement)
        {
            allowPlayerMovement=true;
            playerCamera.SetActive(true);
        }

        if (characterController.isGrounded)
        {
            doubleJumped = false;
        }
        
        if (allowPlayerMovement)
        {
            UpdatePlayer();
        }
        else
        {
            characterController.Move(Vector3.zero);
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            EquipWeapon(PlayerStats.primaryWeapon);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            EquipWeapon(PlayerStats.secondaryWeapon);
        }
        
    }

    void FixedUpdate()
    {
        
        if (allowPlayerMovement)
        {
            //Jump();
            //UpdatePlayer();
            MoveCamera();
            
            //MovePlayer();
            //characterController.Move(playerVelocity * Time.deltaTime);
            /*if (Input.GetKey(KeyCode.B))
            {
                allowCameraMovement = false;
                bookUI.SetActive(true);
                bookUI.GetComponent<UIController>().SetPage();
                Cursor.lockState = CursorLockMode.None;
            }*/

        }
        if (Input.GetMouseButton(0))
        {

            if(weapon.GetComponent<Weapon_Base>() != null)
            {
                weapon.GetComponent<Weapon_Base>().FireWeapon(true);
            }
        }
        else
        {
            if (weapon.GetComponent<Weapon_Base>() != null)
            {
                weapon.GetComponent<Weapon_Base>().StopFiring();
            }
        }

        

    }

    public void EquipWeapon(WeaponStatHolderBase newWeapon)
    {
        if(weapon.GetComponent<Weapon_Base>() != null)
        {
            Destroy(weapon.GetComponent<Weapon_Base>());
        }
        System.Type wepType = newWeapon.weaponType;

        Weapon_Base temp = weapon.AddComponent(newWeapon.weaponType) as Weapon_Base;
        currentWeaponScript = temp;
        temp.init(newWeapon);
        temp.SetupBase(playerCamera.GetComponent<Camera>(), shotMaterial);

        //Material[] mats = weapon.GetComponent<Renderer>().materials;
        //foreach(Material mat in mats)
        //{
        //    Debug.Log(mat.name);
        //    if(mat.name == nameOfMatToChange)
        //    {
        //        mat.SetColor("_EmissionColor", newWeapon.weaponColour);
        //        break;
        //    }
        //}
        matToChange.SetColor("_EmissionColor", newWeapon.weaponColour);

        currentCrystalMultiplier = newWeapon.crysMultiplier;
    }

    void UpdatePlayer()
    {
        groundedPlayer = characterController.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }

        Vector3 move = new Vector3((transform.forward * Input.GetAxisRaw("Vertical")).x + (transform.right * Input.GetAxisRaw("Horizontal")).x, 0, (transform.forward * Input.GetAxisRaw("Vertical")).z + (transform.right * Input.GetAxisRaw("Horizontal")).z);
        move.Normalize();
        playerVelocity.x = move.x * (movementSpeed + (PlayerStats.speedLevel * PlayerStats.buffPerLevel_Speed));
        playerVelocity.z = move.z * (movementSpeed + (PlayerStats.speedLevel * PlayerStats.buffPerLevel_Speed));
        playerVelocity += currentForceVelocity;

        //characterController.Move(move * Time.deltaTime * movementSpeed);

        /*if (move != Vector3.zero)
        {
            gameObject.transform.forward = move;
        }*/

        // Changes the height position of the player..
        if (Input.GetKeyDown(jumpKey) && groundedPlayer)
        {
            playerVelocity.y += Mathf.Sqrt(jumpForce * -3.0f * gravityValue);
        }else if(Input.GetKeyDown(jumpKey) && !doubleJumped)
        {
            doubleJumped = true;
            playerVelocity.y = Mathf.Sqrt(jumpForce * -3.0f * gravityValue);
        }
        //Debug.Log(doubleJumped);
        playerVelocity.y += gravityValue * Time.deltaTime;

        characterController.Move(new Vector3(playerVelocity.x, 0.0f, playerVelocity.z) * Time.deltaTime);
        characterController.Move(new Vector3(0.0f, playerVelocity.y, 0.0f) * Time.deltaTime);
        //playerVelocity -= move * movementSpeed;
        if (currentForceVelocity.magnitude > 0)
        {
            currentForceVelocity = Vector3.Lerp(currentForceVelocity, Vector3.zero, decell);
            if (currentForceVelocity.magnitude < 0.1f)
            {
                currentForceVelocity = Vector3.zero;
            }
        }

        //Debug.Log(currentForceVelocity);
        
    }

    private void TakeDamage(float val)
    {
        PlayerStats.health -= val;
        if(PlayerStats.health <= 0)
        {
            gameController.EndGame(false);
        }
    }

    public void ApplyForce(Vector3 force)
    {
        Debug.Log("Applied force: " + force);
        currentForceVelocity += force;
    }

    
    private void MoveCamera()
    {
        if (Input.GetAxis("Mouse X") != 0)
        {
            Quaternion rotation;
            rotation = Quaternion.Euler(0, Input.GetAxis("Mouse X") * Time.deltaTime * rotationSpeed, 0);
            //Debug.Log(Input.GetAxis("Mouse X"));
            rb.MoveRotation(rb.rotation * rotation);
    }
        if (Input.GetAxis("Mouse Y") != 0)
        {
            float camRotation;
            camRotation = -Input.GetAxis("Mouse Y") * Time.deltaTime * rotationSpeed;
            currentVerticalRotation = Mathf.Clamp(currentVerticalRotation + camRotation, -maxVerticalRotation, maxVerticalRotation);
            playerCamera.transform.localRotation = Quaternion.Euler(new Vector3(currentVerticalRotation, 0, 0));

        }
    }

    //private void MovePlayer()
    //{
    //    /*Vector3 directionVector = new Vector3(0, 0, 0);
    //    if (Input.GetAxisRaw("Vertical") != 0)
    //    {
    //        directionVector += transform.forward * Input.GetAxisRaw("Vertical");
    //    }
    //    if (Input.GetAxisRaw("Horizontal") != 0)
    //    {
    //        directionVector += transform.right * Input.GetAxisRaw("Horizontal");
    //    }
    //    directionVector.Normalize();
    //    //rb.MovePosition(transform.position + directionVector * Time.deltaTime * movementSpeed);
    //    //rb.velocity = new Vector3(directionVector.x * Time.deltaTime * movementSpeed, rb.velocity.y, directionVector.z * Time.deltaTime * movementSpeed);
    //    characterController.Move(directionVector * Time.deltaTime * movementSpeed);
    //    characterController.Move(new Vector3(0, gravityValue, 0) * Time.deltaTime);*/
    //    Vector3 move = new Vector3((transform.forward * Input.GetAxisRaw("Vertical")).x + (transform.right * Input.GetAxisRaw("Horizontal")).x, 0, (transform.forward * Input.GetAxisRaw("Vertical")).z + (transform.right * Input.GetAxisRaw("Horizontal")).z);
    //    move.Normalize();
    //    characterController.Move(move * Time.deltaTime * movementSpeed);
    //    playerVelocity.y += gravityValue * Time.deltaTime;
    //    characterController.Move(playerVelocity * Time.deltaTime);
    //}

    //private void Jump()
    //{
    //    float offsetX = 0.48f;
    //    float offsetZ = 0.48f;
    //    if (Input.GetKey(KeyCode.Space))
    //    {
    //        int layerMask = 1 << 8;
    //        layerMask = ~layerMask;
    //        RaycastHit hit;
    //        /*groundedPlayer = (Physics.Raycast(transform.position, -Vector3.up, out hit, 1.05f, layerMask) ||
    //            Physics.Raycast(new Vector3(transform.position.x + offsetX, transform.position.y, transform.position.z + offsetZ), -Vector3.up, out hit, 1.1f, layerMask) ||
    //            Physics.Raycast(new Vector3(transform.position.x - offsetX, transform.position.y, transform.position.z + offsetZ), -Vector3.up, out hit, 1.1f, layerMask) ||
    //            Physics.Raycast(new Vector3(transform.position.x + offsetX, transform.position.y, transform.position.z - offsetZ), -Vector3.up, out hit, 1.1f, layerMask) ||
    //            Physics.Raycast(new Vector3(transform.position.x - offsetX, transform.position.y, transform.position.z - offsetZ), -Vector3.up, out hit, 1.1f, layerMask)
    //            );*/

    //        groundedPlayer = characterController.isGrounded;
    //        /*if (groundedPlayer && playerVelocity.y < 0)
    //        {
    //            playerVelocity.y = 0f;
    //        }*/

    //        Debug.Log("Attempting Jump");
    //        Debug.Log(groundedPlayer);
    //        if (groundedPlayer)
    //        {
    //            playerVelocity.y += Mathf.Sqrt(jumpForce * -3.0f * gravityValue);
    //        }
    //    }
    //}


}
