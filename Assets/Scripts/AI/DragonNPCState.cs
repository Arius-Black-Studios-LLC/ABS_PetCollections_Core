using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class DragonNPCState : MonoBehaviour
{

    public int minimumHour, maximumHour;
    public int weight_CommonHours;
    public int weight_uncommonHours;
    public abstract void PerformState(DragonNPCManager manager);
    public abstract void ResetState(DragonNPCManager manager);
}
