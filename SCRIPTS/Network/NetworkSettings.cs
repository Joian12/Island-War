using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class NetworkSettings : MonoBehaviourPunCallbacks, IPunObservable
{
   public PlayerSettingsSC playerSC;
   public SpawnAreaSC[] spawnAreaSc;
   public Vector3 spawnArea;
   public TextMeshProUGUI textTest;
   private GameObject shipGO;
 

   public void Init(){   
      
      var index = PhotonNetwork.LocalPlayer.ActorNumber;
      var shipNumber = playerSC.shipInUse;
      Debug.Log(index);
      textTest.text = index.ToString()+spawnAreaSc[index-1].SpawnTransform.position.ToString();
      
      PhotonNetwork.Instantiate(playerSC.shipPrefab[shipNumber-1].name, new Vector3(0,0,0), Quaternion.identity,0);
      
   }

   public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) { 
        
    }
}
