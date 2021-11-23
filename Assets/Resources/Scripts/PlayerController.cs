using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public GameObject playerCamera;
    public float maxVerticalRotation = 0;
    private float currentVerticalRotation = 0;

    public float rotationSpeed;
    public float movementSpeed;
    public float jumpForce;
    public float maxCameraMovement;

    public bool allowCameraMovement = true;

    private Rigidbody rb;
    private CharacterController characterController;

    //private CharacterController controller;
    private Vector3 playerVelocity;
    private bool groundedPlayer;
    /*private float playerSpeed = 10.0f;
    private float jumpHeight = 1.0f;*/
    private float gravityValue = -20f;



    private Vector3 spawnPosition;
  
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        characterController = GetComponent<CharacterController>();
        
        spawnPosition = gameObject.transform.position;
        //Cursor.lockState = CursorLockMode.Locked;
    }

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        
    }
    private void Update()
    {
        groundedPlayer = characterController.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }

        Vector3 move = new Vector3((transform.forward * Input.GetAxisRaw("Vertical")).x + (transform.right * Input.GetAxisRaw("Horizontal")).x, 0, (transform.forward * Input.GetAxisRaw("Vertical")).z + (transform.right * Input.GetAxisRaw("Horizontal")).z);
        move.Normalize();
        characterController.Move(move * Time.deltaTime * movementSpeed);

        /*if (move != Vector3.zero)
        {
            gameObject.transform.forward = move;
        }*/

        // Changes the height position of the player..
        if (Input.GetButtonDown("Jump") && groundedPlayer)
        {
            playerVelocity.y += Mathf.Sqrt(jumpForce * -3.0f * gravityValue);
        }

        playerVelocity.y += gravityValue * Time.deltaTime;
        
    }

    void FixedUpdate()
    {
        
        if (allowCameraMovement)
        {
            Jump();
            MoveCamera();
            MovePlayer();
            characterController.Move(playerVelocity * Time.deltaTime);
            /*if (Input.GetKey(KeyCode.B))
            {
                allowCameraMovement = false;
                bookUI.SetActive(true);
                bookUI.GetComponent<UIController>().SetPage();
                Cursor.lockState = CursorLockMode.None;
            }*/

        }
    }






    
    private void MoveCamera()
    {
        if (Input.GetAxis("Mouse X") != 0)
        {
            Quaternion rotation;
            if(Input.GetAxis("Mouse X") >= maxCameraMovement)
            {
                rotation = Quaternion.Euler(0, maxCameraMovement * Time.deltaTime * rotationSpeed, 0);
                //rb.MoveRotation(Quaternion.Euler(0, gameObject.transform.rotation.y + maxCameraMovement * Time.deltaTime * rotationSpeed, 0));
            }else if(Input.GetAxis("Mouse X") <= -maxCameraMovement)
            {
                rotation = Quaternion.Euler(0, -maxCameraMovement * Time.deltaTime * rotationSpeed, 0);
                
            }
            else
            {
                rotation = Quaternion.Euler(0, Input.GetAxis("Mouse X") * Time.deltaTime * rotationSpeed, 0);
               
            }
            rb.MoveRotation(rb.rotation * rotation);
            
        }
        if (Input.GetAxis("Mouse Y") != 0)
        {
            if (Input.GetAxis("Mouse Y") >= maxCameraMovement)
            {
                currentVerticalRotation += maxCameraMovement;
            }
            else if (Input.GetAxis("Mouse Y") <= -maxCameraMovement)
            {
                currentVerticalRotation -= maxCameraMovement;
            }
            else
            {

                currentVerticalRotation += Input.GetAxis("Mouse Y");
            }
            float rotationCorrection = 0;

            if (currentVerticalRotation >= maxVerticalRotation)
            {
                rotationCorrection = currentVerticalRotation - maxVerticalRotation;
                currentVerticalRotation = maxVerticalRotation;
            }
            else if (currentVerticalRotation <= -maxVerticalRotation)
            {
                rotationCorrection = currentVerticalRotation + maxVerticalRotation;
                currentVerticalRotation = -maxVerticalRotation;
            }

            playerCamera.transform.Rotate(new Vector3(-(Input.GetAxis("Mouse Y") - rotationCorrection) * Time.deltaTime * rotationSpeed, 0, 0));

        }
    }

    private void MovePlayer()
    {
        /*Vector3 directionVector = new Vector3(0, 0, 0);
        if (Input.GetAxisRaw("Vertical") != 0)
        {
            directionVector += transform.forward * Input.GetAxisRaw("Vertical");
        }
        if (Input.GetAxisRaw("Horizontal") != 0)
        {
            directionVector += transform.right * Input.GetAxisRaw("Horizontal");
        }
        directionVector.Normalize();
        //rb.MovePosition(transform.position + directionVector * Time.deltaTime * movementSpeed);
        //rb.velocity = new Vector3(directionVector.x * Time.deltaTime * movementSpeed, rb.velocity.y, directionVector.z * Time.deltaTime * movementSpeed);
        characterController.Move(directionVector * Time.deltaTime * movementSpeed);
        characterController.Move(new Vector3(0, gravityValue, 0) * Time.deltaTime);*/
        Vector3 move = new Vector3((transform.forward * Input.GetAxisRaw("Vertical")).x + (transform.right * Input.GetAxisRaw("Horizontal")).x, 0, (transform.forward * Input.GetAxisRaw("Vertical")).z + (transform.right * Input.GetAxisRaw("Horizontal")).z);
        move.Normalize();
        characterController.Move(move * Time.deltaTime * movementSpeed);
        playerVelocity.y += gravityValue * Time.deltaTime;
        characterController.Move(playerVelocity * Time.deltaTime);
    }

    private void Jump()
    {
        float offsetX = 0.48f;
        float offsetZ = 0.48f;
        if (Input.GetKey(KeyCode.Space))
        {
            int layerMask = 1 << 8;
            layerMask = ~layerMask;
            RaycastHit hit;
            /*groundedPlayer = (Physics.Raycast(transform.position, -Vector3.up, out hit, 1.05f, layerMask) ||
                Physics.Raycast(new Vector3(transform.position.x + offsetX, transform.position.y, transform.position.z + offsetZ), -Vector3.up, out hit, 1.1f, layerMask) ||
                Physics.Raycast(new Vector3(transform.position.x - offsetX, transform.position.y, transform.position.z + offsetZ), -Vector3.up, out hit, 1.1f, layerMask) ||
                Physics.Raycast(new Vector3(transform.position.x + offsetX, transform.position.y, transform.position.z - offsetZ), -Vector3.up, out hit, 1.1f, layerMask) ||
                Physics.Raycast(new Vector3(transform.position.x - offsetX, transform.position.y, transform.position.z - offsetZ), -Vector3.up, out hit, 1.1f, layerMask)
                );*/

            groundedPlayer = characterController.isGrounded;
            /*if (groundedPlayer && playerVelocity.y < 0)
            {
                playerVelocity.y = 0f;
            }*/

            Debug.Log("Attempting Jump");
            Debug.Log(groundedPlayer);
            if (groundedPlayer)
            {
                playerVelocity.y += Mathf.Sqrt(jumpForce * -3.0f * gravityValue);
            }
        }

    }


}
