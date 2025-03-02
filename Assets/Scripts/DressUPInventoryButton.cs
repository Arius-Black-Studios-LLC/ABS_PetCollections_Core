using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DressUPInventoryButton : MonoBehaviour
{
    public DragonAccesory accesory;
    public Image ButtonIcon;
    public TMP_Text buttonLabel;

    public void SetUpButton(DragonAccesory accesory)
    {
        this.accesory = accesory;
        buttonLabel.text = accesory.accesory_name;
        if (accesory.icon != null)
        {
            ButtonIcon.sprite = accesory.icon;
        }
        else
        {
            ButtonIcon.gameObject.SetActive(false);
        }


    }
    public void EquiptThisAccesoryOnDragon()
    {
        if (accesory)
        {
            DragonNPCScriptable currDragon = DragonDatabase.instance.getDragonByID(PlayerManager.instance.dragonInFocus.dragonID);
            if(accesory.accesory_type == AccesoryType.Hat)
            {
                currDragon.Hat_ID = accesory.accesory_id;
                PlayerManager.instance.dragonInFocus.wardorbManager.EquipHat(accesory.accesory_GO);


            }
            else if (accesory.accesory_type == AccesoryType.Back)
            {
                currDragon.BackpackID = accesory.accesory_id;
                PlayerManager.instance.dragonInFocus.wardorbManager.EquipBackpackItem(accesory.accesory_GO);


            }
            else if (accesory.accesory_type == AccesoryType.Hold)
            {
                currDragon.HoldingID = accesory.accesory_id;
                PlayerManager.instance.dragonInFocus.wardorbManager.EquipHoldingItem(accesory.accesory_GO);


            }
            else if (accesory.accesory_type == AccesoryType.Rideable)
            {
                currDragon.RideableID = accesory.accesory_id;
                PlayerManager.instance.dragonInFocus.wardorbManager.EquipRideableItem(accesory.accesory_GO);


            }
            else
            {
                currDragon.PetID = accesory.accesory_id;
                PlayerManager.instance.dragonInFocus.wardorbManager.EquipPet(accesory.accesory_GO);

            }


        }

    }



}
