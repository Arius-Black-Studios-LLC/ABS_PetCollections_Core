using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class IAPManager : MonoBehaviour
{
    public Button productivityBonusButton;

    private async void OnEnable()
    {
        // Safely check and set button interactability based on the productivity bonus.
        bool isBonusAvailable = await CheckProductivityBonusAsync();
        productivityBonusButton.interactable = isBonusAvailable;
    }


    public async void Item1_onPurchaseSuccess()
    {
        await PlayerManager.instance.ApplyProductivityBonus(5);
        productivityBonusButton.interactable = PlayerManager.instance.CheckProductivityBonus().Result;
        
    }
    /// <summary>
    /// Asynchronously checks if the productivity bonus is available.
    /// </summary>
    /// <returns>True if the bonus is available, otherwise false.</returns>
    private async Task<bool> CheckProductivityBonusAsync()
    {
        try
        {
            return await PlayerManager.instance.CheckProductivityBonus();
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"Error checking productivity bonus: {ex.Message}");
            return false;
        }
    }
}
