using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LevelGen
{
    public class StoreTile : MonoBehaviour
    {
        private Transform itemParent;
        [SerializeField] protected GameObject itemPrefab;
        //[SerializeField] protected itemCategory spawnCategory;
        [SerializeField] protected bool randomCategory = false;
        public Vector3[] spawns = new Vector3[1] { Vector3.zero };

        public void SpawnItems()
        {
            if (randomCategory)
            {
                //int num = Enum.GetNames(typeof(itemCategory)).Length;
                //spawnCategory = (itemCategory)Random.Range(0, 2);
            }
            itemParent = new GameObject("ItemParent").transform;
            itemParent.SetParent(transform);
            itemParent.localPosition = Vector3.zero;
            for (int i = 0; i < spawns.Length; i++)
            {
                Vector3 pos = transform.position + transform.InverseTransformVector(spawns[i]);
                //GameObject go = Instantiate(itemPrefab, pos, Quaternion.identity, itemParent);
                //go.GetComponent<ItemValue>().RandomiseFromCategory(spawnCategory);
            }
        }
        public void DestroyItems()
        {
            if (itemParent == null) { return; }
            DestroyImmediate(itemParent.gameObject);
        }

        private void OnEnable()
        {
            if (itemParent == null)
            {
                SpawnItems();
            }
        }
    }
}
