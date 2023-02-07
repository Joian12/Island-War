using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using Photon.Pun;

public class PoolingManager : MonoBehaviour
{
    public CanonProjectile[] canonPrefab_;
    public ObjectPool<CanonProjectile> canonPool;

    private void Awake(){
        canonPool = new ObjectPool<CanonProjectile>(() => {
            return Instantiate(canonPrefab_[0]);
        }, shape => {
            shape.gameObject.SetActive(true);
        }, shape => {
            shape.gameObject.SetActive(false);
        }, shape => {
            Destroy(shape.gameObject);
        }, false, 50, 100);
    }
}
