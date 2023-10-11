using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using enemymanager;
using Items;
//bug when adding reference to level gen assembly
public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public bool gameActive = false;
    public Transform elevatorPoint;
    public GameObject player;//Refactor to playercontroller class/namespace when functional
    public GameObject playerPrefab;
    public EnemyManager enemyManager;
    public Animator elevatorAnim;
    void Awake()
    {
        //Singleton
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
        player.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {

    }
    //Call from main menu button
    public void StartGame()
    {
        gameActive = true;
        player.SetActive(true);
        player.transform.position = elevatorPoint.position;
        player.transform.rotation = Quaternion.identity;
        //5 of each enemy to start
        enemyManager.maxEnemies = 15;
        enemyManager.SpawnEnemies(5, 0);
        enemyManager.SpawnEnemies(5, 1);
        enemyManager.SpawnEnemies(5, 2);
        elevatorAnim.SetBool("state", gameActive);
    }
}