using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ABS_SaveLoadSystem
{
    [System.Serializable]
    public class PlayerData
    {


        // Example of data set up to be done for specialized projects in a child class
        /*
        public int GoldCoins
        {
            get => LoadData<int>(nameof(GoldCoins));
            set
            {
                SaveData(value, nameof(GoldCoins));
            }
        }
        */

        // Method for saving any field to the cloud
        protected async Task SaveData<T>(T value, string propertyName)
        {
            if (PlayerManager.saveLoadManager != null)
            {
                await PlayerManager.saveLoadManager.SavePlayerData(value, propertyName);
            }
            else
            {
                Console.WriteLine("SaveLoadManager not found! Make sure it's in the scene.");
            }
        }

        // Updated method for loading data asynchronously
        protected async Task<T> LoadData<T>(string propertyName, T defaultSetting = default)
        {
            if (PlayerManager.saveLoadManager != null)
            {
                // Await the result from LoadPlayerData
                return await PlayerManager.saveLoadManager.LoadPlayerData<T>(propertyName);
            }
            else
            {
                Console.WriteLine("SaveLoadManager not found! Make sure it's in the scene.");
                return defaultSetting;  // Return default value if the SaveLoadManager is not found
            }
        }

        // General Set Method for AutoSaveLists
        protected async Task<AutoSaveList<T>> LoadOrCreateAutoSaveList<T>(string propertyName)
        {
            var list = await LoadData<AutoSaveList<T>>(propertyName);
            return list ?? new AutoSaveList<T>();
        }

        // General Set Method for Simple Properties
        protected async Task SetAsync<T>(T value, string key, Action<T> setAction)
        {
            setAction(value);
            await SaveData(value, key);
        }
    }
}
