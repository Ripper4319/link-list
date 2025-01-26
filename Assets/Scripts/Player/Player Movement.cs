using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerMovement : MonoBehaviour
{
   
    [Header("Camera")]
    Vector2 camRotation;
    public float mouseSensitivity = 2.0f;
    public float camRotationLimit = 90f;

    [Header("Player")]
    public Rigidbody myRB;
    public Camera playerCamera;
    public Vector3 respawnPos;
    public Quaternion zero;
    public int health = 20;
    public float speed = 5;
    public float jumpHeight = 6.5f;
    public Transform cam1;
    private LayerMask mask;
    private Vector3 velocity;
    public bool isgrap = false;

    [Header("Gun")]
    public LaserGun gun;

    [Header("Grapple Gun")]
    public GameObject Grapple;
    public GrapplingGun grap;

    void Start()
    {
        myRB = GetComponent<Rigidbody>();
        respawnPos = new Vector3(0, -2, 0);
        zero = Quaternion.identity;

        if (playerCamera != null)
        {
            playerCamera.transform.position = transform.position + new Vector3(0, 0.5f, 0);
            playerCamera.transform.parent = transform;
        }
    }

    void Update()
    {
        if (health <= 0)
        {
            transform.SetPositionAndRotation(respawnPos, zero);
            health = 20;
        }

        if (grap != null)
        {
            if (Input.GetMouseButtonDown(1))
            {
                if (!isgrap && !grap.isin5flimit) 
                {
                   grap.StartGrapple();
                   isgrap = true;
                }
                else
                {
                    grap.StopGrapple();
                    isgrap = false;
                }
            }
        }

        Stopgrap();

        if (Input.GetMouseButtonDown(0))
        {
            if (!isgrap)
            {
                gun.ShootLaser();
            }
        }

        if (Input.GetMouseButton(0))
        {
            if (isgrap && grap != null)
            {
                grap.PullPlayer();
            }
        }
        

        velocity = myRB.linearVelocity;

        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");

        Vector3 forward = transform.forward;
        Vector3 right = transform.right;

        Vector3 moveDirection = (right * horizontalInput + forward * verticalInput).normalized;

        velocity.x = moveDirection.x * speed;
        velocity.z = moveDirection.z * speed;

        camRotation.x += Input.GetAxisRaw("Mouse X") * mouseSensitivity * Time.timeScale;
        camRotation.y += Input.GetAxisRaw("Mouse Y") * mouseSensitivity * Time.timeScale;

        camRotation.y = Mathf.Clamp(camRotation.y, -camRotationLimit, camRotationLimit);

        transform.rotation = Quaternion.Euler(-camRotation.y, camRotation.x, 0);

        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded())
        {
            velocity.y = jumpHeight;
        }

        myRB.linearVelocity = velocity;
    }

    private bool IsGrounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, 1.1f);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name.Contains("Enemy"))
        {
            health--;
        }
    }


    public void Stopgrap()
    {
        if (grap.isin5flimit)
        {
            grap.StopGrapple();
            isgrap = false;
            grap.isin5flimit = false;
        }
       
    }
}

