
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Unity.Services.Core;
using Unity.Services.Authentication;
using Unity.Services.CloudSave;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System;
using Unity.VisualScripting;
using ABS_SaveLoadSystem;
using ABS_MobileCollectionGame_Shop;
using Unity.Mathematics;
using System.Collections;




public class PlayerManager : MonoBehaviour
{

    public static PlayerManager instance;
    public static SaveLoadManager saveLoadManager = new SaveLoadManager();

    public int journalingBasePoints = 20;

    public DragonNPCManager dragonInFocus;
    public ShopUIManger shopUIManger;
    public UIManager uiManager;

    public Sprite goldCoinIcon, productivityPointsIcon, productivityBonusIcon, JournalingStreakIcon, DragonsOwnedIcon, accessoriesIcon;




    // Start is called before the first frame update
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            shopUIManger =FindObjectOfType<ShopUIManger>();
            uiManager = FindObjectOfType<UIManager>(); // present in all scenes, just has basics
        }
        else
        {
            Destroy(gameObject);
        }

    }

    #region Update Player Data
    //Tasks
    public void AddTask(ToDoListItem item)
    {
        if (!saveLoadManager.playerData.ToDoListItems.Contains(item))
        {
            saveLoadManager.playerData.ToDoListItems.Add(item, nameof(saveLoadManager.playerData.ToDoListItems));

        }
    }
    internal void RemoveTask(ToDoListItem taskItem)
    {
        saveLoadManager.playerData.ToDoListItems.Remove(taskItem, nameof(saveLoadManager.playerData.ToDoListItems));
    }



    private async Task SetLastEnergyRefillAsync(DateTime refillTime)
    {
        await saveLoadManager.playerData.SetLastEnergyRefillAsync(refillTime);
    }


    public async Task<bool> UseEnergyToCompleteTask(ToDoListItem taskItem)
    {
        if (taskItem._priority <= saveLoadManager.playerData.EnergyPoints)
        {
            //if energy was maxed, use now as the countdown for the the next point refill
            if (saveLoadManager.playerData.EnergyPoints == saveLoadManager.playerData.EnergyCap) 
            {
                await saveLoadManager.playerData.SetLastEnergyRefillAsync(DateTime.UtcNow);
            }
             await saveLoadManager.playerData.SetEnergyPointsAsync(saveLoadManager.playerData.EnergyPoints - taskItem._priority);
            if(uiManager == null) { uiManager = FindObjectOfType<UIManager>(); }
            uiManager.UpdateEnergy();
            return true;
        }
        return false;
    }

    public async void GrantPointsForCompleteingTask(ToDoListItem taskItem)
    {

        Debug.Log("Priority: " + taskItem._priority);
        await AddProductivityPoints(taskItem._priority);
    }


    //Currencies
    public async void IncreaseEggPricePerCat()
    {
        await saveLoadManager.playerData.SetEggPriceAsync(saveLoadManager.playerData.EggPrice * 2);
    }
    public async void IncreaseAccesoryPrice()
    {
        await saveLoadManager.playerData.SetAccesoryPriceAsync(saveLoadManager.playerData.AccesoryPrice * 2);
    }



    public async Task ApplyProductivityBonus(int bonusMultiplier)
    {
        await saveLoadManager.playerData.SetProductivityMultiplierAsync(bonusMultiplier);
        await saveLoadManager.playerData.SetProductivityBonusExpiryAsync(DateTime.Now.AddDays(7));




        if(uiManager ==null) uiManager= FindObjectOfType<UIManager>();
        uiManager.UpdateProductivityBonusUI();


        uiManager.ShowConfirmationWindow(
                $"You Have purchased a Productivity bonus of {saveLoadManager.playerData.ProductivityMultiplier}X",
                $"It expires on {saveLoadManager.playerData.ProductivityBonusExpiry.DayOfWeek}",
                productivityBonusIcon
            );
    }

    public async Task AddProductivityPoints(int basePoints)
    {
        await CheckProductivityBonus();

        int totalPoints =(int) (basePoints * saveLoadManager.playerData.ProductivityMultiplier) + saveLoadManager.playerData.UnlockedDragons.Count;
        await saveLoadManager.playerData.SetProductivityPointsAsync(saveLoadManager.playerData.ProductivityPoints + totalPoints);


        if(uiManager ==null) uiManager= FindObjectOfType<UIManager>();
        uiManager.UpdateProductivityPointsUI();
    }

    public void AddNewDragon(DragonData dragon)
    {
        saveLoadManager.playerData.UnlockedDragons.Add(dragon, nameof(saveLoadManager.playerData.UnlockedDragons));


        if (uiManager == null) uiManager = FindObjectOfType<UIManager>();
        uiManager.UpdateDragonsOwnedUI();
    }



    #endregion



    public async void AddRandomDragon(bool freeEgg = false)
    {
        if (uiManager == null)
        {
            uiManager = FindObjectOfType<UIManager>();
        }
        //pick a random dragon
        int dragonID =DragonDatabase.instance.dragons[UnityEngine.Random.Range(0, DragonDatabase.instance.dragons.Length-1)].DragonID;


        DragonNPCScriptable newDragonScriptable = DragonDatabase.instance.getDragonByID(dragonID);//raplace this with a new DragonData object
        DragonData newDragon = new DragonData
        {
            Id = newDragonScriptable.DragonID,
            Name = newDragonScriptable.DragonName,
            // Set customization fields to defaults (if necessary)
            hatID = -1,
            backpackID = -1,
            petID = -1,
            holdingID = -1,
            rideableID = -1
        };

        //check if dragpn already owned
        bool unlockAccesoryInstead = false;
        if (saveLoadManager.playerData.UnlockedDragons.Contains(newDragon))
        {
            unlockAccesoryInstead = true;
        }

        if (!unlockAccesoryInstead)
        {
            saveLoadManager.playerData.UnlockedDragons.Add(newDragon, nameof(saveLoadManager.playerData.UnlockedDragons));

            //spawn new dragon
            NPCDragonSpawner.instance.SpawnDragonWithBrain(newDragon);

            //save new dragon

            uiManager.ShowConfirmationWindow($"Congrat's, you got {DragonDatabase.instance.getDragonByID(dragonID).DragonName}",
                                                $"Dragons Owned {saveLoadManager.playerData.UnlockedDragons.Count - 1} +1 !",
                                                DragonsOwnedIcon);
        }

        //dragon EGG ALREADY UNLOCKED 
        else
        {
            AddAccessories(true);
        }





        //PAY FOR EGG
        if (!freeEgg)
        {
            await saveLoadManager.playerData.SetProductivityPointsAsync(saveLoadManager.playerData.ProductivityPoints - saveLoadManager.playerData.EggPrice);
            
            IncreaseEggPricePerCat();

        }


        if (uiManager == null) uiManager= FindObjectOfType<UIManager>();
        uiManager.UpdateDragonsOwnedUI();
        uiManager.UpdateProductivityPointsUI();


    }

    //called on special events that grant specific dragon for free or what ever
    public void AddEventDragon(int id)
    {
        DragonNPCScriptable newDragonScriptable = DragonDatabase.instance.getDragonByID(id);
        DragonData newDragon = new DragonData
        {
            Id = newDragonScriptable.DragonID,
            Name = newDragonScriptable.DragonName,
            // Set customization fields to defaults (if necessary)
            hatID = -1,
            backpackID = -1,
            petID = -1,
            holdingID = -1,
            rideableID = -1
        };
        saveLoadManager.playerData.UnlockedDragons.Add(newDragon, nameof(saveLoadManager.playerData.UnlockedDragons));
        NPCDragonSpawner.instance.SpawnDragonWithBrain(newDragon);
        

        if (uiManager == null) uiManager = FindObjectOfType<UIManager>();
        uiManager.ShowConfirmationWindow($"Congrat's, you got {DragonDatabase.instance.getDragonByID(id).DragonName}",
                                    $"Dragons Owned {saveLoadManager.playerData.UnlockedDragons.Count - 1} +1 !",
                                    DragonsOwnedIcon);
        uiManager.UpdateProductivityPointsUI();
    }


    public async void AddAccessories(bool freeAccesory = false)
    {
        // Select a random accessory from the database
        DragonAccesory newAccesory = DragonDatabase.instance.accesories[UnityEngine.Random.Range(0, DragonDatabase.instance.accesories.Length)];
        AccesoryData newAccesoryData = new AccesoryData
        {

            accesory_id = newAccesory.accesory_id,
            quantityOwned = newAccesory.QuantityOwned
        };

        // Check if the accessory already exists in the unlocked list
        AccesoryData existingAccesory = saveLoadManager.playerData.UnlockedAccessories.Find(accessory => accessory.accesory_id == newAccesory.accesory_id);


        if (existingAccesory != null)
        {
            // Increase the quantity if the accessory is already owned
            existingAccesory.quantityOwned++;
        }
        else
        {
            // Add the new accessory to the collection
            newAccesory.QuantityOwned = 1; // Set the initial quantity
            saveLoadManager.playerData.UnlockedAccessories.Add(newAccesoryData, nameof(saveLoadManager.playerData.UnlockedAccessories));
        }

        // Deduct Productivity Points and increase accessory price if not free
        if (!freeAccesory)
        {
            await saveLoadManager.playerData.SetProductivityPointsAsync
                (saveLoadManager.playerData.ProductivityPoints - saveLoadManager.playerData.AccesoryPrice);
            IncreaseAccesoryPrice();
        }

        // Update UI with the new accessory details
        if (uiManager == null) uiManager = FindObjectOfType<UIManager>();
        uiManager.ShowConfirmationWindow(
            $"Congrats! You got the {newAccesory.accesory_name} accessory!",
            $"Accessories Owned: {saveLoadManager.playerData.UnlockedAccessories.Count}",
            accessoriesIcon
        );
        uiManager.UpdateProductivityPointsUI();
    }


    public async Task AddNewJournalEntry(JournalEnty newReading)
    {
        // Check the last journal entry date
        var lastEntryDate = saveLoadManager.playerData.LastJournalEntry;
        var newEntryDate = newReading.Entry_Date.Date; // Ensure only the date part is used

        saveLoadManager.playerData.Journal.Add(newReading, nameof(saveLoadManager.playerData.Journal));



        if (saveLoadManager.playerData.LastJournalEntry.DayOfYear == newEntryDate.DayOfYear &&
            saveLoadManager.playerData.LastJournalEntry.Year == newEntryDate.Year)
        {
            // If the journal entry is for the same day, do not allow spamming
            Debug.Log("Journal entry already added for today. Try again tomorrow!");
            return;
        }
        else
        {
            
            if (lastEntryDate.Date.AddDays(1) == newEntryDate)
            {
                // If journaling on the next consecutive day, increase the streak
                await saveLoadManager.playerData.SetJournalingStreakAsync(saveLoadManager.playerData.JournalingStreak+1);
                
            }
            else
            {
                // If a day is missed, reset the streak
                await saveLoadManager.playerData.SetJournalingStreakAsync(1);
            }
            
        }


        await saveLoadManager.playerData.SetLastJournalEntryAsync(newEntryDate);






    }

    public async Task<bool> CheckProductivityBonus()
    {
        if (saveLoadManager.playerData.ProductivityMultiplier > 1 && DateTime.Now > saveLoadManager.playerData.ProductivityBonusExpiry)
        {
            Debug.Log("Productivity Bonus Expired!"); // printing when this function is called
            //TO_DO pop-up to renue
            // Remove the bonus
            await saveLoadManager.playerData.SetProductivityMultiplierAsync( 1);




            if (uiManager == null) { uiManager= FindObjectOfType<UIManager>(); }
            uiManager.UpdateProductivityBonusUI();
            return false;
        }

        return true;
    }


    
   

}
