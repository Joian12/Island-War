using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSceneInfoManager : MonoBehaviour
{
    public static PlayerSceneInfoManager instance;
    public PlayerSettingsSC playerSettingsSC;

    public void Awake(){
      instance = this;
    }

    public void ChangeShipsInPlaerPrefs(GameObject ship, int dockNumber){
        switch(dockNumber){
            case 1:
                playerSettingsSC.shipPrefab[0] = ship;
                    break;
            case 2:
                playerSettingsSC.shipPrefab[1] = ship;
                    break;
            case 3:
                playerSettingsSC.shipPrefab[2] = ship;
                    break;        
        }
    }
}
