using UnityEngine;
using UnityEngine.UI;

public class GrapplingGun : MonoBehaviour
{

    private LineRenderer lr;
    private Vector3 grapplePoint;
    public LayerMask whatIsGrappleable;
    public Transform gunTip, playercamera, player;
    private float maxDistance = 100f;
    private SpringJoint joint;
    public Sprite GRAP;
    public Sprite NONE;
    public bool colco = false;
    public float pullSpeed = 10f;
    public Rigidbody playerRigidbody;
    public Material grappleMaterial;
    public bool isin5flimit = false;
    public float grapplelimitclose = 5f;

    void Awake()
    {
        lr = GetComponent<LineRenderer>();
        if (grappleMaterial != null)
        {
            lr.material = grappleMaterial; 
        }
    }


    void LateUpdate()
    {
        DrawRope();
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

            if (!colco)
            {
                GameObject myImage = GameObject.Find("CROSSHAIR");
                Image imageComponent = myImage.GetComponent<Image>();
                imageComponent.sprite = GRAP;
                colco = true;
            }
        }
    }


    public void StopGrapple()
    {
        lr.positionCount = 0;
        Destroy(joint);
        
        if (colco)
        {
            GameObject myImage = GameObject.Find("CROSSHAIR");
            Image imageComponent = myImage.GetComponent<Image>();
            imageComponent.sprite = NONE;
            colco = false;
        }
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
        Vector3 directionToGrapplePoint = (grapplePoint - player.position).normalized;
        float distanceToGrapplePoint = Vector3.Distance(player.position, grapplePoint);

        if (distanceToGrapplePoint > grapplelimitclose)
        {
            playerRigidbody.AddForce(directionToGrapplePoint * pullSpeed, ForceMode.Acceleration);
        }
        else
        {
            isin5flimit = true;
        }
    }

    public Vector3 GetGrapplePoint()
    {
        return grapplePoint;
    }
}
