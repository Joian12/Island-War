using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using Photon.Realtime;

public class LeaveRoom : MonoBehaviourPunCallbacks
{
    public NetworkUI networkUI;
    public void LeaveRoomButton(){
        networkUI.currentRoomText.text = "";
        networkUI.currentRoom.SetActive(false);
        networkUI.dockButton.SetActive(true);
        PhotonNetwork.LeaveRoom();

       Debug.Log("LeaveRoom()");
    }

     public override void OnPlayerLeftRoom(Player otherPlayer)
    {
         int index = networkUI.allPlayer.FindIndex( x => x.player == otherPlayer);
         if(index != -1){
            Destroy(networkUI.allPlayer[index].gameObject);
            networkUI.allPlayer.RemoveAt(index);
         }
    }
}
