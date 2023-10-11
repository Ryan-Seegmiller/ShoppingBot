using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using enemymanager;
using Items;
using System.Linq;
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
    public GameObject elevatorViewCamera;
    public float GameTime = 0;
    public Vector3 starRatingsPerTime = new Vector3(180, 300, 500);//Time in seconds less than x for 3 star, between x and y for 2, more than y for 1, more than z for 0.
    public int TimePenaltyPerMissedItemPerDollar = 15;
    public float finalCalculatedTime = 0;
    int CurrentStars = 0;
    //GAME SCORING : 
    // time<180=3star
    // time>180 time < 500=2star
    // time>500=1star
    //EACH MISSED ITEM = X SECOND PENALTY. Possibly scale by price?

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
        //Camera that provides 3rd person view of doors closing from side angle
        elevatorViewCamera.SetActive(false);
    }

    void Update()
    {
        if (gameActive)
        {
            GameTime += Time.deltaTime;
        }
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

    //WHEN PLAYER ENTERS ELEVATOR TRIGGER COLLIDER
    //code for player


    void AttemptEndRound()
    {
        if (gameActive)
        {
            gameActive = false;
            player.transform.position = elevatorPoint.position;
            player.transform.rotation = Quaternion.identity;
            player.SetActive(gameActive);
            elevatorViewCamera.SetActive(!gameActive);
            elevatorAnim.SetBool("state", gameActive);
            //destroy all current enemies, clear list. Same for items
            for (int i = 0; i < enemyManager.currentEnemies.Count; i++)
                Destroy(enemyManager.currentEnemies[i].gameObject);
            enemyManager.currentEnemies.Clear();
            GameObject[] itemGOs = GameObject.FindGameObjectsWithTag("Item");
            for (int i = 0; i < itemGOs.Length; i++)
            {
                Destroy(itemGOs[i].gameObject);
            }
            /*
            //calculate sum of missed item penalties
            float totalPenalty = 0;
            for (int i = 0; i < ItemManager.instance.shoppingList.Length; i++)
            {
                //if the shopping list has an item that the inventory doesnt, add value*penalty to total penalty
                if (!ItemManager.instance.inventory.Contains(ItemManager.instance.shoppingList[i]))
                {   //cost of the item that is missing. shoppinglist[i] returns an int representing the item
                    totalPenalty += ItemManager.instance.itemCost[ItemManager.instance.shoppingList[i]] * TimePenaltyPerMissedItemPerDollar;
                }
            }
            finalCalculatedTime = GameTime + totalPenalty;
            //Get number of "stars" earned for time
            CurrentStars = 3;
            if (finalCalculatedTime > starRatingsPerTime.x)
                CurrentStars--;
            if (finalCalculatedTime > starRatingsPerTime.y)
                CurrentStars--;
            if (finalCalculatedTime > starRatingsPerTime.z)
                CurrentStars--;
            */

        }
    }
}