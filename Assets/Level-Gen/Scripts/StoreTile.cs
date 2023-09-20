using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreTile : MonoBehaviour
{
    [SerializeField] protected itemCategory spawnCategory;
    [SerializeField] protected Vector3[] spawns = new Vector3[1] { Vector3.zero };
}
