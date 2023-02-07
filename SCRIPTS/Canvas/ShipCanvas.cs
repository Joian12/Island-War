using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using Photon.Pun;
using Photon.Realtime;

public class ShipCanvas : MonoBehaviourPunCallbacks
{   
    public TextMeshProUGUI playerName;
    public TextMeshProUGUI playerID;
    public Button forward;
    public Button reverse;
    public RawImage wheel;
    // public Button shootRight, shootLeft;
    public Button test;
    public Button shootProjectileRight;
    public Button shootProjectileLeft;
    // PlayerInfoList UIS
    public Transform playerListParent;
    public GameObject playerInfo;
    public TextMeshProUGUI countDownNumber;
    ///To Disable in Start
    public Image[] allImageToDisable;
    public RawImage[] allRawImageToDisable;
    public GameObject palyerInfo, displayStatusList;

    ///Game End UI
    public Transform containerBox_Win;
    public Image gameEndpanel;
    public TextMeshProUGUI endGamePlayerName;

    ///change value to 0 to disable and 1 to re-enable

    public void Start(){
        //EnableGameEndUI();
    }
    public void DisableOrEnableUI_inScene(float alphaValueForImage, float alphaValueForRawImage){
        for (int i = 0; i < allImageToDisable.Length; i++){
            Color newColor = allImageToDisable[i].color;
            newColor.a = alphaValueForImage;
            allImageToDisable[i].color = newColor;
        }

        for (int i = 0; i < allRawImageToDisable.Length; i++){
            Color newRawImageColor = allRawImageToDisable[i].color;
            newRawImageColor.a = alphaValueForRawImage;
            allRawImageToDisable[i].color = newRawImageColor;
        }

        if(alphaValueForRawImage == 1f){
            displayStatusList.SetActive(true);
        }else{
            displayStatusList.SetActive(false);
        }
    } 
    
    ///Eneables all UI tha associate with game state end;
    public void EnableGameEndUI(){
        gameEndpanel.gameObject.SetActive(true);
        gameEndpanel.DOFade(0.75f, 2f).SetEase(Ease.InOutSine);
        containerBox_Win.DOScale(1f, 3f).SetEase(Ease.OutBounce);
    }


    public void GoToMenu(){
        PhotonNetwork.LeaveRoom();
        Time.timeScale = 1f;
    }

    public override void OnLeftRoom(){
        Debug.Log("Leaving Room");
        if(!PhotonNetwork.InRoom){ PhotonNetwork.LoadLevel(0);}
    }

    public void TurnWheel(float spin){
        float rotationSpeed = 70f;
        Vector3 turnVector = new Vector3(0f,0f,-spin);
        wheel.transform.Rotate(turnVector * Time.deltaTime * rotationSpeed);
    }
}
