using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrentRoomManager : MonoBehaviour
{
    public NetworkUI networkUI;

    void OnDisable(){
        for (int i = 0; i < networkUI.allPlayer.Count; i++)
        {
            Destroy(networkUI.allPlayer[i].gameObject);
        }
        networkUI.allPlayer.Clear();
    }
}
