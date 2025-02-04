using System.Collections;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

public class MovingThing : MonoBehaviour
{
    public float doorOpenTime = 50f;
    public Vector3 openPosition;
    public Vector3 closedPosition;
    public float doorSpeed = 2f;
    public NavDisabler disabled;


    public void Start()
    {

    }

    public IEnumerator OpenDoorAfterDelay()
    {
        yield return new WaitForSeconds(2f);


        StartCoroutine(MoveDoor(openPosition));
        disabled.DisableAllAgents();


        yield return new WaitForSeconds(doorOpenTime);


        StartCoroutine(MoveDoor(closedPosition));
        disabled.EnableAllAgents();
    }

    private IEnumerator MoveDoor(Vector3 targetPosition)
    {
        while (Vector3.Distance(transform.localPosition, targetPosition) > 0.01f)
        {
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, targetPosition, doorSpeed * Time.deltaTime);
            yield return null;
        }
    }
}