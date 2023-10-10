using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace enemymanager
{
    public class EnemyManager : MonoBehaviour
    {
        public static EnemyManager instance;
        protected List<GameObject> enemySpawns = new List<GameObject>();
        protected List<GameObject> enemyPrefabs = new List<GameObject>();
        public List<EnemyBase> currentEnemies = new List<EnemyBase>();
        protected int airEnemies;
        protected int groundEnemies;
        protected int crawlerEnemies;
        protected int enemiesSpawnQueue = 0;
        protected float time = 0;
        protected float lastCheckTime = 0;
        protected float lastSpawnTime = 0;
        protected float spawnDelay = 5;

        void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(this);
            }
        }
        public void SpawnEnemies(int count, int index)
        {
            for (int i = 0; i < count; i++)
            {
                EnemyBase e = Instantiate(enemyPrefabs[index], enemySpawns[UnityEngine.Random.Range(0, enemySpawns.Count)].transform.position + new Vector3(UnityEngine.Random.Range(-1, 1), 0, Random.Range(-1, 1)), Quaternion.identity).GetComponent<EnemyBase>();
                currentEnemies.Add(e);
            }
        }
        public void Update()
        {
            time += Time.deltaTime;
            if (time > lastCheckTime + 1)
            {
                lastCheckTime = time;
                enemiesSpawnQueue = enemySpawns.Count - currentEnemies.Count;
                if (enemiesSpawnQueue > 0 && time > lastSpawnTime + spawnDelay)
                {
                    lastSpawnTime = time;
                    SpawnEnemies(1, Random.Range(0, enemyPrefabs.Count));
                }
            }
        }
    }
}
