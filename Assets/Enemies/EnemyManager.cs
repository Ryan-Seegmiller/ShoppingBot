using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager instance;
    public List<GameObject> enemySpawns = new List<GameObject>();
    public List<GameObject> enemyPrefabs = new List<GameObject>();
    public List<Enemy> currentEnemies=new List<Enemy>();
    // Start is called before the first frame update
    void Start()
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

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SpawnEnemies(int count)
    {
        for(int i=0; i<count; i++)
        {
            Enemy e=Instantiate(enemyPrefabs[Random.Range(0, enemyPrefabs.Count)], enemySpawns[Random.Range(0, enemySpawns.Count)].transform.position, Quaternion.identity).GetComponent<Enemy>();
            currentEnemies.Add(e);
        }
    }

    //subject to change
    //This script will determine based on score/money wether or not to spawn enemies 
}
