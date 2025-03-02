
using ABS_SaveLoadSystem;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;




public class JournalUIManager : MonoBehaviour
{


    
    [Header("New Entry")]
    public GameObject newEntryGO;
    public TMP_InputField new_Title;
    public TMP_InputField new_Body;

    [Header("Old Entry")]
    public GameObject oldEntryGO;
    public TMP_Text old_Title;
    public TMP_Text old_Body;
    public TMP_Text old_Date;

    public GameObject journalButtonGO;
    public Transform journalButtonGroup;
    public List<GameObject> buttonsShowing = new List<GameObject>();



    #region Create New Entry

    public async void LogJournalEntry()
    {
        JournalEnty entry = new JournalEnty
        {
            Entry_Date = DateTime.Now.Date,
            Entry_Title = new_Title.text,
            Entry_Body = new_Body.text
        };

        int pointsEarned = await getProdPointsFromjournal(entry);
        await PlayerManager.instance.AddProductivityPoints(pointsEarned); // Ensure this method supports async if it doesn't already
        await PlayerManager.instance.AddNewJournalEntry(entry);          // Ensure this method supports async if needed
    }


    private async Task<int> getProdPointsFromjournal(JournalEnty item)
    {


       if(PlayerManager.saveLoadManager.playerData.LastJournalEntry.DayOfYear == item.Entry_Date.DayOfYear &&
            PlayerManager.saveLoadManager.playerData.LastJournalEntry.Year == item.Entry_Date.Year)
        {
            return 0;
        }
        else
        {
            float journlingstreakBonus = PlayerManager.instance.journalingBasePoints * ((float)PlayerManager.saveLoadManager.playerData.JournalingStreak / 100);
            int journalingPoints = (int)(PlayerManager.instance.journalingBasePoints + journlingstreakBonus); //bASE POINTS + A 10 PERCENT BONUS FOR EVERY SUCCESSIVE DAY
            await PlayerManager.saveLoadManager.playerData.SetProductivityPointsAsync(PlayerManager.saveLoadManager.playerData.ProductivityPoints + journalingPoints);

            if (PlayerManager.instance.uiManager == null) PlayerManager.instance.uiManager = FindObjectOfType<UIManager>();
            PlayerManager.instance.uiManager.UpdateJournalingStreakUI();
            PlayerManager.instance.uiManager.ShowConfirmationWindow($"Daily Journaling streak: {PlayerManager.saveLoadManager.playerData.JournalingStreak}",
                                                    $"{journalingPoints} points earned with a {journlingstreakBonus + PlayerManager.saveLoadManager.playerData.ProductivityPoints} % bonus!",
                                                    PlayerManager.instance.JournalingStreakIcon);
            return journalingPoints;
        }
    }
    #endregion

    #region show old entries
    public void LoadOldEntries() //called when opening journal page
    {
        ClearJournalButtons();
        foreach (JournalEnty item in PlayerManager.saveLoadManager.playerData.Journal)
        {
            SpawnJournalButton(item);
            
        }
    }

    public void ShowOldEntry(JournalEnty oldEntry) //calleed when clicking on journal entry

    {

        newEntryGO.SetActive(false);
        oldEntryGO.SetActive(true);
        old_Title.text = oldEntry.Entry_Title;

        old_Body.text = oldEntry.Entry_Body;
        old_Date.text = oldEntry.Entry_Date.Month.ToString() + "-" + oldEntry.Entry_Date.Day.ToString()+"-"+ oldEntry.Entry_Date.Year.ToString();
    }
    private void SpawnJournalButton(JournalEnty item)
    {
        //set up and spawn object
        GameObject buttonGO = Instantiate(journalButtonGO, journalButtonGroup);
        if (buttonGO != null)
        {
            buttonsShowing.Add(buttonGO);
            JournalEntryButtonUI itemUI = buttonGO.GetComponent<JournalEntryButtonUI>();
            itemUI.entry = item;
            itemUI.journalTitle.text = item.Entry_Title;
        }
    }

    private void ClearJournalButtons()
    {
        // Destroy previously shown buttons
        foreach (GameObject button in buttonsShowing)
        {
            Destroy(button);
        }
        buttonsShowing.Clear();
    }


    #endregion
}
