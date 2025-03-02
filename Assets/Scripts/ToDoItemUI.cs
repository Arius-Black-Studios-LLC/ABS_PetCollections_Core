using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using ABS_SaveLoadSystem;

public class ToDoItemUI : MonoBehaviour
{
    public TMP_Text taskName;

    public ToDoListItem taskItem;

    public async void LogTaskAsComplete()
    {
        // Set the taskItem's lastLogged date to the current date (today)
        taskItem.SetLastLogged(DateTime.Now);

        //TODO check if EnergyPoints is greaterthan or equal to task priority level
        bool hasEnergy =await PlayerManager.instance.UseEnergyToCompleteTask(taskItem);
        if (hasEnergy)
        {

            // Grant the player points according to priority
            PlayerManager.instance.GrantPointsForCompleteingTask(taskItem);


        }

        // Remove this button from the ToDoListManager's list
        RemoveFromToDoList();
        Destroy(gameObject);
    }

    public void RemoveTaskForever()
    {
        PlayerManager.instance.RemoveTask(taskItem);

        Destroy(gameObject);
    }





    private void RemoveFromToDoList()
    {
        // Get a reference to the ToDoListManager
        ToDoListManager listManager = FindObjectOfType<ToDoListManager>();

        if (listManager != null)
        {
            // Remove this button from the list of displayed buttons in ToDoListManager
            listManager.buttonsShowing.Remove(gameObject);
        }
    }
}
