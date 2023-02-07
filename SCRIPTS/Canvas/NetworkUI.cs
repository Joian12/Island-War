using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;
using TMPro;
using Photon.Realtime;

public class NetworkUI : MonoBehaviourPunCallbacks
{   
    [SerializeField]
    private NetworkSettingSC nsSC;
    public PlayerSettingsSC playerSC;
    public GameObject userNameInput, connectButton, lobby, startGameButton, dockButton, title;
    public TextMeshProUGUI textConnect, welcomeUser;
    public TextMeshProUGUI userName, currentRoomText;
    public GameObject currentRoom;

    public string usernameText;
    public List<PlayerInfo_> allPlayer = new List<PlayerInfo_>();
    public GameObject playerPrefab;
    public Transform playerListParent;

     public void Start(){
        usernameText = PlayerPrefs.GetString("Name");
        lobby.SetActive(false);
        currentRoom.SetActive(false);
        Debug.Log(usernameText.Length);
        if(usernameText.Length == 0){
            userNameInput.SetActive(true);
            connectButton.SetActive(false);
            dockButton.SetActive(false);
            title.SetActive(false);
        }else{
            userNameInput.SetActive(false);
            connectButton.SetActive(true);
            welcomeUser.text = usernameText;
            playerSC.Name = usernameText;
            dockButton.SetActive(true);
            title.SetActive(true);
        }
        PhotonNetwork.GameVersion = "0.0.1";
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    public void Connect(){
        
        PhotonNetwork.ConnectUsingSettings();
        PhotonNetwork.ConnectToRegion("jp");
        textConnect.text = "Connecting....";
    }

    public void DisConnect(){
        
    }

    public override void OnJoinedRoom(){  
        startGameButton.SetActive(PhotonNetwork.IsMasterClient);
        Debug.Log("OnJoinedRoom()");
        CheckPlayersInRoom();
    }
    public override void OnPlayerEnteredRoom(Player newPlayer){   
        Debug.Log("OnPlayerEnteredRoom()");
        CheckPlayersInRoom();
    }

    public override void OnConnectedToMaster()
    {
        lobby.SetActive(true);
        PhotonNetwork.NickName = usernameText;
        base.OnConnectedToMaster();
        Debug.Log("Connected To Server");
        if(!PhotonNetwork.InLobby)
            PhotonNetwork.JoinLobby();
    }

    public void CheckPlayersInRoom(){
        foreach(Player player in PhotonNetwork.PlayerList){
            int index = allPlayer.FindIndex( x => x.player == player);
            if(index == -1){
                GameObject go = Instantiate(playerPrefab);
                PlayerInfo_ playerInfo = go.GetComponent<PlayerInfo_>();
                playerInfo.player = player;
                playerInfo.playerName.text = player.NickName;
                go.transform.SetParent(playerListParent, false);
                allPlayer.Add(playerInfo);
            }
        }
    }

    public void StartGame(){

        if(PhotonNetwork.IsMasterClient){
            PhotonNetwork.CurrentRoom.IsOpen = false;
            PhotonNetwork.CurrentRoom.IsVisible = false;
            PhotonNetwork.LoadLevel(2);
        }
        
    }

    public void GoToDock(){
        if(PhotonNetwork.IsConnected)
            PhotonNetwork.Disconnect();
            
        SceneManager.LoadScene(1);
    }
}
