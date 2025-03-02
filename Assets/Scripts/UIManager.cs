using UnityEngine;
using TMPro;
using UnityEngine.UI;
using ABS_SaveLoadSystem;

public class UIManager : MonoBehaviour
{
    [Header("UI References")]
    public TextMeshProUGUI goldCoinsText;
    public TextMeshProUGUI productivityPointsText;
    public TextMeshProUGUI dragonsOwnedText;
    public TextMeshProUGUI productivityBonusText;

    public TextMeshProUGUI journalingStreakText; // Drag your UI Text here in the Inspector


    [Header("Confirmation Window")]
    public GameObject ConformationWindowGO;
    public TMP_Text ConfirmationDescriptionText;
    public TMP_Text ConfirmationCurrencyChangeText;
    public Image NewItemSprite;

    private void Start()
    {
        InitializeUI();
        
    }

    public void InitializeUI()
    {
        if (PlayerManager.instance != null)
        {

            UpdateEnergy();
            UpdateJournalingStreakUI();
            UpdateProductivityPointsUI();
            UpdateProductivityBonusUI();
            UpdateDragonsOwnedUI();

        }
    }
    public void UpdateJournalingStreakUI()
    {
        journalingStreakText.text = $"{PlayerManager.saveLoadManager.playerData.JournalingStreak}";
    }

    public void UpdateEnergy()
    {
        if (goldCoinsText != null)
        {
            goldCoinsText.text = $"{PlayerManager.saveLoadManager.playerData.EnergyPoints}/{PlayerManager.saveLoadManager.playerData.EnergyCap}";
        }   
    }

    public void UpdateProductivityPointsUI()
    {
        if (productivityPointsText != null)
        {
            productivityPointsText.text = $"{PlayerManager.saveLoadManager.playerData.ProductivityPoints}";
        }
    }

    public void UpdateDragonsOwnedUI()
    {
        if (dragonsOwnedText != null)
        {
            dragonsOwnedText.text = $"{PlayerManager.saveLoadManager.playerData.UnlockedDragons.Count}";
        }
    }

    public void UpdateProductivityBonusUI()
    {
        if (productivityBonusText != null)
        {
            productivityBonusText.text = $"{PlayerManager.saveLoadManager.playerData.ProductivityMultiplier}";
        }
    }

    public void ShowConfirmationWindow(string description, string currencyChange, Sprite sprite = null)
    {
        ConformationWindowGO.SetActive(true);
        ConfirmationDescriptionText.text = description;
        ConfirmationCurrencyChangeText.text = currencyChange;
        if (sprite != null)
        {
            NewItemSprite.enabled = true;
            NewItemSprite.sprite = sprite;
        }
        else
        {
            NewItemSprite.enabled = false;
        }
    }
}
