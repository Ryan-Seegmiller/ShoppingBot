using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager instance;
    public List<GameObject> enemySpawns = new List<GameObject>();
    public List<GameObject> enemyPrefabs = new List<GameObject>();
    public List<Enemy> currentEnemies=new List<Enemy>();
    public float aerialEnemyHeight =3;
    public float groundEnemyHeight=0.5f;
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

        SpawnEnemies(50,0);
        SpawnEnemies(50,1);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SpawnEnemies(int count, int index)
    {
        for(int i=0; i< count; i++)
        {
            Enemy e=Instantiate(enemyPrefabs[index], enemySpawns[Random.Range(0, enemySpawns.Count)].transform.position, Quaternion.identity).GetComponent<Enemy>();
            currentEnemies.Add(e);
        }
    }

    //subject to change
    //This script will determine based on score/money wether or not to spawn enemies 
}
