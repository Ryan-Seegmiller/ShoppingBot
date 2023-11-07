using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace enemymanager
{
    public class EnemyManager : MonoBehaviour
    {
        public static EnemyManager instance;
        public GameObject player;
        public Transform spawnObject;
        protected List<Transform> enemySpawns = new List<Transform>();
        public List<GameObject> enemyPrefabs = new List<GameObject>();
        public List<EnemyBase> currentEnemies = new List<EnemyBase>();
        protected int enemiesSpawnQueue = 0;
        public int maxEnemies = 15;
        public float spawnPositionOffset = 1;
        #region Timers
        public float time = 0; //TIME IS ONLY SET BY THE GAMEMANAGER SCRIPT.
        public float gracePeriodTime = 30;
        protected float lastCheckTime = 0;
        protected float lastSpawnTime = 0;
        public float spawnDelay = 15;//Spawn delay is the time it takes for the spawner to produce a new enemy after its previous enemy was destroyed.
        #endregion

        void Awake()
        {
            if (instance == null) { instance = this; } else { Destroy(this); }
            enemySpawns.Clear();
            for(int i=0; i<spawnObject.transform.childCount; i++) { enemySpawns.Add(spawnObject.transform.GetChild(i)); }
        }
        public void SpawnEnemies(int count, int index)//number of enemies to spawn, then index 0-2 for which type of enemy
        {
            for (int i = 0; i < count; i++)
            {
                //spawns the enemy at the position of the spawn transform +- the position offset, at quaternion.identity
                Vector3 spawnPos = enemySpawns[UnityEngine.Random.Range(0, enemySpawns.Count)].transform.position + new Vector3(Random.Range(-spawnPositionOffset, spawnPositionOffset), player.transform.position.y* Random.Range(-spawnPositionOffset, spawnPositionOffset), Random.Range(-spawnPositionOffset, spawnPositionOffset));
                EnemyBase e = Instantiate(enemyPrefabs[index], spawnPos, Quaternion.identity).GetComponent<EnemyBase>();
                currentEnemies.Add(e);
            }
        }
        public void DestroyEnemies()
        {
            for (int i = currentEnemies.Count; i > 1; i--)
            {
                Destroy(currentEnemies[i].gameObject);
            }
            currentEnemies.Clear();
        }
    }
}
