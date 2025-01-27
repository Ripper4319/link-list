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
    public float groundDrag = 6f;
    public float airMultiplier = 0.5f;
    public Transform cam1;
    private LayerMask mask;
    private Vector3 velocity;
    public bool isgrap = false;
    private bool readyToJump = true;
    public float jumpCooldown = 0.25f;

    [Header("Gun")]
    public LaserGun gun;

    [Header("Grapple Gun")]
    public GameObject Grapple;
    public GrapplingGun grap;

    private bool grounded;

    void Start()
    {
        myRB = GetComponent<Rigidbody>();
        myRB.freezeRotation = true;
        respawnPos = new Vector3(0, 1, 38);
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

        HandleGrappling();
        HandleShooting();
        HandleCameraRotation();

        grounded = IsGrounded();
        HandleDrag();
        ProcessMovement();
    }

    private void FixedUpdate()
    {
        ApplyMovement();
    }

    private void ProcessMovement()
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");

        Vector3 forward = transform.forward;
        Vector3 right = transform.right;

        Vector3 moveDirection = (right * horizontalInput + forward * verticalInput).normalized;

        if (grounded)
        {
            velocity.x = moveDirection.x * speed;
            velocity.z = moveDirection.z * speed;
        }
        else
        {
            velocity.x = moveDirection.x * speed * airMultiplier;
            velocity.z = moveDirection.z * speed * airMultiplier;
        }

        if (Input.GetKeyDown(KeyCode.Space) && readyToJump && grounded)
        {
            Jump();
            readyToJump = false;
            Invoke(nameof(ResetJump), jumpCooldown);
        }
    }

    private void ApplyMovement()
    {
        Vector3 flatVel = new Vector3(myRB.linearVelocity.x, 0f, myRB.linearVelocity.z);

        if (flatVel.magnitude > speed)
        {
            Vector3 limitedVel = flatVel.normalized * speed;
            myRB.linearVelocity = new Vector3(limitedVel.x, myRB.linearVelocity.y, limitedVel.z);
        }

        myRB.linearVelocity = new Vector3(velocity.x, myRB.linearVelocity.y, velocity.z);
    }

    private void HandleDrag()
    {
        if (grounded)
        {
            myRB.linearDamping = groundDrag;
        }
        else
        {
            myRB.linearDamping = 0;
        }
    }

    private void Jump()
    {
        myRB.linearVelocity = new Vector3(myRB.linearVelocity.x, 0f, myRB.linearVelocity.z);
        myRB.AddForce(Vector3.up * jumpHeight, ForceMode.Impulse);
    }

    private void ResetJump()
    {
        readyToJump = true;
    }

    private void HandleCameraRotation()
    {
        camRotation.x += Input.GetAxisRaw("Mouse X") * mouseSensitivity * Time.timeScale;
        camRotation.y += Input.GetAxisRaw("Mouse Y") * mouseSensitivity * Time.timeScale;

        camRotation.y = Mathf.Clamp(camRotation.y, -camRotationLimit, camRotationLimit);

        transform.rotation = Quaternion.Euler(0, camRotation.x, 0);
        playerCamera.transform.localRotation = Quaternion.Euler(-camRotation.y, 0, 0);
    }

    private void HandleGrappling()
    {
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

        StopGrapple();

        if (Input.GetMouseButton(0))
        {
            if (isgrap && grap != null)
            {
                grap.PullPlayer();
            }
        }
    }

    private void HandleShooting()
    {
        if (Input.GetMouseButtonDown(0) && !isgrap)
        {
            gun.ShootLaser();
        }
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

    public void StopGrapple()
    {
        if (grap.isin5flimit)
        {
            grap.StopGrapple();
            isgrap = false;
            grap.isin5flimit = false;
        }
    }
}
