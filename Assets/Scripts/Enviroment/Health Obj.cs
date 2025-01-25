using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthObj : MonoBehaviour
{
    public float detectionRadius = 5f;
    public playerController player;
    public float healtimer = 0.3f;
    public ManagerGen GenMan;

    private Coroutine healCoroutine;

    void Update()
    {
        if (player != null && Vector3.Distance(transform.position, player.transform.position) <= detectionRadius)
        {
            if (healCoroutine == null)
            {
                healCoroutine = StartCoroutine(HealPlayer());
            }
        }
        else
        {
            if (healCoroutine != null)
            {
                StopCoroutine(healCoroutine);
                healCoroutine = null;
                GenMan.isinhealtharea = false;
            }

            healCoroutine = null;
        }
    }

    private IEnumerator HealPlayer()
    {
        GenMan.isinhealtharea = true;

        if (player.health < 20)
        {
            player.health++;
        }

        yield return new WaitForSeconds(healtimer);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}

