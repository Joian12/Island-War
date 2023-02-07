using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanonRayCast : MonoBehaviour
{
    public Transform[] rightCanonHolder;
    public Transform[] leftCanonHolder;
    public GameObject[] allParticlesForRight;
    public GameObject[] allParticlesForLeft;
    private string[] layerString = {"Water", "Ground"};

    void Awake(){

    }

    public void ShootCanonLeft(){
        for (int i = 0; i < leftCanonHolder.Length; i++)
        {   
            RaycastHit _hit;
            var _physicsRaycast = Physics.Raycast(leftCanonHolder[i].position, transform.right, out _hit, 100f, LayerMask.GetMask(layerString));
            if(_physicsRaycast){
                allParticlesForLeft[i].transform.position = _hit.point;
                var shipStatus = _hit.transform.gameObject.GetComponent<IDamageble>();
                if(shipStatus == null) return;
                    shipStatus.TakeDamage(25);
            }
        }
    }

    public void ShootCanonRight(){
        for (int i = 0; i < rightCanonHolder.Length; i++)
        {   
            RaycastHit _hit;
            var _physicsRaycast = Physics.Raycast(rightCanonHolder[i].position, -transform.right, out _hit, 100f, LayerMask.GetMask(layerString));
            if(_physicsRaycast){
                allParticlesForRight[i].transform.position = _hit.point;
                var shipStatus = _hit.transform.gameObject.GetComponent<IDamageble>();
                if(shipStatus == null) return;
                    shipStatus.TakeDamage(25);
            }
        }
    }
}
