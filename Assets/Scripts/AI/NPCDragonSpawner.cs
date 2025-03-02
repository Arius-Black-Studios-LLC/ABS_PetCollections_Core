using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using TMPro;
using ABS_SaveLoadSystem;
using Unity.VisualScripting;

public class NPCDragonSpawner : MonoBehaviour
{
    public static NPCDragonSpawner instance;

    [Header("Prefabs")]
    [Tooltip("The Prefab that controls the NPC Dragons")]
    [SerializeField]private GameObject NPC_Brain;

    [Space(5)]
    [Header("Scene References")]
    [SerializeField]private CinemachineFreeLook cam;
    [SerializeField]private TMP_Text dragonName;
    [SerializeField]private GameObject firstDragonUI;
    [SerializeField] private Transform[] spawnAreas;

    private List<GameObject> dragonsInScene = new List<GameObject>();
    private int dragonIndex;
    private bool isFirstTimePlaying;




    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        Debug.Log("Dragons unlocked :" + PlayerManager.saveLoadManager.playerData.UnlockedDragons.Count);
        isFirstTimePlaying = PlayerManager.saveLoadManager.playerData.UnlockedDragons.Count <= 0;

        if (!isFirstTimePlaying)
        {
            SpawnNPCDragons();
            firstDragonUI.SetActive(false);
        }
    }


    #region Button Functions
    public void LookAtNextDragon()
    {
        dragonIndex++;
        if (dragonIndex >= dragonsInScene.Count)
        {
            dragonIndex = 0;
        }
        GameObject lookFollow = dragonsInScene[dragonIndex];
        PlayerManager.instance.dragonInFocus = lookFollow.GetComponent<DragonNPCManager>();
        cam.Follow = lookFollow.transform;
        cam.LookAt = lookFollow.transform;
        ShowDragonName();
    }
    public void LookAtLastDragon()
    {
        dragonIndex--;
        if (dragonIndex < 0)
        {
            dragonIndex = dragonsInScene.Count - 1;
        }
        GameObject lookFollow = dragonsInScene[dragonIndex];
        PlayerManager.instance.dragonInFocus = lookFollow.GetComponent<DragonNPCManager>();
        cam.Follow = lookFollow.transform;
        cam.LookAt = lookFollow.transform;
        ShowDragonName();
    }

    //Called By The New Player Dragon Granting UI
    public void GiveDragonToPlayer()
    {
        PlayerManager.instance.AddRandomDragon(true);
        firstDragonUI.SetActive(false);
    }
    #endregion

    public void SpawnDragonWithBrain(DragonData dragon)
    {
        GameObject dragonGO = DragonDatabase.instance.getDragonByID(dragon.Id).DragonObject;
        if (dragonGO != null)
        {
            //Spawn Dragon Manager
            Vector3 randomOffset = new Vector3(UnityEngine.Random.Range(-10, 10), 0f, UnityEngine.Random.Range(-10, 10));
            Transform randomTrasform = spawnAreas[UnityEngine.Random.Range(0, spawnAreas.Length)];
            GameObject _SpawnedBrain = Instantiate(NPC_Brain, randomTrasform.position + randomOffset, Quaternion.identity);
            dragonsInScene.Add(_SpawnedBrain);
            dragonIndex = dragonsInScene.IndexOf(_SpawnedBrain);

            //set up dragon manager
            DragonNPCManager DManager = _SpawnedBrain.GetComponent<DragonNPCManager>();
            DManager.dragonID = dragon.Id;


            //spawn dragon body
            Instantiate(dragonGO, _SpawnedBrain.transform);

            DManager.animManager = _SpawnedBrain.GetComponentInChildren<DragonAnimManager>();
            DManager.wardorbManager = _SpawnedBrain.GetComponentInChildren<DragonWardorbManager>();


            DragonAccesory temp_hat = DragonDatabase.instance.getAccesoryByID(dragon.hatID);
            if (temp_hat != null)
            {
                DManager.wardorbManager.EquipHat(temp_hat.accesory_GO);
            }
            DragonAccesory temp_pet = DragonDatabase.instance.getAccesoryByID(dragon.petID);
            if (temp_pet != null)
            {
                DManager.wardorbManager.EquipPet(temp_pet.accesory_GO);
            }

            PlayerManager.instance.dragonInFocus = DManager;
            cam.Follow = DManager.transform;
            cam.LookAt = DManager.transform;
            ShowDragonName();



        }
    }

    private void SpawnNPCDragons()
    {
        foreach(DragonData dragon in PlayerManager.saveLoadManager.playerData.UnlockedDragons)
        {
                SpawnDragonWithBrain(dragon);
        }

        dragonIndex = Random.Range(0, dragonsInScene.Count - 1);
        GameObject lookFollow = dragonsInScene[dragonIndex];
        PlayerManager.instance.dragonInFocus = lookFollow.GetComponent<DragonNPCManager>();
        ShowDragonName();

        cam.Follow = lookFollow.transform;
        cam.LookAt = lookFollow.transform;
    }


   


    private void ShowDragonName()
    {
        dragonName.text = DragonDatabase.instance.getDragonByID(dragonsInScene[dragonIndex].GetComponent<DragonNPCManager>().dragonID).DragonName;
    }
}
