using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthObj : MonoBehaviour
{
    [Header("Detection")]

    public float detectionRadius = 5f;
    public playerMovement player;
    public float healtimer = 0.3f;

    [Header("Manager")]

    public ManagerGen GenMan;

    [Header("State")]

    public bool istrigger = false;
    private Coroutine healCoroutine;

    [Header("Movement")]

    public MovingThing mT;


    void Update()
    {
        if (player != null && Vector3.Distance(transform.position, player.transform.position) <= detectionRadius)
        {
            if (healCoroutine == null)
            {
                healCoroutine = StartCoroutine(HealPlayer());
            }

            GenMan.isinhealtharea = true;
        }
        else
        {
            if (healCoroutine != null)
            {
                StopCoroutine(healCoroutine);
                healCoroutine = null;
                GenMan.isinhealtharea = false;
            }
        }
    }


    private IEnumerator HealPlayer()
    {

        while (player != null && Vector3.Distance(transform.position, player.transform.position) <= detectionRadius)
        {
            if (istrigger)
            {
                StartCoroutine(mT.OpenDoorAfterDelay());

                istrigger = false;
            }

            if (player.health < 20)
            {
                player.health++;
            }

            yield return new WaitForSeconds(healtimer);
        }

        GenMan.isinhealtharea = false;
        healCoroutine = null; 
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}