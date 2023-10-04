using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager instance;
    public List<GameObject> enemySpawns = new List<GameObject>();
    public List<GameObject> enemyPrefabs = new List<GameObject>();
    public List<EnemyBase> currentEnemies=new List<EnemyBase>();
    public float aerialEnemyHeight =3;
    public float groundEnemyHeight=0.5f;
    public int airEnemies;
    public int groundEnemies;
    public int crawlerEnemies;
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

        SpawnEnemies(groundEnemies, 0);
        SpawnEnemies(airEnemies, 1);
        SpawnEnemies(crawlerEnemies, 2);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray r = Camera.main.ScreenPointToRay(Input.mousePosition);

            if(Physics.Raycast(r, out hit))
            {
                EnemyBase eb;
                if(hit.transform.gameObject.TryGetComponent<EnemyBase>(out eb))
                {
                    eb.Hit();
                    Debug.Log("God hit " + hit.transform.name);
                }
            }
        }
    }
    public void SpawnEnemies(int count, int index)
    {
        for(int i=0; i< count; i++)
        {
            EnemyBase e =Instantiate(enemyPrefabs[index], enemySpawns[Random.Range(0, enemySpawns.Count)].transform.position + new Vector3(Random.Range(-1,1),0,Random.Range(-1,1)), Quaternion.identity).GetComponent<EnemyBase>();
            currentEnemies.Add(e);
        }
    }
}
