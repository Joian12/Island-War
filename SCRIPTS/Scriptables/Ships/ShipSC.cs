using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Settings", menuName = "Ship")]
public class ShipSC : ScriptableObject
{
    public string ShipName;
    public int ShipNumber;
    public GameObject ShipObject;
}
