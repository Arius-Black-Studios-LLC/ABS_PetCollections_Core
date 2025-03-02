using ABS_SaveLoadSystem;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ToDoListItem
{
    [JsonProperty("taskName")]
    public string _taskName { get; private set; }

    [JsonProperty("priority")]
    public int _priority { get; private set; }

    [JsonProperty("taskCategory")]
    public TaskCategory _taskCategory { get; private set; }

    [JsonProperty("taskFrequency")]
    public TaskFrequency _taskFrequency { get; private set; }

    [JsonProperty("lastLogged")]
    public DateTime LastLogged { get; private set; }

    // Parameterless constructor for deserialization
    public ToDoListItem() { }
    public ToDoListItem(string taskName, int priority, TaskCategory taskCategory, TaskFrequency taskFrequency, DateTime lastLogged)
    {
        _taskName = taskName;
        _priority = priority;
        this._taskCategory = taskCategory;
        this._taskFrequency = taskFrequency;
        LastLogged = lastLogged;
    }

    public void SetTaskName(string value)
    {
        if (_taskName != value)
        {
            _taskName = value;
            NotifyToDoListItemChanged();
        }
    }


    public void SetPriority(int value)
    {
        if (_priority != value)
        {
            _priority = value;
            NotifyToDoListItemChanged();
        }
    }

    public void SetTaskCategory(TaskCategory value)
    {
        if (_taskCategory != value)
        {
            _taskCategory = value;
            NotifyToDoListItemChanged();
        }
    }


    public void SetTaskFrequency(TaskFrequency value)
    {
        if (_taskFrequency != value)
        {
            _taskFrequency = value;
            NotifyToDoListItemChanged();
        }
    }


    public void SetLastLogged(DateTime value)
    {
        if (LastLogged != value)
        {
            LastLogged = value;
            NotifyToDoListItemChanged();
        }
    }
    // Method to notify changes (called after deserialization or saving)

    public void NotifyToDoListItemChanged()
    {
        if (PlayerManager.saveLoadManager != null)
        {
            PlayerManager.saveLoadManager.SetChanges(this, PlayerManager.saveLoadManager.playerData.ToDoListItems, nameof(PlayerManager.saveLoadManager.playerData.ToDoListItems));
        }
    }
}


public enum TaskCategory { Downtime, Routine, Activity, Growth, Organize, Nutrition, Social }
public enum TaskFrequency { Daily, Weekly, BiWeekly, Monthly }
