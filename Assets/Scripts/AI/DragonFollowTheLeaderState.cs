using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class DragonFollowTheLeaderState : DragonNPCState
{
    public DragonNPCManager leader;
    public float maxDistance = 20f;
    public float TimerMax = 10f;
    public float counter;

    public static int FollowLeaderGames = 0;
    public int FollowLeaderGameID;
    public int FollowLeaderPlayers = 0;
    public static int MaxPlayers = 5;

    public float searchRadius = 50f; // Radius to search for other dragons

    public override void PerformState(DragonNPCManager manager)
    {
        if (leader == null)
        {
            // Find dragons within the specified radius
            Collider[] hits = Physics.OverlapSphere(manager.transform.position, searchRadius);

            foreach (var hit in hits)
            {
                DragonNPCManager dragon = hit.GetComponent<DragonNPCManager>();
                if (dragon != null && dragon != manager)
                {
                    if (dragon.currentstate is DragonFollowTheLeaderState followLeaderState)
                    {
                        if (followLeaderState.leader != null && followLeaderState.FollowLeaderPlayers < MaxPlayers)
                        {
                            leader = followLeaderState.leader;
                            followLeaderState.FollowLeaderPlayers++;
                            FollowLeaderPlayers = followLeaderState.FollowLeaderPlayers;
                            FollowLeaderGameID = followLeaderState.FollowLeaderGameID;
                            break;
                        }
                    }
                }
            }

            // No leader found, become the leader
            if (leader == null)
            {
                leader = manager;
                FollowLeaderGames++;
                FollowLeaderGameID = FollowLeaderGames;
                FollowLeaderPlayers = 1;
            }
        }
        else
        {
            // Check if the leader is still valid
            if (leader.currentstate is not DragonFollowTheLeaderState)
            {
                leader = null;
                FollowLeaderPlayers = 0;
                FollowLeaderGames--;
            }
                
            else if (leader.currentstate is DragonFollowTheLeaderState followLeaderState)
            {
                FollowLeaderPlayers = followLeaderState.FollowLeaderPlayers;
            }
        }

        // Movement logic
        counter += Time.deltaTime;
        if (counter > TimerMax || manager.agent.remainingDistance <= manager.agent.stoppingDistance)
        {
            if (leader == manager) // If this dragon is the leader
            {
                Vector3 randomPosition = transform.position + Random.insideUnitSphere * maxDistance;
                NavMeshHit hit;
                if (NavMesh.SamplePosition(randomPosition, out hit, maxDistance, NavMesh.AllAreas))
                {
                    manager.agent.SetDestination(hit.position);
                }
            }
            else // If following a leader
            {
                manager.agent.SetDestination(leader.transform.position);
            }
            counter = 0;
        }
    }

    public override void ResetState(DragonNPCManager manager)
    {
        if (leader == manager)
        {
            FollowLeaderGames--;
        }
        leader = null;
        counter = 0;
        FollowLeaderGameID = 0;
        FollowLeaderPlayers = 0;
    }
}
