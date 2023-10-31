using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace enemymanager
{
    public class EnemyManager : MonoBehaviour
    {
        public static EnemyManager instance;

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
                EnemyBase e = Instantiate(enemyPrefabs[index], enemySpawns[UnityEngine.Random.Range(0, enemySpawns.Count)].transform.position + new Vector3(Random.Range(-spawnPositionOffset, spawnPositionOffset), 0, Random.Range(-spawnPositionOffset, spawnPositionOffset)), Quaternion.identity).GetComponent<EnemyBase>();
                currentEnemies.Add(e);
            }
        }
        public void FixedUpdate()
        {
            if (time > lastCheckTime + 1 && time > gracePeriodTime) // Combination of fixed update and 1 second timer to reduce how often this is called/checked
            {
                lastCheckTime = time;
                enemiesSpawnQueue = enemySpawns.Count - currentEnemies.Count;//For how its currently setup, the amount of enemies it wants to spawn is the # of spawners minus the current amount of enemies.
                                                                             //IE 10 spawners - 5 current players means spawn 5 more enemies over time
                if (enemiesSpawnQueue > 0 && time > lastSpawnTime + spawnDelay && currentEnemies.Count<maxEnemies)//make sure the delay has passed, and that there is space for another enemy
                {
                    lastSpawnTime = time;
                    SpawnEnemies(1, Random.Range(0, enemyPrefabs.Count)); //spawn a singular random enemy
                }
            }
        }
    }
}
