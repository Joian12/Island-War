using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class PlayerInfoContainer : MonoBehaviourPun
{   
    public ShipCanvas shipCanvas;
    public TextMeshProUGUI playerName;
    public TextMeshProUGUI alive;
    public TextMeshProUGUI dead;
    private void Awake(){
        //if(!base.photonView.IsMine) return;
        
        // playerName = GetComponentInChildren<TextMeshProUGUI>();
        // shipCanvas = GameObject.FindGameObjectWithTag("ShipCanvas").GetComponent<ShipCanvas>();
        // var name = PhotonNetwork.LocalPlayer.NickName;
        // photonView.RPC("SetContainerPos", RpcTarget.All, name);

        // if(!base.photonView.IsMine) return;
        
        // playerName.text = PhotonNetwork.LocalPlayer.NickName;
    }   

    // [PunRPC]
    // private void SetContainerPos(string name){
    //     transform.localPosition = new Vector3(97.2905f, -17.5f, 0f);
    //     transform.SetParent(shipCanvas.playerListParent, false);
    //     //playerName.text = name;
        
    // }


}
