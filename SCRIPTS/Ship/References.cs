using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Photon.Pun;
using Photon.Realtime;

public class References : MonoBehaviourPunCallbacks
{   
    public CinemachineFreeLook cnmFL;

    public ShipCanvas shipCanvas;
    public ShipStatus shipStatus;
    public Inputs_ input;
    public PlayerSceneInfoManager playerManager;
    public CanonRayCast canonRayCast;
    public NetworkObjectPooling _networkObjectPooling;
    public GameObject netwokrManager;
    public SpawnAreaSC[] spawnAreaSc;
    public GameManager gameManager;
    public GameObject childShip; 

    public void Awake(){

        netwokrManager = GameObject.FindGameObjectWithTag("NetworkManager");
        gameManager = netwokrManager.GetComponent<GameManager>();

        if(!base.photonView.IsMine) return;
        

        cnmFL = FindObjectOfType<CinemachineFreeLook>();
        playerManager = (PlayerSceneInfoManager)FindObjectOfType(typeof(PlayerSceneInfoManager));
        
    
        _networkObjectPooling.Init(shipStatus);

        shipCanvas = GameObject.FindGameObjectWithTag("ShipCanvas").GetComponent<ShipCanvas>();
        if(shipCanvas != null){
            input.Init(cnmFL, shipCanvas, shipStatus, canonRayCast, childShip);
            shipStatus.Init(playerManager, _networkObjectPooling, shipCanvas, gameManager);
        }

        RepositionShip();
    }

    private void RepositionShip(){
        var index = PhotonNetwork.LocalPlayer.ActorNumber;
        spawnAreaSc = netwokrManager.GetComponent<NetworkSettings>().spawnAreaSc;
        var spawnArea = spawnAreaSc[index-1].SpawnTransform.position;
        transform.position = spawnArea;
        
    }

    public void OnDisable(){
        if(gameManager != null){
            gameManager.photonView.RPC("Test", RpcTarget.All);
        }
    }

}
