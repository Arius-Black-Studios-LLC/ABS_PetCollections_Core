using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Services.CloudSave;
using Unity.Services.CloudSave.Models;
using UnityEngine;

namespace ABS_SaveLoadSystem
{
    public class SaveLoadManager
    {
        public DragonCarePlayerData playerData;

        public async Task LoadAllPlayerDataAsync()
        {
            // Initialize a new player data object asynchronously
            playerData = await DragonCarePlayerData.LoadAsync();
        }


        //used to check when an item that is part of an autosave
        public async void SetChanges<T>(T item, AutoSaveList<T> itemList, string propertyName) where T : class
        {
            try
            {
                // Ensure the item exists in the list
                foreach (T existingItem in itemList)
                {
                    if (existingItem == item)
                    {
                        // Resave the entire list
                        await SavePlayerData(itemList, propertyName);
                        Debug.Log($"Changes saved for {typeof(T).Name}: {item}");
                        return;
                    }
                }

                Debug.LogWarning($"{typeof(T).Name} item not found in {propertyName}.");
            }
            catch (Exception ex)
            {
                Debug.LogError($"Failed to save changes for {typeof(T).Name}: {ex.Message}");
            }
        }

        
        public async Task<T> LoadPlayerData<T>(string key, T defaultValue = default)
        {
            try
            {
                Dictionary<string, string> savedData = await CloudSaveService.Instance.Data.LoadAsync(new HashSet<string> { key });

                if (savedData.ContainsKey(key))
                {
                    string dataString = savedData[key];
                    Debug.Log("Data loaded: " + dataString);

                    T data = JsonConvert.DeserializeObject<T>(dataString);
                    return data;
                }
                else
                {
                    Debug.LogWarning($"Key '{key}' not found. Returning default value.");
                    return defaultValue;
                }
            }
            catch (Exception ex)
            {
                Debug.LogError($"Failed to load data for key '{key}': {ex.Message}");
                return defaultValue;
            }
        }

        public async Task SavePlayerData<T>(T inData, string key)
        {
            try
            {
                string jsonData = JsonConvert.SerializeObject(inData);
                var data = new Dictionary<string, object> { { key, jsonData } };
                Debug.Log("Data saved: " + jsonData);
                await CloudSaveService.Instance.Data.Player.SaveAsync(data);

            }
            catch (Exception ex)
            {
                Debug.LogError($"Failed to save data for key '{key}': {ex.Message}");
            }
        }

        public async Task<bool> KeyExists(string key)
        {
            try
            {
                Dictionary<string, string> savedData = await CloudSaveService.Instance.Data.LoadAsync(new HashSet<string> { key });
                return savedData.ContainsKey(key);
            }
            catch (Exception ex)
            {
                Debug.LogError($"Failed to check key '{key}': {ex.Message}");
                return false;
            }
        }
    }
}
