using ABS_SaveLoadSystem;
using UnityEngine;

public enum DRAGONCategory { Any, D, R, A, G, O, N, S }

[CreateAssetMenu(menuName = "DragonCare/Dragon")]
[System.Serializable]
public class DragonNPCScriptable : ScriptableObject
{
    [Header("Dragon Info")]
    public int DragonID;
    public string DragonName;
    public GameObject DragonObject;

    [Header("Customizations")]
    private int hat_ID = -1;
    public int Hat_ID
    {
        get => hat_ID;
        set
        {
            if (hat_ID != value)
            {
                hat_ID = value;
                NotifyDragonChanged();
            }
        }
    }

    private int backpack_ID = -1;
    public int BackpackID
    {
        get => backpack_ID;
        set
        {
            if (backpack_ID != value)
            {
                backpack_ID = value;
                NotifyDragonChanged();
            }
        }
    }

    private int pet_ID = -1;
    public int PetID
    {
        get => pet_ID;
        set
        {
            if (pet_ID != value)
            {
                pet_ID = value;
                NotifyDragonChanged();
            }
        }
    }

    private int holding_ID = -1;
    public int HoldingID
    {
        get => holding_ID;
        set
        {
            if (holding_ID != value)
            {
                holding_ID = value;
                NotifyDragonChanged();
            }
        }
    }

    private int rideable_ID = -1;
    public int RideableID
    {
        get => rideable_ID;
        set
        {
            if (rideable_ID != value)
            {
                rideable_ID = value;
                NotifyDragonChanged();
            }
        }
    }

    // Notify SaveLoadManager of changes to this dragon
    private void NotifyDragonChanged()
    {
        DragonData changedDragon = PlayerManager.saveLoadManager.playerData.UnlockedDragons
           .Find(dragon => dragon.Id == this.DragonID);

        if (changedDragon != null)
        {
            // Update the customization data (like Hat_ID, Pet_ID, etc.) in DragonData
            changedDragon.hatID = this.Hat_ID;
            changedDragon.backpackID = this.BackpackID;
            changedDragon.petID = this.PetID;
            changedDragon.holdingID = this.HoldingID;
            changedDragon.rideableID = this.RideableID;

            // Notify SaveLoadManager to save the updated list of dragons
            PlayerManager.saveLoadManager.SetChanges(changedDragon, PlayerManager.saveLoadManager.playerData.UnlockedDragons, nameof(PlayerManager.saveLoadManager.playerData.UnlockedDragons));

            Debug.Log($"Dragon customization for {this.DragonName} has been updated.");
        }
        else
        {
            Debug.LogWarning($"Dragon with ID {this.DragonID} not found in UnlockedDragons list.");
        }

    }
}
