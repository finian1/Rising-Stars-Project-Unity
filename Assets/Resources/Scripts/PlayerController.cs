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




    private Vector3 spawnPosition;
  
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        spawnPosition = gameObject.transform.position;
        //Cursor.lockState = CursorLockMode.Locked;
    }

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        
    }


    void FixedUpdate()
    {
        
        if (allowCameraMovement)
        {
            Jump();
            MoveCamera();
            MovePlayer();
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
        Vector3 directionVector = new Vector3(0, 0, 0);
        if (Input.GetAxisRaw("Vertical") != 0)
        {
            directionVector += transform.forward * Input.GetAxisRaw("Vertical");
        }
        if (Input.GetAxisRaw("Horizontal") != 0)
        {
            directionVector += transform.right * Input.GetAxisRaw("Horizontal");
        }
        directionVector.Normalize();
        rb.MovePosition(transform.position + directionVector * Time.deltaTime * movementSpeed);
        //rb.velocity = new Vector3(directionVector.x * Time.deltaTime * movementSpeed, rb.velocity.y, directionVector.z * Time.deltaTime * movementSpeed);

    }

    private void Jump()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            int layerMask = 1 << 8;
            layerMask = ~layerMask;
            RaycastHit hit;

            Debug.Log("Attempting Jump");
            if (Physics.Raycast(transform.position, -Vector3.up, out hit, 1.05f, layerMask) ||
                Physics.Raycast(new Vector3(transform.position.x + 0.5f, transform.position.y, transform.position.z + 0.5f), -Vector3.up, out hit, 1.05f, layerMask) ||
                Physics.Raycast(new Vector3(transform.position.x - 0.5f, transform.position.y, transform.position.z + 0.5f), -Vector3.up, out hit, 1.05f, layerMask) ||
                Physics.Raycast(new Vector3(transform.position.x + 0.5f, transform.position.y, transform.position.z - 0.5f), -Vector3.up, out hit, 1.05f, layerMask) ||
                Physics.Raycast(new Vector3(transform.position.x - 0.5f, transform.position.y, transform.position.z - 0.5f), -Vector3.up, out hit, 1.05f, layerMask)
                )
            {
                rb.velocity = new Vector3(rb.velocity.x, jumpForce, rb.velocity.z); ;
            }
        }

    }


}
