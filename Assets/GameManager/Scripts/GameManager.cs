using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using enemymanager;
using Items;
using PlayerContoller;

//bug when adding reference to level gen assembly
public class GameManager : MonoBehaviour, UIEvents
{
    #region Singleton
    public static GameManager instance;
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            UIEvents.instance = this;
        }
        else
        {
            Destroy(this);
        }
    }
    #endregion

    public bool gameActive = false;
    public PlayerMovement player;
    [Range(1, 5)] public int enemyMultiplyer = 1;

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
    // Menu 
    public void StartGame()
    {
        GameStart();
        Debug.Log("UIEvents :: Start game", this);
    }
    // TODO: pause/unpause functionality
    public void PauseGame()
    {
        
    }
    public void ContinueGame()
    {

    }
    public void StopGame()
    {
        // TODO: Stop Game
        GameStop();
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
        if (gameRules.gameTime > 10)
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
        // init
        Debug.Log("GameManager :: Game is starting", this);
        LevelGen.LevelManager.instance.InstanceMall(); // level
        ItemManager.instance.RandomiseList(); // shopping list
        // player
        ResetPlayer();
        EnemyManager.instance.player = player.gameObject;
        player.GetComponentInChildren<Rigidbody>().isKinematic = false;
        // clock
        gameRules = new GameRules(Time.time);
        StartCoroutine(Clock());
        gameActive = true;
        gameRules.wavePeriod = 10;
    }
    private void GameUpdate()
    {
        if (gameRules.waveCount < (int)(gameRules.gameTime / gameRules.wavePeriod))
        {
            EnemyManager.instance.UpdateSpawners();
            // spawn enemies
            gameRules.waveCount++;
            EnemyManager.instance.SpawnEnemies(gameRules.waveCount * enemyMultiplyer, Random.Range(0, EnemyManager.instance.enemyPrefabs.Count));
            Debug.Log($"GameManager :: {gameRules.waveCount} enemy spawned at {gameRules.gameTime}", this);
        }
    }
    private void GameStop()
    {
        Debug.Log("GameManager :: Game is stopping (no points recevied)", this);
        ClockStop();
        player.backupCameraCanvas.SetActive(false);
        ItemManager.instance.DestroyItems();
        EnemyManager.instance.DestroyEnemies();
        LevelGen.LevelManager.instance.DeleteLevel(false);
    }
    private void GameEnd()
    {
        Debug.Log("GameManager :: Game is ending", this);
        // TODO: save score
        StopCoroutine(Clock());
        player.backupCameraCanvas.SetActive(false);
        UIChanger.instance.SetSceneScoring();
        ItemManager.instance.DestroyItems();
        EnemyManager.instance.DestroyEnemies();
        LevelGen.LevelManager.instance.DeleteLevel(false);
    }
    #endregion

    #region Clock
    protected void ClockStart()
    {
        gameRules.gameStartTime = Time.time;
        gameRules.holdTime = 0;
        StartCoroutine(Clock());
        Debug.Log("GameManager :: Clock started", this);
    }
    protected void ClockContinue()
    {
        gameRules.gameStartTime = Time.time - gameRules.holdTime;
        gameRules.holdTime = 0;
        StartCoroutine(Clock());
        Debug.Log("GameManager :: Clock continued", this);
    }
    protected void ClockStop()
    {
        gameRules.holdTime = gameRules.gameTime;
        StopCoroutine(Clock());
        Debug.Log("GameManager :: Clock stopped", this);
    }
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

    private void ResetPlayer()
    {
        if (player == null) { player = FindObjectOfType<PlayerMovement>(); }
        player.backupCameraCanvas.SetActive(true);
        player.transform.position = new Vector3(-2.5f, 2, -2.5f);
        player.transform.rotation = Quaternion.identity;
    }

    private void Start()
    {
        EnemyManager.instance.maxEnemies = 20;
        LevelGen.LevelManager.instance.InstanceElevatorShaft();
        if (player == null) { player = FindObjectOfType<PlayerMovement>(); }
        player.backupCameraCanvas.SetActive(false);
    }
    private void Update()
    {
        GameUpdate();
    }
}