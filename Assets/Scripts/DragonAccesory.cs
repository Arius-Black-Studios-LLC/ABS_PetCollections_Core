using ABS_SaveLoadSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AccesoryType { Hat, Pet, Back, Hold, Rideable }

[CreateAssetMenu(menuName = "DragonCare/Accesory")]
public class DragonAccesory : ScriptableObject
{
    [Header("Accessory Info")]
    public int accesory_id;
    public string accesory_name;

    private int quantityOwned;
    public int QuantityOwned
    {
        get => quantityOwned;
        set
        {
            if (quantityOwned != value)
            {
                quantityOwned = value;
                NotifyAccesoryChanged();
            }
        }
    }

    public GameObject accesory_GO;
    public AccesoryType accesory_type;
    public Sprite icon;

    // Notify SaveLoadManager of changes to this accessory
    private void NotifyAccesoryChanged()
    {

        AccesoryData changedAccesory = PlayerManager.saveLoadManager.playerData.UnlockedAccessories
           .Find(accesory => accesory.accesory_id == this.accesory_id);

        if (changedAccesory != null)
        {
            // Update the customization data (like Hat_ID, Pet_ID, etc.) in DragonData
            changedAccesory.quantityOwned = this.quantityOwned;
;

            // Notify SaveLoadManager to save the updated list of dragons
            PlayerManager.saveLoadManager.SetChanges(changedAccesory, PlayerManager.saveLoadManager.playerData.UnlockedAccessories, nameof(PlayerManager.saveLoadManager.playerData.UnlockedAccessories));

            Debug.Log($"quantity for {this.accesory_name} has been updated.");
        }
        else
        {
            Debug.LogWarning($"quantity for  {this.accesory_name} not found in Unlocked Accesories list.");
        }
 
    }
}
