using UnityEngine;
using UnityEngine.UI;

public class GrapplingGun : MonoBehaviour
{
    private LineRenderer lr;
    private Vector3 grapplePoint;
    public LayerMask whatIsGrappleable;
    public Transform gunTip, playercamera, player;
    public float maxDistance = 100f;
    private SpringJoint joint;
    public Sprite GRAP;
    public Sprite NONE;
    public Sprite CAN_GRAPPLE;
    public bool colco = false;
    public float pullSpeed = 10f;
    public Rigidbody playerRigidbody;
    public Material grappleMaterial;
    public bool isin5flimit = false;
    public float grapplelimitclose = 5f;

    private Image crosshairImage;

    void Awake()
    {
        lr = GetComponent<LineRenderer>();
        if (grappleMaterial != null)
        {
            lr.material = grappleMaterial;
        }

        GameObject myImage = GameObject.Find("CROSSHAIR");
        if (myImage != null)
        {
            crosshairImage = myImage.GetComponent<Image>();
        }
    }

    void Update()
    {
        HandleCrosshair();
    }

    void LateUpdate()
    {
        DrawRope();
    }

    private void HandleCrosshair()
    {
        RaycastHit hit;
        bool canGrapple = Physics.Raycast(playercamera.position, playercamera.forward, out hit, maxDistance, whatIsGrappleable);

        if (IsGrappling())
        {
            crosshairImage.sprite = GRAP;
        }
        else if (canGrapple)
        {
            crosshairImage.sprite = CAN_GRAPPLE;
        }
        else
        {
            crosshairImage.sprite = NONE;
        }
    }

    public void StartGrapple()
    {
        RaycastHit hit;
        if (Physics.Raycast(playercamera.position, playercamera.forward, out hit, maxDistance, whatIsGrappleable))
        {
            grapplePoint = hit.point;
            joint = player.gameObject.AddComponent<SpringJoint>();
            joint.autoConfigureConnectedAnchor = false;
            joint.connectedAnchor = grapplePoint;

            float distanceFromPoint = Vector3.Distance(player.position, grapplePoint);
            joint.maxDistance = distanceFromPoint * 0.8f;
            joint.minDistance = distanceFromPoint * 0.25f;
            joint.spring = 7.5f;
            joint.damper = 7f;
            joint.massScale = 4.5f;

            if (lr.material != grappleMaterial)
            {
                lr.material = grappleMaterial;
            }

            lr.enabled = true;
            lr.positionCount = 2;
            currentGrapplePosition = gunTip.position;
            colco = true;
        }
    }

    public void StopGrapple()
    {
        lr.positionCount = 0;
        Destroy(joint);
        colco = false;
    }

    private Vector3 currentGrapplePosition;

    void DrawRope()
    {
        if (!joint) return;
        currentGrapplePosition = Vector3.Lerp(currentGrapplePosition, grapplePoint, Time.deltaTime * 8f);
        lr.SetPosition(0, gunTip.position);
        lr.SetPosition(1, currentGrapplePosition);
    }

    public bool IsGrappling()
    {
        return joint != null;
    }

    public void PullPlayer()
    {
        Vector3 directionToGrapplePoint = (grapplePoint - player.position);
        float distanceToGrapplePoint = directionToGrapplePoint.magnitude;

        if (distanceToGrapplePoint > grapplelimitclose)
        {
            playerRigidbody.linearVelocity = Vector3.Lerp(playerRigidbody.linearVelocity, directionToGrapplePoint.normalized * pullSpeed, Time.deltaTime * 5f);
        }
        else
        {
            isin5flimit = true;
            playerRigidbody.useGravity = true;
        }
    }

    public Vector3 GetGrapplePoint()
    {
        return grapplePoint;
    }
}
