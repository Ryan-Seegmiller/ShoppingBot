using System.Collections.Generic;
using UnityEngine;

namespace enemymanager
{
    public class EnemyManager : MonoBehaviour
    {
        public static EnemyManager instance;
        public GameObject player;
        public List<Transform> enemySpawns = new List<Transform>();
        public List<GameObject> enemyPrefabs = new List<GameObject>();
        public List<EnemyBase> allEnemies = new List<EnemyBase>();
        protected int enemiesSpawnQueue = 0;
        public int maxEnemies = 15;
        public float spawnPositionOffset = 1;
        public bool PauseEnemies=false;
        public float time = 0;
        void Awake()
        {
            if (instance == null) { instance = this; } else { Destroy(this); }
            UpdateSpawners();
        }
        private void Start()
        {
            CreateAllEnemies(15);
        }
        public void UpdateSpawners()
        {
            enemySpawns.Clear();
            GameObject[] spawners = GameObject.FindGameObjectsWithTag("Respawn");
            for (int i = 0; i < spawners.Length; i++) { enemySpawns.Add(spawners[i].transform); }
        }
        public void DeployDeadEnemy(int count)//number of enemies redeploy
        {
            for (int j = 0; j < count; j++)
            {
                for (int i = 0; i < allEnemies.Count; i++)
                {
                    if (allEnemies[i].deathTime == 0f) { continue; }//skip if not dead
                    else
                    {
                        Vector3 spawnPos = enemySpawns[Random.Range(0, enemySpawns.Count)].transform.position;
                        spawnPos.y = player.transform.position.y;
                        allEnemies[i].targetRespawnPosition = spawnPos;
                        allEnemies[i].Respawn();
                        break;
                    }

                }
            }
        }
        public void CreateAllEnemies(int count)
        {
            DestroyEnemies();
            for (int j = 0; j < count; j++)
            {
                Vector3 spawnPos = enemySpawns[Random.Range(0, enemySpawns.Count)].transform.position;
                spawnPos.y = player.transform.position.y;
                allEnemies.Add(Instantiate(enemyPrefabs[Random.Range(0, 3)], spawnPos, Quaternion.identity).GetComponent<EnemyBase>());
            }
        }
        public void DestroyEnemies()
        {
            for (int i = allEnemies.Count; i > 0; i--)
            {
                Destroy(allEnemies[i - 1].gameObject);
            }
            allEnemies.Clear();
        }
    }
}
