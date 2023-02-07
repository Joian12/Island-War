using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;
using Photon.Pun;
using Photon.Realtime;


public class Inputs_ : MonoBehaviourPunCallbacks, IPunObservable{
    
    public PlayerInput playerInput; 
    public Vector2 look, move;
    //public References ref_;
    public float speed;
    public Vector3 lookOffset;
    public Rigidbody rb;
     public bool isForward = false;
    public bool isReverse = false;
    public Vector3 spawnArea;
    public CinemachineFreeLook cnmFL;
    private CanonRayCast canonRayCast;

    private Vector3 networkPosition;
    private Quaternion networkRotation;
    private ShipStatus _shipStatus;
    private GameObject _childShip;
    private ShipCanvas _shipCanvas;

    public void Init(CinemachineFreeLook cnmFL_, ShipCanvas shipCanvas_, ShipStatus shipStatus_, CanonRayCast canonRayCast_, GameObject childShip_)
    {   
        _childShip = childShip_;
        cnmFL = cnmFL_;
        canonRayCast = canonRayCast_;
        rb = GetComponent<Rigidbody>();
        cnmFL.LookAt = transform;
        cnmFL.Follow = transform;

        PhotonNetwork.SendRate = 30; //Default is 30
        PhotonNetwork.SerializationRate = 10; //5 is really laggy, jumpy. Default is 10?

        _shipStatus = shipStatus_;
        speed = shipStatus_.shipSettingSC.shipSpeed;

        if(shipCanvas_ != null){
            _shipCanvas = shipCanvas_;
            shipCanvas_.forward.onClick.AddListener(delegate {ToggleForward();});
            shipCanvas_.reverse.onClick.AddListener(delegate {ToggleReverse();});
            shipCanvas_.shootProjectileRight.onClick.AddListener(delegate {ShootCanonRight(shipStatus_);});
            shipCanvas_.shootProjectileLeft.onClick.AddListener(delegate {ShootCanonLeft(shipStatus_);});
        }
        
        spawnArea = new Vector3(Random.Range(-10, 10), 0.5f, Random.Range(-10, 10));
        transform.position = spawnArea;
    }


    void FixedUpdate(){ 

        if(!base.photonView.IsMine) return;
    
        if(_shipStatus.isAlive){
            Movement();
            lastRotation = _childShip.transform.localRotation;
        }
    }

    void Update(){
        if(!base.photonView.IsMine) return;

        if(!_shipStatus.isAlive){
            SinkingShip();
        }
    }

    private void Movement(){
        move = playerInput.actions["Movement"].ReadValue<Vector2>();
        look = playerInput.actions["FreeLook"].ReadValue<Vector2>();

        rb.AddTorque(move.x * transform.up * speed);
        _shipCanvas.TurnWheel(move.x);
        cnmFL.m_XAxis.Value = look.x;

        if(isForward){
            rb.AddForceAtPosition(((1 * transform.forward) + networkPosition) * speed, transform.position);
            // _shipCanvas.TurnWheel(6f);
        }

        if(isReverse){
            rb.AddForceAtPosition(((-1 * transform.forward) + networkPosition)  * speed, transform.position);
        }
        
        rb.position = Vector3.MoveTowards(rb.position, rb.position+networkPosition, Time.fixedDeltaTime);
    }

    public float lerpTime = 15f;
    private float currentLerpTime;
    public Quaternion sinkPosition, lastRotation;

    public void SinkingShip(){
        float valueX = -23f;
        float valueZ = 118.6f;
        sinkPosition = Quaternion.Euler(-23f, 0, 118.6f);
    
        currentLerpTime += Time.deltaTime;
        if (currentLerpTime > lerpTime) { currentLerpTime = lerpTime; }

        float t = currentLerpTime / lerpTime;
        _childShip.transform.localRotation = Quaternion.Slerp(lastRotation, sinkPosition, t);
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) { 
        
        if (stream.IsWriting)
        {
            stream.SendNext(rb.position);
            stream.SendNext(rb.rotation);
            stream.SendNext(rb.velocity);
        }
        else
        {
            networkPosition = (Vector3) stream.ReceiveNext();
            networkRotation = (Quaternion) stream.ReceiveNext();
            rb.velocity = (Vector3) stream.ReceiveNext();

            float lag = Mathf.Abs((float) (PhotonNetwork.Time - info.SentServerTime));
            networkPosition += (rb.velocity * lag);
        }
    }

    void ToggleForward(){
        isReverse = false;
        isForward = !isForward;
    }

    void ToggleReverse(){
        isForward = false;
        isReverse = !isReverse;
    }
    
    public void ShootCanonRight(ShipStatus shipStatus_){
        shipStatus_.FireCannonsRight();
    }

    public void ShootCanonLeft(ShipStatus shipStatus_){
        shipStatus_.FireCannonsLeft();
    }
}
