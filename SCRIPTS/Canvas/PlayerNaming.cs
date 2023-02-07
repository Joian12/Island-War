using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using Photon.Realtime;

public class PlayerNaming : MonoBehaviourPunCallbacks
{
    public NetworkUI networkUI;
     public void EnterName(){
        
        if(networkUI.userName.text.Length-1 != 0){
            networkUI.userNameInput.SetActive(false);
            networkUI.connectButton.SetActive(true);
            PlayerPrefs.SetString("Name", networkUI.userName.text);
            networkUI.usernameText = networkUI.userName.text;
            networkUI.welcomeUser.text = networkUI.usernameText;
            networkUI.playerSC.Name = networkUI.usernameText;
            networkUI.dockButton.SetActive(true);
        }
    }

    public void Rename(){
        networkUI.userNameInput.SetActive(true);
        networkUI.connectButton.SetActive(false);
        PlayerPrefs.DeleteKey("Name");
        networkUI.playerSC.Name = "";
        networkUI.dockButton.SetActive(false);
    }
}
