using System.Collections;
using UnityEngine;

public class LaserGun : MonoBehaviour
{
    public LineRenderer laserBeam;
    public Transform firePoint;
    public float laserRange = 50f;
    public float laserDuration = 0.1f;
    public LayerMask targetLayers;
    public int damage = 10;
    public Material laserMaterial; 

    private void Start()
    {
        if (laserMaterial != null && laserBeam.material == null)
        {
            laserBeam.material = laserMaterial;  
        }
    }

    public void ShootLaser()
    { 
        StartCoroutine(LaserEffect());

        RaycastHit hit;
        
        if (Physics.Raycast(firePoint.position, firePoint.forward, out hit, laserRange, targetLayers))
        {
            Debug.Log("Laser hit: " + hit.collider.name);

            ColorSetter target = hit.collider.GetComponent<ColorSetter>();
            if (target !=null)
            {


                StartCoroutine(target.DestroyTime());
            }

        }

       
    }

    public IEnumerator LaserEffect()
    {
        laserBeam.positionCount = 2;

        if (laserBeam.material != laserMaterial)
        {
            laserBeam.material = laserMaterial; 
        }

        laserBeam.SetPosition(0, firePoint.position);
        laserBeam.SetPosition(1, firePoint.position + firePoint.forward * laserRange);
        laserBeam.enabled = true;

        yield return new WaitForSeconds(laserDuration);

        laserBeam.enabled = false;
    }
}
