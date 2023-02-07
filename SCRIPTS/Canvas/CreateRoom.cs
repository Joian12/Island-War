using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using Photon.Realtime;
public class CreateRoom : MonoBehaviourPunCallbacks
{
    public NetworkUI networkUI;
    public TextMeshProUGUI RoomName;
    public Transform roomList;
    public GameObject roomPrefab;
    public List<Room> allRoom = new List<Room>();

    public void CreateRoomButton(){
        RoomOptions option = new RoomOptions();
        option.MaxPlayers = 4;
        PhotonNetwork.CreateRoom(RoomName.text, option);
        networkUI.currentRoom.SetActive(true);
        networkUI.currentRoomText.text = RoomName.text;
        networkUI.dockButton.SetActive(false);
    }

    public override void OnCreatedRoom()
    {
        networkUI.CheckPlayersInRoom();
    }

     public override void OnRoomListUpdate(List<RoomInfo> list){
        
        foreach(RoomInfo roomInfo in list){
            if(roomInfo.RemovedFromList){
                int index = allRoom.FindIndex( x => x.roomName.text == roomInfo.Name);
                if(index != -1){
                    Destroy(allRoom[index].gameObject);
                    allRoom.RemoveAt(index);
                }
            }else{
                int index = allRoom.FindIndex( x => x.roomName.text == roomInfo.Name);
                if(index == -1){
                    GameObject go = Instantiate(roomPrefab);
                    Room goRoom = go.GetComponent<Room>();
                    goRoom.roomName.text = roomInfo.Name;
                    goRoom.InitializedRoom(networkUI.currentRoom, networkUI.currentRoomText);
                    go.transform.SetParent(roomList, false);
                    allRoom.Add(goRoom);
                }
                
            }  
        }
    }
}
