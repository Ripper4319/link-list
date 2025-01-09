using UnityEngine;

public class Enemy : MonoBehaviour
{
    private EnemyFSM fsm;

    void Start()
    {
        EnemyState idleState = new EnemyState(
            "Idle",
            () => Debug.Log("Enemy Entered Idle State"),
            () => Debug.Log("Enemy is Idling..."),
            () => Debug.Log("Enemy Exited Idle State")
        );

        EnemyState attackState = new EnemyState(
            "Attack",
            () => Debug.Log("Enemy Entered Attack State"),
            () => Debug.Log("Enemy is Attacking!"),
            () => Debug.Log("Enemy Exited Attack State")
        );

        EnemyState patrolState = new EnemyState(
            "Patrol",
            () => Debug.Log("Enemy Entered Patrol State"),
            () => Debug.Log("Enemy is Patrolling..."),
            () => Debug.Log("Enemy Exited Patrol State")
        );

        idleState.NextState = attackState;
        attackState.NextState = patrolState;
        patrolState.NextState = idleState;

        fsm = gameObject.AddComponent<EnemyFSM>();
        fsm.SetInitialState(idleState);
    }

    void Update()
    {
        fsm.UpdateFSM();
    }
}
