using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Linq;
using JetBrains.Annotations;
using ABS_SaveLoadSystem;

namespace ABS_MobileCollectionGame_Shop
{
    public class ShopUIManger : MonoBehaviour
    {
        [Header("UI")]
        public Button BuyDragon_ProductivtyPoints;
        public Button BuyAccesory_ProductivityPoints;


        [Header("Price Labels")]
        public TMP_Text DragonProductivityPrice_Text;
        public TMP_Text AccesoryProductivityPrice_Text;

        private void Awake()
        {
            FillStatsWindow(); // Initialize UI with current player stats
        }

        void Start()
        {
            // Button listeners for purchasing actions
            AssignButtonListener(BuyDragon_ProductivtyPoints, () => PurchaseDragonWithProductivityPoints());

            AssignButtonListener(BuyAccesory_ProductivityPoints, () => PurchaseAccessoryWithProductivityPoints());

        }

        private void OnEnable()
        {
            FillStatsWindow(); // Refresh UI when enabled
        }

        private void AssignButtonListener(Button button, UnityEngine.Events.UnityAction action)
        {
            button.onClick.AddListener(action); // Assign actions to buttons
        }

        // Purchase Dragon with Productivity Points
        private void PurchaseDragonWithProductivityPoints()
        {
            if (PlayerManager.saveLoadManager.playerData.ProductivityPoints >= PlayerManager.saveLoadManager.playerData.EggPrice)
            { 
                PlayerManager.instance.AddRandomDragon();
                FillStatsWindow(); // Update UI
            }
            else
            {
                Debug.LogWarning("Not enough Productivity Points!");
            }
        }



        // Purchase Accessory with Productivity Points
        private async void PurchaseAccessoryWithProductivityPoints()
        {
            if (PlayerManager.saveLoadManager.playerData.ProductivityPoints >= PlayerManager.saveLoadManager.playerData.AccesoryPrice)
            {
                // Deduct the cost and add the accessory
                await PlayerManager.saveLoadManager.playerData.SetProductivityPointsAsync(PlayerManager.saveLoadManager.playerData.ProductivityPoints - PlayerManager.saveLoadManager.playerData.AccesoryPrice);
                PlayerManager.instance.AddAccessories();
                FillStatsWindow(); // Update UI
            }
            else
            {
                Debug.LogWarning("Not enough Productivity Points!");
            }
        }

    

        // Updates UI with the current prices and player's balance
        private void FillStatsWindow()
        {
            // Set price labels

            AccesoryProductivityPrice_Text.text = PlayerManager.saveLoadManager.playerData.AccesoryPrice.ToString();
            DragonProductivityPrice_Text.text = PlayerManager.saveLoadManager.playerData.EggPrice.ToString();

            // Enable or disable purchase buttons based on player's balance
            BuyDragon_ProductivtyPoints.interactable = PlayerManager.saveLoadManager.playerData.ProductivityPoints >= PlayerManager.saveLoadManager.playerData.EggPrice;
            BuyAccesory_ProductivityPoints.interactable = PlayerManager.saveLoadManager.playerData.ProductivityPoints >= PlayerManager.saveLoadManager.playerData.AccesoryPrice;
        }
    }
}
