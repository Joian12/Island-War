using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalSaveandLoad : MonoBehaviour
{   
    public PlayerSettingsSC _playerSettings;
    public void Init(PlayerSettingsSC playerSettingsSC_){
        _playerSettings = playerSettingsSC_;
        LoadPlayerInfo();
    }

    public void LoadPlayerInfo(){
        LoadDockSetings();
    }

    public void SavePlayerInfo(Vector3 currentCamPos){
        SaveDockSettings(currentCamPos);
    }

    private void SaveDockSettings(Vector3 currentCamPos){
        PlayerPrefs.SetString("Name", _playerSettings.Name);
        for (int i = 0; i < _playerSettings.shipPrefab.Length; i++){
            PlayerPrefs.SetString("Ship"+i, _playerSettings.shipPrefab[i].name);
        }
        PlayerPrefs.SetInt("ShipInUse", _playerSettings.shipInUse);
        
        PlayerPrefs.SetFloat("PosX", currentCamPos.x);
        PlayerPrefs.SetFloat("PosY", currentCamPos.y);
        PlayerPrefs.SetFloat("PosZ", currentCamPos.z); 
        
    }

    public Vector3 LoadDockSetings(){
        Vector3 loadCamPos;
        loadCamPos.x = PlayerPrefs.GetFloat("PosX");
        loadCamPos.y = PlayerPrefs.GetFloat("PosY");
        loadCamPos.z = PlayerPrefs.GetFloat("PosZ");
        return loadCamPos;
    }

    public int LoadShipDockInUse(){
        return PlayerPrefs.GetInt("ShipInUse");
    }
}
