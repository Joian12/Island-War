using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class DockManager : MonoBehaviourPunCallbacks
{       
    public DockUICanvasReferences dockRef;
    public LocalSaveandLoad localSaveandLoad;
    
    public CinemachineFreeLook cam;
    public Vector3 currentCamPos = new Vector3(-22, 7, -30);
    public Vector3  camPosFrom = new Vector3(-22, 7, -30), 
                    camPosTo = new Vector3(-22, 7, -30); 
    private float desireDuration = 1.5f;
    private float elapsedTime;
    private int shipNumber;

    private string playerName;


    void Start(){
        localSaveandLoad = FindObjectOfType(typeof(LocalSaveandLoad)) as LocalSaveandLoad;
        
        var camPosLoad = localSaveandLoad.LoadDockSetings();
        PlayerSceneInfoManager.instance.playerSettingsSC.x = camPosLoad.x;
        PlayerSceneInfoManager.instance.playerSettingsSC.y = camPosLoad.y;
        PlayerSceneInfoManager.instance.playerSettingsSC.z = camPosLoad.z;
        PlayerSceneInfoManager.instance.playerSettingsSC.shipInUse = localSaveandLoad.LoadShipDockInUse();
        currentCamPos = new Vector3(-22, 7, -30);
        camPosFrom = camPosLoad;//new Vector3(-22, 7, -30);
        camPosTo = camPosLoad;//new Vector3(-22, 7, -30);
        dockRef.ship1.onClick.AddListener( delegate {ChooseShip1();});
        dockRef.ship2.onClick.AddListener( delegate {ChooseShip2();});
        dockRef.ship3.onClick.AddListener( delegate {ChooseShip3();});
        dockRef.select.onClick.AddListener( delegate {Select();});
        dockRef.yes.onClick.AddListener( delegate {Yes();});
        dockRef.no.onClick.AddListener( delegate {No();});

        var shipNumber = PlayerSceneInfoManager.instance.playerSettingsSC.shipInUse;
        playerName = PlayerSceneInfoManager.instance.playerSettingsSC.Name;
        dockRef.PlayerInfo(playerName, PlayerSceneInfoManager.instance.playerSettingsSC.shipPrefab[1].name);
    }

    public void ChooseShip1(){
        Choosing(1, currentCamPos, new Vector3(-22, 7, -30));
    }

    public void ChooseShip2(){
        Choosing(2, currentCamPos, new Vector3(-12, 7, -30));
    }

    public void ChooseShip3(){
        Choosing(3, currentCamPos, new Vector3(-2, 7, -30));
    }

    private void Choosing(int shipInUse, Vector3 currentCamPos_, Vector3 camPosTo_){
        elapsedTime = 0;
        shipNumber = shipInUse;
        camPosFrom = currentCamPos_;
        camPosTo = camPosTo_;
        
        dockRef.PlayerInfo(playerName, PlayerSceneInfoManager.instance.playerSettingsSC.shipPrefab[shipNumber-1].name);
    }
    
    public void Yes(){
        dockRef.successfullUI.SetActive(true);
        dockRef.confirmation.SetActive(false);
        localSaveandLoad.SavePlayerInfo(camPosTo);
        PlayerSceneInfoManager.instance.playerSettingsSC.shipInUse = shipNumber;
    }

    public void No(){
        dockRef.confirmation.SetActive(false);
    }

    public void Select(){
        dockRef.container.SetActive(false);
        dockRef.confirmation.SetActive(true);
    }

    public void ReturnToMainMenu(){
        SceneManager.LoadScene(0);
    }

    void Update(){
        elapsedTime += Time.deltaTime;
        float percentage = elapsedTime/desireDuration;
        cam.transform.position = Vector3.Lerp(camPosFrom, camPosTo, Mathf.SmoothStep(0, 1, percentage));
        currentCamPos = cam.transform.position;
    }

    
}
