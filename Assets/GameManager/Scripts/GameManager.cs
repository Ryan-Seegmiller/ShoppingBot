using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using enemymanager;
using Items;
using PlayerContoller;

//bug when adding reference to level gen assembly
public class GameManager : MonoBehaviour
{
    #region Singleton
    public static GameManager instance;
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
    #endregion

    public bool gameActive = false;
    public PlayerMovement player;

    //public Transform elevatorPoint;
    public Animator elevatorAnim;
    /*
    public Vector3 starRatingsPerTime = new Vector3(180, 300, 500);//Time in seconds less than x for 3 star, between x and y for 2, more than y for 1, more than z for 0.
    public int TimePenaltyPerMissedItemPerDollar = 15;
    public float finalCalculatedTime = 0;
    */
    //GAME SCORING : 
    // time<180=3star
    // time>180 time < 500=2star
    // time>500=1star
    //EACH MISSED ITEM = X SECOND PENALTY. Possibly scale by price?


    #region UIEvents
    //Call from main menu button
    public void StartGame()
    {
        GameStart();
    }
    public void EndGame()
    {
        GameEnd();
    }
    public void QuitGame()
    {
        if (Application.isEditor) { Debug.Break(); }
        else { Application.Quit(); }
    }
    #endregion

    #region Elevator
    public void OnPlayerEnterElevator()
    {
        if (gameRules.gameTime > 60)
        {
            GameEnd();
        }
    }
    public void OnPlayerExitElevator() 
    {

    }
    #endregion

    #region GameRules
    public struct GameRules
    {
        public int waveCount;
        public float wavePeriod;
        public float gameStartTime; 
        public float gameTime
        {
            get { return Time.time - gameStartTime; }
            private set { }
        }
        public GameRules(float time)
        {
            waveCount = 0;
            wavePeriod = 0;
            gameStartTime = time;
        }
    }
    private GameRules gameRules;
    private void GameStart()
    {
        LevelGen.LevelManager.instance.InstanceMall();
        gameActive = true;
        gameRules = new GameRules(Time.time);
        StartCoroutine(Clock());
    }
    private void GameUpdate()
    {
        if (gameRules.waveCount < (int)(gameRules.gameTime / gameRules.wavePeriod))
        {
            // spawn enemies
        }
    }
    private void GameEnd()
    {
        Debug.Log("GameEnd()");
        LevelGen.LevelManager.instance.DeleteLevel(false);
        UIChanger.instance.SetSceneScoring();
        StopCoroutine(Clock());
    }
    #endregion

    private void Start()
    {
        EnemyManager.instance.maxEnemies = 20;
        player.gameObject.SetActive(true);
        LevelGen.LevelManager.instance.InstanceElevatorShaft();
    }
    private void Update()
    {
        GameUpdate();
    }

    #region Coroutine
    IEnumerator Clock()
    {
        while (true)
        {
            EnemyManager.instance.time = gameRules.gameTime;
            TimeCalc.instance.timer = (int)(gameRules.gameTime * 100);
            yield return new WaitForSeconds(.01f);
        }
    }
    #endregion

    //WHEN PLAYER ENTERS ELEVATOR TRIGGER COLLIDER
    //code for player
    void AttemptEndRound()
    {/*
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
            

        }*/
    }
}