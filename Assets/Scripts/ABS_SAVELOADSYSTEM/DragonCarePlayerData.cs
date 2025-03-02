using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ABS_SaveLoadSystem;
using System;
using System.Threading.Tasks;
using static UnityEngine.Rendering.DebugUI.Table;


[System.Serializable]
public class DragonCarePlayerData : PlayerData
{
    // Properties with private setters
    public int GoldCoins { get; private set; }
    public int ProductivityPoints { get; private set; }
    public int ProductivityMultiplier { get; private set; }
    public DateTime ProductivityBonusExpiry { get; private set; }
    public int EggPrice { get; private set; }
    public int AccesoryPrice { get; private set; }
    public DateTime LastJournalEntry { get; private set; }
    public int JournalingStreak { get; private set; }
    public AutoSaveList<DragonData> UnlockedDragons { get; private set; }
    public AutoSaveList<ToDoListItem> ToDoListItems { get; private set; }
    public AutoSaveList<AccesoryData> UnlockedAccessories { get; private set; }
    public AutoSaveList<JournalEnty> Journal { get; private set; }
    public int EnergyPoints { get; private set; }
    public int EnergyCap { get; private set; }
    public int EnergySodas { get; private set; }
    public DateTime LastLogin { get; private set; }
    public DateTime LastEnergyRefill { get; private set; }




    // Asynchronous factory method to load data
    public static async Task<DragonCarePlayerData> LoadAsync()
    {
        var data = new DragonCarePlayerData();

        // Load all properties
        data.GoldCoins = await data.LoadData<int>(nameof(GoldCoins));
        data.ProductivityPoints = await data.LoadData<int>(nameof(ProductivityPoints));
        data.ProductivityMultiplier = await data.LoadData<int>(nameof(ProductivityMultiplier), 1);
        data.ProductivityBonusExpiry = await data.LoadData<DateTime>(nameof(ProductivityBonusExpiry), DateTime.MinValue);

        data.EggPrice = await data.LoadData<int>(nameof(EggPrice), 100);
        data.AccesoryPrice = await data.LoadData<int>(nameof(AccesoryPrice), 100);
        data.LastJournalEntry = await data.LoadData<DateTime>(nameof(LastJournalEntry), DateTime.MinValue);
        data.JournalingStreak = await data.LoadData<int>(nameof(JournalingStreak));
        data.EnergyCap = 100;
        data.EnergyPoints = await data.LoadData<int>(nameof(EnergyPoints), 100);
        data.EnergySodas = await data.LoadData<int>(nameof(EnergySodas));

        // Initialize and load AutoSaveList properties
        data.UnlockedDragons = await data.LoadOrCreateAutoSaveList<DragonData>(nameof(UnlockedDragons));
        data.ToDoListItems = await data.LoadOrCreateAutoSaveList<ToDoListItem>(nameof(ToDoListItems));
        data.UnlockedAccessories = await data.LoadOrCreateAutoSaveList<AccesoryData>(nameof(UnlockedAccessories));
        data.Journal = await data.LoadOrCreateAutoSaveList<JournalEnty>(nameof(Journal));


        data.LastLogin = await data.LoadData<DateTime>(nameof(LastLogin), DateTime.MinValue);
        if (data.LastLogin != DateTime.MinValue)
        {
            DateTime now = DateTime.Now;
            TimeSpan TimeSinceLastRefill = now - data.LastLogin;


            if (TimeSinceLastRefill.TotalDays > 1)
            {
                await data.SetJournalingStreakAsync(0);
                Debug.Log("Journaling streak reset due to inactivity.");
            }

        }
        data.LastEnergyRefill = await data.LoadData<DateTime>(nameof(LastEnergyRefill),DateTime.MinValue);
        if (data.LastEnergyRefill != DateTime.MinValue)
        {
            DateTime now = DateTime.Now;
            TimeSpan TimeSinceLastRefill = now - data.LastEnergyRefill;


            // Award free sodas for each missed week
            int missedDays = (int)TimeSinceLastRefill.TotalDays;
            if (missedDays > 7)
            {
                await data.SetEnergySodaAsync( data.EnergySodas + (int)( missedDays/7));
                Debug.Log($"Awarded {(int)(missedDays / 7)} free sodas for missed days.");
            }

            // Calculate energy recovery
            int minutesSinceLastRefill = (int)TimeSinceLastRefill.TotalMinutes;
            int recoveredEnergy = minutesSinceLastRefill / 9; // 1 energy point per 9 minutes
            int newEnergyPoints = Mathf.Min(data.EnergyPoints + recoveredEnergy, data.EnergyCap);

            await data.SetEnergyPointsAsync(newEnergyPoints);
            await data.SetLastEnergyRefillAsync(DateTime.Now);
        }
        else
        {
            await data.SetEnergyPointsAsync(data.EnergyCap);
            await data.SetLastEnergyRefillAsync(DateTime.Now);
        }
        await data.SetLastLoginAsync(DateTime.Now);

        return data;
    }


    // Example Specific Setter
    public async Task SetGoldCoinsAsync(int value)
    {
        await SetAsync(value, nameof(GoldCoins), v => GoldCoins = v);
    }

    public async Task SetProductivityPointsAsync(int value)
    {
        await SetAsync(value, nameof(ProductivityPoints), v => ProductivityPoints = v);
    }

    public async Task SetProductivityMultiplierAsync(int value)
    {
        await SetAsync(value, nameof(ProductivityMultiplier), v => ProductivityMultiplier = v);
    }

    public async Task SetProductivityBonusExpiryAsync(DateTime value)
    {
        await SetAsync(value, nameof(ProductivityBonusExpiry), v => ProductivityBonusExpiry = v);
    }

    public async Task SetEggPriceAsync(int value)
    {
        await SetAsync(value, nameof(EggPrice), v => EggPrice = v);
    }

    public async Task SetAccesoryPriceAsync(int value)
    {
        await SetAsync(value, nameof(AccesoryPrice), v => AccesoryPrice = v);
    }

    public async Task SetLastJournalEntryAsync(DateTime value)
    {
        await SetAsync(value, nameof(LastJournalEntry), v => LastJournalEntry = v);
    }

    public async Task SetJournalingStreakAsync(int value)
    {
        await SetAsync(value, nameof(JournalingStreak), v => JournalingStreak = v);
    }

    public async Task SetEnergyPointsAsync(int value)
    {
        await SetAsync(value, nameof(EnergyPoints), v => EnergyPoints = v);
    }

    public async Task SetEnergyCapAsync(int value)
    {
        await SetAsync(value, nameof(EnergyCap), v => EnergyCap = v);
    }
    public async Task SetEnergySodaAsync(int value)
    {
        await SetAsync(value, nameof(EnergySodas), v => EnergySodas  = v);
    }
    public async Task SetLastLoginAsync(DateTime value)
    {
        await SetAsync(value, nameof(LastLogin), v => LastLogin = v);
    }
    public async Task SetLastEnergyRefillAsync(DateTime value)
    {
        await SetAsync(value, nameof(LastEnergyRefill), v => LastEnergyRefill = v);
    }
}



[System.Serializable]
public class JournalEnty
{
    public DateTime Entry_Date { get; set; }
    public string Entry_Title { get; set; }
    public string Entry_Body { get; set;}
}
public class DragonData
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int hatID { get; set; }
    public int backpackID { get; set; }
    public int holdingID { get; set; }
    public int petID { get; set; }
    public int rideableID { get; set; }
}

public class AccesoryData
{
    public int accesory_id;

    public int quantityOwned;
}


