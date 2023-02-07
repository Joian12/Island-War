using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Photon.Pun;

public class CanonProjectile : MonoBehaviourPun, IPunObservable
{   
    private Action<CanonProjectile> destroy; 
    public int playerID;
    public float canonBallDamage;
    public Rigidbody rb;
    private Vector3 networkPosition;
    private Quaternion networkRotation;
    private float launchVelocity = 14f;
    public bool isActiveObject;

    public void Init(int playerID_, bool isActiveObject_){
        playerID = playerID_;
        PhotonNetwork.SendRate = 30; //Default is 30
        PhotonNetwork.SerializationRate = 10; //5 is really laggy, jumpy. Default is 10?
        gameObject.SetActive(isActiveObject);
    }

    private void OnTriggerEnter(Collider other)
    {   
        if(other.CompareTag("Water") || other.CompareTag("Ground")){
            photonView.RPC("DisableCanon", RpcTarget.All);
        }

        PhotonView photonView_ = other.gameObject.GetComponent<PhotonView>();
        if(photonView_ == null) return;
        if(!photonView_.IsMine) return;

        var damageble = other.GetComponent<IDamageble>();
        var shipStatus = other.GetComponent<ShipStatus>();
    
        if(damageble == null) return;
        if(shipStatus == null) return;

        if(shipStatus.playerID != playerID){
            Debug.Log("hitting enemy");
            //shipStatus.TakeDamage(canonBallDamage);
            shipStatus.photonView.RPC("TakeDamage", RpcTarget.All, canonBallDamage);
        }
    }

    public void OnDisable(){
        if(!base.photonView.IsMine) return;

        rb.velocity = Vector3.zero;
    }

    public void OnEnable(){
        rb.AddForce((1*transform.right)*launchVelocity, ForceMode.Impulse);
    }

    public void FixedUpdate(){
        if(!base.photonView.IsMine) return;
        
    
        rb.position = Vector3.MoveTowards(rb.position, rb.position+networkPosition, Time.fixedDeltaTime);
    }

    [PunRPC]
    public void DisableCanon(){
        gameObject.SetActive(false);
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
         if (stream.IsWriting)
        {
            stream.SendNext(rb.position);
            stream.SendNext(rb.rotation);
            stream.SendNext(rb.velocity);
            stream.SendNext(gameObject.activeInHierarchy);
        }
        else
        {
            networkPosition = (Vector3) stream.ReceiveNext();
            networkRotation = (Quaternion) stream.ReceiveNext();
            rb.velocity = (Vector3) stream.ReceiveNext();
            gameObject.SetActive((bool) stream.ReceiveNext());

            float lag = Mathf.Abs((float) (PhotonNetwork.Time - info.SentServerTime));
            networkPosition += (rb.velocity * lag);
        }
    }
}
