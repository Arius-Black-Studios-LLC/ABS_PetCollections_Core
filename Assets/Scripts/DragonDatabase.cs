using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonDatabase : MonoBehaviour
{

    public static DragonDatabase instance;
    public DragonNPCScriptable[] dragons;

    public DragonAccesory[] accesories;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(instance );
        }
    }

  

    public DragonNPCScriptable getDragonByID(int id)
    {
        foreach (var dragon in dragons)
        {
            if (id == dragon.DragonID)
            {
                return dragon;
            }
        }

        return null;
    }





    public DragonAccesory getAccesoryByID(int accesoryID)
    {
        Debug.Log("Hat ID" + accesoryID);
        foreach (var accesory in accesories)
        {
            if (accesoryID == accesory.accesory_id)
            {
                Debug.Log("Hat found!");
                return accesory;
            }
        }
        Debug.Log("Hat NOT found!");
        return null;
    }


}
