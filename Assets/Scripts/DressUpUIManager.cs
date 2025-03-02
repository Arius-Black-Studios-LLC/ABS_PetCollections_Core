using ABS_SaveLoadSystem;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DressUpUIManager : MonoBehaviour
{
    [SerializeField] private Transform scrollView;
    [SerializeField] private DressUPInventoryButton buttonPrefab;
    public TMP_Dropdown OutfitCategories;
    private List<DressUPInventoryButton> buttonList = new List<DressUPInventoryButton>();

    void Start()
    {
        // Add listener to the dropdown
        OutfitCategories.onValueChanged.AddListener(delegate {
            SpawnButtonsOfAccesoryByType(OutfitCategories.value);
        });

        // Initialize the Inventory Images list with the default category
        SpawnButtonsOfAccesoryByType(OutfitCategories.value);
    }

    private void SpawnButtonsOfAccesoryByType(int accesoryTypeIndex)
    {
        DestoyAllButtons();
        AccesoryType accesoryType = (AccesoryType)accesoryTypeIndex;
        for (int i = 0; i < PlayerManager.saveLoadManager.playerData.UnlockedAccessories.Count; i++)
        {
            DragonAccesory accesory = DragonDatabase.instance.getAccesoryByID(PlayerManager.saveLoadManager .playerData.UnlockedAccessories[i].accesory_id);
            if (accesory.accesory_type == accesoryType)
            {
                DressUPInventoryButton dressUpButton = Instantiate(buttonPrefab, scrollView);
                buttonList.Add(dressUpButton);
                dressUpButton.SetUpButton(accesory);
            }


        }
    }
    private void DestoyAllButtons()
    {
        foreach(DressUPInventoryButton butt in buttonList)
        {
            Destroy(butt.gameObject);
        }
        buttonList.Clear();
    }
}
