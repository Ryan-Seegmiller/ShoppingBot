using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Items;

namespace LevelGen
{
    public class StoreTile : MonoBehaviour
    {
        private Transform itemParent;
        [SerializeField] protected GameObject itemPrefab;
        [SerializeField] protected ItemCategory spawnCategory;
        [SerializeField] protected bool randomCategory = false;
        public Vector3[] spawns = new Vector3[1] { Vector3.zero };

        public void SpawnItems()
        {
            if (ItemManager.instance == null) { Debug.LogWarning("StoreTile.SpawnItems() :: ItemManager.instance == null", this); return; }
            if (ItemManager.instance.itemFactory == null) { Debug.LogWarning("StoreTile.SpawnItems() :: ItemManager.instance.itemFactory == null", this); return; }

            if (randomCategory)
            {
                int num = System.Enum.GetNames(typeof(ItemCategory)).Length;
                spawnCategory = (ItemCategory)Random.Range(0, 2);
            }
            itemParent = new GameObject("ItemParent").transform;
            itemParent.SetParent(transform);
            itemParent.localPosition = Vector3.zero;
            for (int i = 0; i < spawns.Length; i++)
            {
                Vector3 pos = transform.position + transform.InverseTransformVector(spawns[i]);
                ItemManager.instance.itemFactory.InstanceItem<ItemCategory>(spawnCategory, pos);
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
