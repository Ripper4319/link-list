using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerMovement : MonoBehaviour, IDataPersistence
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
    public ManagerGen mangen;
    private bool grappleCheckRunning = false;
    public float grappleMinSpeed = 10f;
    public ParticleSystem speedLines;
    public float speedLinesMin = 20f;
    public float speedLinesMax = 40f;

    ParticleSystem.EmissionModule slEmission;
    ParticleSystem.VelocityOverLifetimeModule slVel;

    public Transform cameraHolder;

    public Transform speedLinesTransform;
    public float minVelocityForRotation = 1f;

    [Header("Gun")]
    public LaserGun gun;

    [Header("Grapple Gun")]
    public GameObject Grapple;
    public GrapplingGun grap;

    private bool grounded;

    void Start()
    {
        slEmission = speedLines.emission;
        slVel = speedLines.velocityOverLifetime;
        slEmission.rateOverTime = 0;

        transform.SetPositionAndRotation(respawnPos, zero);

        myRB = GetComponent<Rigidbody>();
        myRB.freezeRotation = true;
        respawnPos = new Vector3(0, 1, 38);
        zero = Quaternion.identity;

        if (playerCamera != null)
        {
            playerCamera.transform.SetParent(cameraHolder, false);
            playerCamera.transform.localPosition = new Vector3(0, 0.5f, 0);
        }

        isgrap = false;
        if (grap != null)
            grap.StopGrapple();

    }

    void Update()
    {
        UpdateSpeedLineDirection();

        HandleSpeedLines();

        if (health <= 0)
        {
            transform.SetPositionAndRotation(respawnPos, zero);
            health = 20;
            grap.StopGrapple();
            isgrap = false;

        }

        if (!mangen.Pausee)
        {
            HandleGrappling();
            HandleShooting();
            HandleCameraRotation();
        }
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

    public void LoadData(GameData data)
    {
        this.transform.position = data.playerPosition;
    }

    public void SaveData(GameData data)
    {
        data.playerPosition = this.transform.position;
    }

    private void ApplyMovement()
    {
        Vector3 move = new Vector3(velocity.x, 0f, velocity.z);
        move = Vector3.ProjectOnPlane(move, GetGroundNormal());

        if (grounded)
            myRB.AddForce(move * 10f, ForceMode.Acceleration);
        else
            myRB.AddForce(move * 6f, ForceMode.Acceleration);

        Vector3 flatVel = new Vector3(myRB.linearVelocity.x, 0, myRB.linearVelocity.z);
        if (flatVel.magnitude > speed)
        {
            Vector3 limited = flatVel.normalized * speed;
            myRB.linearVelocity = new Vector3(limited.x, myRB.linearVelocity.y, limited.z);
        }
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

                    if (!grappleCheckRunning)
                        StartCoroutine(GrappleSpeedCheck());
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

    private Vector3 GetGroundNormal()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, 1.5f))
            return hit.normal;

        return Vector3.up;
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

    private System.Collections.IEnumerator GrappleSpeedCheck()
    {
        grappleCheckRunning = true;
        yield return new WaitForSeconds(0.7f);

        float speed = myRB.linearVelocity.magnitude;

        if (speed < grappleMinSpeed && isgrap)
        {
            grap.StopGrapple();
            isgrap = false;
        }

        grappleCheckRunning = false;
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

    private void HandleSpeedLines()
    {
        float s = myRB.linearVelocity.magnitude;
        float t = Mathf.InverseLerp(speedLinesMin, speedLinesMax, s);

        var rate = slEmission.rateOverTime;
        rate.constant = Mathf.Lerp(0f, 200f, t);
        slEmission.rateOverTime = rate;
    }

    private void UpdateSpeedLineDirection()
    {
        Vector3 vel = myRB.linearVelocity;

        if (vel.sqrMagnitude < minVelocityForRotation * minVelocityForRotation)
            return;

        Vector3 dir = vel.normalized;

        Quaternion targetRot = Quaternion.LookRotation(dir, Vector3.up) * Quaternion.Euler(0, 180f, 0);
        speedLinesTransform.rotation = Quaternion.Slerp(
            speedLinesTransform.rotation,
            targetRot,
            Time.deltaTime * 10f
        );
    }



}