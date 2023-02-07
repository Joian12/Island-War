using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipSpawner : MonoBehaviour
{
    public GameObject[] shipSpawner = new GameObject[3];

    public void Start(){
        Debug.Log(PlayerSceneInfoManager.instance == null);
        for (int i = 0; i < shipSpawner.Length; i++){
           Instantiate(PlayerSceneInfoManager.instance.playerSettingsSC.shipPrefab[i], 
                        shipSpawner[i].transform.position, shipSpawner[i].transform.rotation );
        }
    }
}
