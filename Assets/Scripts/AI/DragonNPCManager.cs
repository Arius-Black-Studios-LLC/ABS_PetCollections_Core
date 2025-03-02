using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.AI;

public class DragonNPCManager : MonoBehaviour
{
    public DragonNPCState defaultState;
    public DragonNPCState[] allStates;

    public DragonNPCState currentstate;
    public NavMeshAgent agent;
    public int dragonID;

    public DragonAnimManager animManager;
    public DragonWardorbManager wardorbManager;

    public float stateTimer;
    public float counter;
    private void Awake()
    {
        allStates = GetComponents<DragonNPCState>();
        agent = GetComponent<NavMeshAgent>();
        currentstate = allStates[0];
        animManager = GetComponentInChildren<DragonAnimManager>();
    }

    private void FixedUpdate()
    {
        currentstate.PerformState(this);
        if (animManager != null)
        {
            animManager.SetAnimatorVelocity(agent.velocity);
        }
        else
        {
            animManager = GetComponentInChildren<DragonAnimManager>();
            animManager.SetAnimatorVelocity(agent.velocity);
        }


        counter += Time.deltaTime;
        if(counter > stateTimer)
        {
            List<DragonNPCState> viableStates = new List<DragonNPCState>();
            int weight = Random.Range(0, 100);
            foreach(DragonNPCState state in allStates)
            {
                if(state.minimumHour > state.maximumHour )//overnight
                {
                    if ((System.DateTime.Now.Hour >= state.minimumHour) || (System.DateTime.Now.Hour <= state.maximumHour))
                    {
                        if (state.weight_CommonHours >= weight)
                        {
                            viableStates.Add(state);
                        }
                    }
                    else
                    {
                        if (state.weight_uncommonHours >= weight)
                        {
                            viableStates.Add(state);
                        }
                    }
                }
                else//overnight
                {
                    if(System.DateTime.Now.Hour >= state.minimumHour && System.DateTime.Now.Hour <= state.maximumHour)
                    {
                        if (state.weight_CommonHours >= weight)
                        {
                            viableStates.Add(state);
                        }
                    }
                    else
                    {
                        if (state.weight_uncommonHours >= weight)
                        {
                            viableStates.Add(state);
                        }
                    }
                }
            }

            if (viableStates.Count > 0)
            {
                currentstate = viableStates[Random.Range(0, viableStates.Count - 1)];
                WakeUp();
            }

            counter = 0;


        }
    }
    public void WakeUp()
    {
        if (animManager != null)
        {
            animManager.SetSleepingFalse();
        }
        else
        {
            animManager = GetComponentInChildren<DragonAnimManager>();
            animManager.SetSleepingFalse();
        }

    }

    public void goToSleep()
    {
        if (animManager != null)
        {
            animManager.SetSleepingTrue();
        }
        else
        {
            animManager = GetComponentInChildren<DragonAnimManager>();
            animManager.SetSleepingTrue();
        }
    }
}
