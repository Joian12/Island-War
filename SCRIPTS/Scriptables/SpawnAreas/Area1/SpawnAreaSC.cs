using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SpawnArea", menuName = "SpawnArea")]
public class SpawnAreaSC : ScriptableObject
{
    public int SpawnID;
    public bool IsUsed;
    public Transform SpawnTransform;
}
