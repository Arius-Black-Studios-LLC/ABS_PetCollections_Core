using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace ABS_SaveLoadSystem
{
    [System.Serializable]
    public class AutoSaveList<T> : List<T>
    {

        // Parameterless constructor required for deserialization
        public AutoSaveList() : base() { }



        // Override Add method to automatically save after adding an item
        public void Add(T item, string propertyName)
        {
            base.Add(item);
            // Call Save asynchronously
            SaveListAsync(propertyName).ConfigureAwait(false);
        }

        // Override Remove method to automatically save after removing an item
        public bool Remove(T item, string propertyName)
        {
            bool removed = base.Remove(item);
            if (removed)
            {
                // Call Save asynchronously
                SaveListAsync(propertyName).ConfigureAwait(false);
            }
            return removed;
        }

        // Override Clear method to automatically save after clearing the list
        public void Clear(string propertyName)
        {
            base.Clear();
            // Call Save asynchronously
            SaveListAsync(propertyName).ConfigureAwait(false);
        }

        // Method to save the list using SaveData
        private async Task SaveListAsync(string propertyName)
        {
            if (PlayerManager.saveLoadManager != null)
            {
                // Wait for the Save operation to complete (if necessary)
                await PlayerManager.saveLoadManager.SavePlayerData(this, propertyName);
            }
            else
            {
                Console.WriteLine("SaveLoadManager not found! Make sure it's in the scene.");
            }
        }
    }
}
