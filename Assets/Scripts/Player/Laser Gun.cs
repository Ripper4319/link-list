using System.Collections;
using UnityEngine;

public class LaserGun : MonoBehaviour
{
    [Header("Laser Beam")]
    public LineRenderer laserBeam;
    public Transform firePoint;
    public float laserRange = 50f;
    public float laserDuration = 0.1f;

    [Header("Damage / Interaction")]
    public LayerMask targetLayers;
    public int damage = 10;

    [Header("Laser Visuals")]
    public Material laserMaterial;
    public Transform cameraHolder;
    public ParticleSystem hitEffect;


    private void Start()
    {
        if (laserMaterial != null && laserBeam.material == null)
            laserBeam.material = laserMaterial;
    }

    public void ShootLaser()
    {
        cameraHolder.GetComponent<CameraShake>().Shake(0.3f);
        StartCoroutine(LaserEffect());

        RaycastHit hit;

        if (Physics.Raycast(firePoint.position, firePoint.forward, out hit, laserRange, targetLayers))
        {
            if (hitEffect != null)
            {
                ParticleSystem fx = Instantiate(hitEffect, hit.point, Quaternion.LookRotation(hit.normal));
                fx.Play();
                Destroy(fx.gameObject, 2f); 
            }

            ColorSetter target = hit.collider.GetComponent<ColorSetter>();
            if (target != null)
                StartCoroutine(target.DestroyTime());
        }
    }

    public IEnumerator LaserEffect()
    {
        laserBeam.positionCount = 2;

        if (laserBeam.material != laserMaterial)
            laserBeam.material = laserMaterial;

        laserBeam.SetPosition(0, firePoint.position);
        laserBeam.SetPosition(1, firePoint.position + firePoint.forward * laserRange);
        laserBeam.enabled = true;

        yield return new WaitForSeconds(laserDuration);

        laserBeam.enabled = false;
    }
}
