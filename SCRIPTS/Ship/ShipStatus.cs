using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using UnityEngine.InputSystem;

public class ShipStatus : MonoBehaviourPun , IDamageble
{
    private const byte CHANGE_HP_EVENTS = 0;
    public string playerName;
    public int playerID;
    public Image shipHP;
    public Image mainBar;
    public Camera cam;
    public float currentHP, maxHP;
    public Transform[] allCanonTransformRight;
    public Transform[] allCanonTransformLeft;
    public ShipSettingSC shipSettingSC;
   //public PoolingManager poolingManager;
    public PlayerSceneInfoManager playerSceneInfoManager;
    public NetworkObjectPooling _networkObjectPooling;
    private ShipCanvas _shipCanvas;
    private GameManager _gameManager;
    public bool isAlive;

    public void Awake(){
        cam = Camera.main;
        PhotonView photonView = PhotonView.Get(this);
        isAlive = true;
    }

    public void Init(PlayerSceneInfoManager playerSceneInfoManager_, NetworkObjectPooling networkObjectPooling_, ShipCanvas shipCanvas_, GameManager gameManager_){
        
        playerID = PhotonNetwork.LocalPlayer.ActorNumber;
        playerName = PhotonNetwork.LocalPlayer.NickName;
        playerSceneInfoManager = playerSceneInfoManager_;
        _gameManager = gameManager_;
        //playerName = playerSceneInfoManager.playerSettingsSC.name;
       // poolingManager = poolingManager_;
        _networkObjectPooling = networkObjectPooling_;
        
        currentHP = shipSettingSC.maxHealth;
        maxHP = shipSettingSC.maxHealth;
        //poolingManager.canonPrefab_[0].canonBallDamage = shipSettingSC.damage;
        photonView.RPC("SetShipData", RpcTarget.All, currentHP, maxHP);
        shipHP.fillAmount = currentHP/maxHP;
        shipCanvas_.playerName.text = playerName;
        shipCanvas_.playerID.text = (playerID).ToString();
    }

    [PunRPC]
    void SetShipData(float currentHP_, float maxHP_){
        currentHP = currentHP_;
        maxHP = maxHP_;
    }

    void Update(){
        mainBar.transform.LookAt(cam.transform);
        if(!base.photonView.IsMine) return;
    }


    [PunRPC]
    public void TakeDamage(float damage){
        currentHP -= damage;
        shipHP.fillAmount = currentHP/maxHP;
        if(currentHP <= 0){
            SetupName();
            Death();
        }
    }

    private string nameTest;
    private void SetupName(){
        nameTest = PhotonNetwork.LocalPlayer.NickName;
    }
    public void Death(){
        if(!base.photonView.IsMine) return;

        isAlive = false;
        _gameManager.photonView.RPC("UpdateShipDisplayStatus", RpcTarget.All, nameTest);
        _gameManager.photonView.RPC("UpdateGameStateInScene", RpcTarget.All);
    }

    

    public void FireCannonsRight(){

        if(!base.photonView.IsMine) return;

        for (int i = 0; i < allCanonTransformRight.Length; i++)
        {
            var go = _networkObjectPooling.GetCanonRight();
            go.transform.position = allCanonTransformRight[i].position;
            go.transform.rotation = allCanonTransformRight[i].rotation;
            go.SetActive(true);
        }
    }   

    public void FireCannonsLeft(){

        if(!base.photonView.IsMine) return;

        for (int i = 0; i < allCanonTransformLeft.Length; i++)
        {
            var go = _networkObjectPooling.GetCanonLeft();
            go.transform.position = allCanonTransformLeft[i].position;
            go.transform.rotation = allCanonTransformLeft[i].rotation;
            go.SetActive(true);
        }
    }  
}
