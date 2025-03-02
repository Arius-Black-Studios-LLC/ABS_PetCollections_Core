using UnityEngine;
using System.Collections.Generic;

public static class TransformExtensions
{
    //Breadth-first search for finding a child transform by name
    public static Transform FindDeepChild(this Transform aParent, string aName)
    {
        Queue<Transform> queue = new Queue<Transform>();
        queue.Enqueue(aParent);
        while (queue.Count > 0)
        {
            var c = queue.Dequeue();
            if (c.name == aName)
                return c;
            foreach (Transform t in c)
                queue.Enqueue(t);
        }
        return null;
    }
}

public class DragonWardorbManager : MonoBehaviour
{
    public string holdingSpotName = "Head";
    public GameObject holdingSpotParentPrefab;
    private Transform holdingSpotSpawnPoint;
    private GameObject currentHolding;


    public string hatSpotName = "Head";
    public GameObject hatSpotParentPrefab;
    private Transform hatSpotSpawnPoint;
    private GameObject currentHat;

    public string rideableSpotName = "Root";
    public GameObject rideableSpotParentPrefab;
    private Transform rideableSpotSpawnPoint;
    private GameObject currentRideable;

    public string backpacksSpotName = "Spine1";
    public GameObject backpacksSpotParentPrefab;
    public Transform backpacksSpotSpawnPoint;
    private GameObject currentBackpack;




    private GameObject currentPet;




    private void Awake()
    {
        // Spawn holding spot parent under the joint called "Head"
        SpawnAttachmentParent(holdingSpotName, holdingSpotParentPrefab, ref holdingSpotSpawnPoint);
        // Spawn other spot parents similarly
        SpawnAttachmentParent(hatSpotName, hatSpotParentPrefab, ref hatSpotSpawnPoint);
        SpawnAttachmentParent(rideableSpotName, rideableSpotParentPrefab, ref rideableSpotSpawnPoint);
        SpawnAttachmentParent(backpacksSpotName, backpacksSpotParentPrefab, ref backpacksSpotSpawnPoint);
    }

    private GameObject SpawnAttachmentParent(string attachmentName, GameObject parentPrefab, ref Transform spawnPointRef)
    {
        Transform attachmentPoint = FindAttachmentPoint(attachmentName);
        if (attachmentPoint != null && parentPrefab != null)
        {
            // Instantiate the parent object at the attachment point
            GameObject parentObject = Instantiate(parentPrefab,attachmentPoint);
            parentObject.transform.SetParent(attachmentPoint);
            spawnPointRef = parentObject.transform;
            return parentObject;
        }
        else
        {
            Debug.LogError("Attachment point or parent prefab is null.");
            return null;
        }
    }
    public void EquipHat(GameObject hatPrefab)
    {
        EquipItem(hatPrefab, hatSpotSpawnPoint, ref currentHat);
    }

    public void EquipHoldingItem(GameObject holdingPrefab)
    {
        EquipItem(holdingPrefab, holdingSpotSpawnPoint, ref currentHolding);
    }

    public void EquipBackpackItem(GameObject backpackPrefab)
    {
        EquipItem(backpackPrefab, backpacksSpotSpawnPoint, ref currentBackpack);
    }

    public void EquipRideableItem(GameObject rideablePrefab)
    {
        EquipItem(rideablePrefab, rideableSpotSpawnPoint, ref currentRideable);
    }

    // Equip a pet to the dragon
    public void EquipPet(GameObject petPrefab)
    {
        if (petPrefab != null)
        {
            // Remove current pet if exists
            if (currentPet != null)
                Destroy(currentPet);

            // Instantiate the new pet
            currentPet = Instantiate(petPrefab, transform.position, Quaternion.identity);
        }
    }

    // Helper method to equip an item
    private void EquipItem(GameObject prefab,Transform attachmentPoint, ref GameObject currentItem)
    {// Remove current item if exists
        if (currentItem != null)
            Destroy(currentItem);
        
        currentItem = Instantiate(prefab, attachmentPoint);
        

    }

    // Helper method to find attachment points by name under the given parent
    private Transform FindAttachmentPoint(string attachmentName)
    {
        Transform attachment = transform.FindDeepChild(attachmentName);
        if (attachment != null)
            return attachment;
        else
        {
            Debug.LogError("Attachment point '" + attachmentName + "' not found.");
            return null;
        }
    }
}
