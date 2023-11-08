using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using enemymanager;
using Items;
using PlayerContoller;
using audio;

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

    // TODO: elevator animation
    //public Transform elevatorPoint;
    public Animator elevatorAnim;


    #region UIEvents
    // Menu 
    public void StartGame()
    {
        GameStart();
        Debug.Log("UIEvents :: Start game", this);
    }
    public void PauseGame()
    {
        ClockStop();
        AudioManager.instance.PlaySound2D(0);
        player.backupCameraCanvas.SetActive(false);
    }
    public void ContinueGame()
    {
        ClockContinue();
        player.backupCameraCanvas.SetActive(true);
    }
    public void StopGame()
    {
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
        public float wavePeriod; // how often enemies will spawn
        public float gameStartTime; // time the game started (may change if paused)
        [HideInInspector] public float holdTime; // time saved when we pause
        public float gameTime
        {
            get { return Time.time - gameStartTime; }
            private set { }
        }
        public GameRules(float holdTime)
        {
            this.waveCount = 0;
            this.wavePeriod = 0;
            this.holdTime = holdTime;
            this.gameStartTime = 0;
        }
    }
    private GameRules gameRules;
    private void GameStart()
    {
        // init
        Debug.Log("GameManager :: Game is starting", this);
        LevelGen.LevelManager.instance.InstanceMall(); // level
        ItemManager.instance.RandomiseList(); // shopping list
        EnemyManager.instance.UpdateSpawners();
        // player
        if (player == null) { player = FindObjectOfType<PlayerMovement>(); }
        player.backupCameraCanvas.SetActive(true);
        player.transform.position = new Vector3(-2.5f, 2, -2.5f);
        player.transform.rotation = Quaternion.identity;
        EnemyManager.instance.player = player.gameObject;

        // clock
        gameRules = new GameRules(0);
        ClockStart();
        gameActive = true;
        gameRules.wavePeriod = 10;
    }
    private void GameUpdate()
    {
        if (gameRules.waveCount < (int)(gameRules.gameTime / gameRules.wavePeriod))
        {
            // spawn enemies
            gameRules.waveCount++;
            EnemyManager.instance.SpawnEnemies(gameRules.waveCount, Random.Range(0, EnemyManager.instance.enemyPrefabs.Count));
            Debug.Log($"Enemy Spawned at {gameRules.gameTime}");
        }
    }
    private void GameStop()
    {
        ClockStop();
        player.backupCameraCanvas.SetActive(false);
        ItemManager.instance.DestroyItems();
        EnemyManager.instance.DestroyEnemies();
        LevelGen.LevelManager.instance.DeleteLevel(false);
    }
    private void GameEnd()
    {
        // TODO: save score
        ClockStop();
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
    }
    protected void ClockContinue()
    {
        gameRules.gameStartTime = Time.time - gameRules.holdTime;
        gameRules.holdTime = 0;
        StartCoroutine(Clock());
    }
    protected void ClockStop()
    {
        gameRules.holdTime = gameRules.gameTime;
        StopCoroutine(Clock());
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