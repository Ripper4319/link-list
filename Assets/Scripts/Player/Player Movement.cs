using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerController : MonoBehaviour
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
    public int health = 3;
    public float speed = 5;
    public float jumpHeight = 6.5f;

    [Header("Gun")]
    public GameObject bullet;
    private Vector3 velocity;
    public float bulletSpeed = 5;
    public float bulletLifespan = .5f;

    [Header("Grapple Gun")]
    public GameObject Grapple;

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
            health = 3;
        }

        velocity = myRB.linearVelocity;

        velocity.x = Input.GetAxisRaw("Horizontal") * speed;
        velocity.z = Input.GetAxisRaw("Vertical") * speed;

        camRotation.x += Input.GetAxisRaw("Mouse X") * mouseSensitivity * Time.timeScale;
        camRotation.y += Input.GetAxisRaw("Mouse Y") * mouseSensitivity * Time.timeScale;

        camRotation.y = Mathf.Clamp(camRotation.y, -camRotationLimit, camRotationLimit);

        transform.rotation = Quaternion.Euler(-camRotation.y, camRotation.x, 0);

        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded())
        {
            velocity.y = jumpHeight;
        }

        if (Input.GetMouseButtonDown(0))
        {
            GameObject b = Instantiate(bullet, transform.position + transform.forward, Quaternion.identity);

            Physics.IgnoreCollision(b.GetComponent<Collider>(), GetComponent<Collider>());

            Vector3 lookPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            lookPos.y = transform.position.y;
            Vector3 direction = (lookPos - transform.position).normalized;

            b.GetComponent<Rigidbody>().linearVelocity = direction * bulletSpeed;
            Destroy(b, bulletLifespan);
        }

        myRB.linearVelocity = velocity;
    }

    private bool IsGrounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, 1.1f);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name.Contains("enemy"))
        {
            health--;
        }
    }
}

