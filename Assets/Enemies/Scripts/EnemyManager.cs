using System.Collections.Generic;
using UnityEngine;

namespace enemymanager
{
    public class EnemyManager : MonoBehaviour
    {
        public static EnemyManager instance;
        public GameObject player;
        protected List<Transform> enemySpawns = new List<Transform>();
        public List<GameObject> enemyPrefabs = new List<GameObject>();
        public List<EnemyBase> currentEnemies = new List<EnemyBase>();
        protected int enemiesSpawnQueue = 0;
        public int maxEnemies = 15;
        public float spawnPositionOffset = 1;

        public bool PauseEnemies=false;
        public float time = 0;
        public int multiplierOverride = 1;
        void Awake()
        {
            if (instance == null) { instance = this; } else { Destroy(this); }
            UpdateSpawners();
        }
        public void UpdateSpawners()
        {
            enemySpawns.Clear();
            GameObject[] spawners = GameObject.FindGameObjectsWithTag("Respawn");
            for (int i = 0; i < spawners.Length; i++) { enemySpawns.Add(spawners[i].transform); }
        }
        public void SpawnEnemies(int count, int index)//number of enemies to spawn, then index 0-2 for which type of enemy
        {
            if (currentEnemies.Count < maxEnemies)
            {
                count *= multiplierOverride;
                for (int i = 0; i < count; i++)
                {
                    //spawns the enemy at the position of the spawn transform +- the position offset, at quaternion.identity
                    Vector3 spawnPos = enemySpawns[Random.Range(0, enemySpawns.Count)].transform.position;
                    spawnPos.y = player.transform.position.y;
                    currentEnemies.Add(Instantiate(enemyPrefabs[index], spawnPos, Quaternion.identity).GetComponent<EnemyBase>());
                }
            }
        }
        public void DestroyEnemies()
        {
            for (int i = currentEnemies.Count; i > 0; i--)
            {
                Destroy(currentEnemies[i - 1].gameObject);
            }
            currentEnemies.Clear();
        }
    }
}
