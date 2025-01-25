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

    void Awake()
    {
        lr = GetComponent<LineRenderer>();
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

    public Vector3 GetGrapplePoint()
    {
        return grapplePoint;
    }
}
