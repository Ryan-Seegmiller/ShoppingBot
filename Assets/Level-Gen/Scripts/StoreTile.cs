using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Items;

namespace LevelGen
{
    public class StoreTile : MonoBehaviour
    {
        private Transform itemParent;
        private Vector3 lightFixturePos = new Vector3(0f, 4.9f, 0f); //The spawn position of the light fixture
        [SerializeField] protected GameObject itemPrefab;
        [SerializeField] protected ItemCategory spawnCategory;
        [SerializeField] protected bool randomCategory = false;
        [SerializeField, Range(0f, 1f)] protected float spawnLightChance = 0.3f; //The chance of a light fixture spawning
        [SerializeField] protected GameObject lightFixturePrefab;
        [SerializeField, Range(0f, 1f)] protected float spawnerChance = .01f;
        public Vector3[] spawns = new Vector3[1] { Vector3.zero };

        public void SpawnItems()
        {
            if (ItemManager.instance == null) { Debug.LogWarning("StoreTile.SpawnItems() :: ItemManager.instance == null", this); return; }
            if (ItemManager.instance.itemFactory == null) { Debug.LogWarning("StoreTile.SpawnItems() :: ItemManager.instance.itemFactory == null", this); return; }
            if (LevelManager.instance == null) { Debug.LogWarning("StoreTile.SpawnItems() :: LevelManager.instance == null", this); return; }

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
                float num = Random.Range(0f, 1f);
                if (num > LevelManager.instance.itemSpawnChance) { continue; }
                Vector3 pos = transform.position + transform.InverseTransformVector(spawns[i]);
                ItemManager.instance.itemFactory.InstanceItem<ItemCategory>(spawnCategory, pos);
            }
        }
        public void DestroyItems()
        {
            if (itemParent == null) { return; }
            DestroyImmediate(itemParent.gameObject);
        }

        //Spawn light fixtures
        public void SpawnLighting()
        {
            float num = Random.Range(0f, 1f);
            if (num < spawnLightChance)
            {
                GameObject newLight = Instantiate(lightFixturePrefab, transform); //Instantiate light
                newLight.transform.position = transform.position + lightFixturePos; //Set light's position
            }
        }
        public void SpawnSpawner()
        {
            float num = Random.Range(0f, 1f);
            if (num < spawnerChance)
            {
                GameObject spawner = new GameObject("Spawner");
                spawner.transform.SetParent(transform);
                spawner.transform.localPosition = new Vector3(0, 1, 0);
                spawner.tag = "Respawn";
            }
        }

        private void Start()
        {
            SpawnItems();
            SpawnLighting();
            SpawnSpawner();
        }
    }
}































































//lol hi Hawi

















































































































































































































































































































































































































































































































































//hello again Hawi













































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































































//Ryan and Joel were here
















































































//Lol made u look