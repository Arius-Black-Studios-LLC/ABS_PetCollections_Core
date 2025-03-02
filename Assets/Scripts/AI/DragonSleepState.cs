using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DragonSleepState : DragonNPCState
{

    public bool IsSleeping;
    public DragonNPCManager sleepingBuddy;
    public float minDistance, maxDistance;

    public override void PerformState(DragonNPCManager manager)
    {
        if(IsSleeping)
        {
            //IF REACHED DESTINATION
            if(manager.agent.remainingDistance < manager.agent.stoppingDistance)
            {
                manager.goToSleep();
            }
        }
        else
        {
            FindAndSleepNearbyDragon(manager);
        }
    }

    private void FindAndSleepNearbyDragon(DragonNPCManager manager)
    {
        bool foundLocalDragon = false;
        //find all DRAGON NPCMANAGERS
        DragonNPCManager[] allDragons = FindObjectsOfType<DragonNPCManager>();
        foreach (DragonNPCManager dragon in allDragons)
        {
            if (dragon != manager)
            {
                //DragonSleepState otherSleepingDragon = (DragonSleepState)dragon.currentstate;
                if (dragon.currentstate is DragonSleepState)
                {
                        // find a spot near that dragon 
                        Vector3 dragonPos = dragon.transform.position;
                        float offsetX = Random.Range(-maxDistance, maxDistance);
                        float offsetZ = Random.Range(-maxDistance, maxDistance);
                        manager.agent.SetDestination(new Vector3 (dragonPos.x + offsetX , dragonPos.y, dragonPos.z + offsetZ));
                        foundLocalDragon = true;
                        sleepingBuddy = dragon;
                        IsSleeping = true;
                        return;

    
                }
            }
        }

        if(foundLocalDragon == false)
        {
            sleepingBuddy = null;
            manager.agent.SetDestination(transform.position + Random.insideUnitSphere * maxDistance);
            IsSleeping = true;
            return;
        }
    }


    public override void ResetState(DragonNPCManager manager)
    {
        IsSleeping = false;
        sleepingBuddy = null;
}


}
