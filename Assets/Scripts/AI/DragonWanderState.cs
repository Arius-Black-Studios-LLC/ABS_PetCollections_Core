using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonWanderState : DragonNPCState
{
    public float wanderRadius = 5.0f;
    public float wanderSpeed = 2.0f;
    public float elapsedTime = 0f;

    private Vector3 randomDestination;
    public float nextWanderTime=2f;




    public override void PerformState(DragonNPCManager manager)
    {
        if (elapsedTime >= nextWanderTime)
        {
            SetNewRandomDestination();
            manager.agent.SetDestination(randomDestination);
            elapsedTime = 0f;
        }

        // Move towards the random destination
        elapsedTime += Time.deltaTime;
        

    }


    private void SetNewRandomDestination()
    {
        randomDestination = transform.position + Random.insideUnitSphere * wanderRadius;
        randomDestination.y = transform.position.y; // Keep dragons at the same height
        nextWanderTime = Random.Range(2f, 5.0f); // Random time between wander actions
    }

    public override void ResetState(DragonNPCManager manager)
    {
        elapsedTime = 0f;

    }
}
