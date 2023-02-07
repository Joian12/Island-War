using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine.UI;

public class Room : MonoBehaviourPunCallbacks
{
    public TextMeshProUGUI roomName, currentText_;
    public GameObject currentRoom_, netWorkUI_Canvas;
    
    public Button startGame;

    // public void Awake(){
    //     // if(!PhotonNetwork.IsMasterClient) return; 

    //     netWorkUI_Canvas = GameObject.FindGameObjectWithTag("NetworkUI_Canvas").gameObject;
    //     startGame = netWorkUI_Canvas.gameObject.transform.GetChild(4).gameObject.transform.GetChild(0).
    //                         gameObject.transform.GetChild(3).gameObject.GetComponent<Button>();

    //     startGame.onClick.AddListener(delegate {StartGame();});
    // }

    public void InitializedRoom(GameObject currentRoom, TextMeshProUGUI currentText){
        currentRoom_ = currentRoom;
        currentText_ = currentText;
    }

    public void JoinThisRoom(){
        Debug.Log("Enter This Room");
        currentRoom_.SetActive(true);
        currentText_.text = roomName.text;
        PhotonNetwork.JoinRoom(currentText_.text);
        
    }
}
