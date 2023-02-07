using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;


public class NetworkObjectPooling : MonoBehaviourPun
{   
    public List<GameObject> pooledCanonRight = new List<GameObject>();
    public List<GameObject> pooledCanonLeft = new List<GameObject>();
    public string canonInUse;
    public Transform parentOfCanons;
    public ShipStatus _shipStatus;
    public void Init (ShipStatus shipStatus_)
    {   
        _shipStatus = shipStatus_;
        
        var pos = new Vector3(0,-10,0);
        var playerID_ = PhotonNetwork.LocalPlayer.ActorNumber;

        for (int i = 0; i < _shipStatus.allCanonTransformRight.Length; i++)
        {
            InstantiateObjects(canonInUse, pos, Quaternion.identity, pooledCanonRight, playerID_);
        }

        for (int i = 0; i < _shipStatus.allCanonTransformLeft.Length; i++)
        {
            InstantiateObjects(canonInUse, pos, Quaternion.identity, pooledCanonLeft, playerID_);
        }
	}

    public void InstantiateObjects(string prefabId, Vector3 position, Quaternion rotation, List<GameObject> list, int playerID){

        if(!base.photonView.IsMine) return;

        GameObject go = PhotonNetwork.Instantiate(prefabId, position, rotation);
        go.GetComponent<CanonProjectile>().isActiveObject = false;
        go.GetComponent<CanonProjectile>().Init(playerID, false);
       // go.SetActive(false);
        list.Add(go);
    }

    public GameObject GetCanonRight(){
        for (int i = 0; i < pooledCanonRight.Count; i++)
        {
            if(!pooledCanonRight[i].activeInHierarchy){
                return pooledCanonRight[i];
            }
        }
        return null;
    }
    public GameObject GetCanonLeft(){
        for (int i = 0; i < pooledCanonLeft.Count; i++)
        {
            if(!pooledCanonLeft[i].activeInHierarchy){
                return pooledCanonLeft[i];
            }
        }
        return null;
    }
}
