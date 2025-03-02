using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public class EnergySoda : ScriptableObject
{
    public Sprite sodaIcon;
    public int energyPoints;


    public async Task<bool> FillPlayerEnergy()
    {
        if(PlayerManager.saveLoadManager.playerData.EnergyPoints >= PlayerManager.saveLoadManager.playerData.EnergyCap)
        {
            return false;
        }
        await PlayerManager.saveLoadManager.playerData.SetEnergyPointsAsync(
            Mathf.Min(energyPoints + PlayerManager.saveLoadManager.playerData.EnergyPoints, PlayerManager.saveLoadManager.playerData.EnergyCap)
            );

        return true;
    }
}
