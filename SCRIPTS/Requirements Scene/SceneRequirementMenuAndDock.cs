using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[RequireComponent (typeof(LocalSaveandLoad))]

public class SceneRequirementMenuAndDock : MonoBehaviour
{   
    public LocalSaveandLoad _localSaveandLoad;
    
    private void Start(){
        _localSaveandLoad = gameObject.GetComponent<LocalSaveandLoad>();//gameObject.AddComponent(typeof(LocalSaveandLoad)) as LocalSaveandLoad;
        _localSaveandLoad.Init(PlayerSceneInfoManager.instance.playerSettingsSC);
    } 
}
