using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using enemymanager;
using Items;
using PlayerContoller;
using audio;
using LevelGen;

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
    [Range(1, 5)] public int enemyMultiplier = 1;

    private GameRules gameRules;

    #region GameRules
    public struct GameRules
    {
        public int waveCount;
        public float wavePeriod; // how often enemies will spawn
        public float gameStartTime;  // time the game started (may change if paused)
        [HideInInspector] public float holdTime; // time saved when we pause
        public float elevatorLockTime; // how long the elevator stays locked after the game starts
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
            this.elevatorLockTime = 30f;
        }
    }
    #endregion

    #region UIEvents
    // Menu 
    public void StartGame()
    {
        Debug.Log("UIEvents :: Start game", this);
        // init
        Debug.Log("GameManager :: Game is starting", this);
        try { LevelManager.instance.InstanceMall(); } catch (Exception e) { Debug.LogError(e.Message, this); } // level
        try { ItemManager.instance.RandomiseList(); } catch (Exception e) { Debug.LogError(e.Message, this); } // shopping list
        try { EnemyManager.instance.PauseEnemies = true; } catch (Exception e) { Debug.LogError(e.Message, this); }
        try { UnlockElevator(); } catch (Exception e) { Debug.LogError(e.Message, this); }
        // player
        try { ResetPlayer(); } catch (Exception e) { Debug.LogError(e.Message, this); }
        try { EnemyManager.instance.player = player.gameObject; } catch (Exception e) { Debug.LogError(e.Message, this); }
        try { player.GetComponentInChildren<Rigidbody>().isKinematic = false; } catch (Exception e) { Debug.LogError(e.Message, this); }
        // clock
        gameRules = new GameRules(0);
        ClockStart();
        gameActive = true;
        gameRules.wavePeriod = 10;
    }
    // TODO: pause/unpause functionality
    public void PauseGame()
    {
        Debug.Log("GameManager :: Pause game", this);
        ClockStop();
        gameActive = false;
        try { AudioManager.instance.PlaySound2D(0); } catch (Exception e) { Debug.LogError(e.Message, this); }
        try { EnemyManager.instance.PauseEnemies = true; } catch (Exception e) { Debug.LogError(e.Message, this); }
        try { player.backupCameraCanvas.SetActive(false); } catch (Exception e) { Debug.LogError(e.Message, this); }
    }
    public void ContinueGame()
    {
        Debug.Log("GameManager :: Continue game", this);
        ClockContinue();
        gameActive = true;
        try { EnemyManager.instance.PauseEnemies = false; } catch (Exception e) { Debug.LogError(e.Message, this); }
        player.backupCameraCanvas.SetActive(true);
    }
    public void StopGame()
    {
        Debug.Log("GameManager :: Stop game", this);
        ClockStop();
        gameActive = false;
        try { player.backupCameraCanvas.SetActive(false); } catch (Exception e) { Debug.LogError(e.Message, this); }
        try { ItemManager.instance.DestroyItems(); } catch (Exception e) { Debug.LogError(e.Message, this); }
        try { EnemyManager.instance.DestroyEnemies(); } catch (Exception e) { Debug.LogError(e.Message, this); }
        try { LevelManager.instance.DeleteLevel(false); } catch (Exception e) { Debug.LogError(e.Message, this); }
    }
    public void EndGame()
    {
        Debug.Log("GameManager :: End Game", this);
        // TODO: save score
        ClockStop();
        gameActive = false;
        try { player.backupCameraCanvas.SetActive(false); } catch (Exception e) { Debug.LogError(e.Message, this); }
        try { UIChanger.instance.SetSceneScoring(); } catch (Exception e) { Debug.LogError(e.Message, this); }
        try { ItemManager.instance.DestroyItems(); } catch (Exception e) { Debug.LogError(e.Message, this); }
        try { EnemyManager.instance.DestroyEnemies(); } catch (Exception e) { Debug.LogError(e.Message, this); }
        try { LevelManager.instance.DeleteLevel(false); } catch (Exception e) { Debug.LogError(e.Message, this); }
    }
    public void QuitGame()
    {
        Debug.Log("UIEvents :: Quit game", this);
        if (Application.isEditor) { Debug.Break(); }
        else { Application.Quit(); }
    }
    #endregion

    #region Elevator
    public void OnPlayerEnterElevator()
    {
        if (gameRules.gameTime > 10)
        {
            LockElevator();
            EndGame();
        }
    }
    public void OnPlayerExitElevator() 
    {
        if (gameRules.gameTime < 10)
        {
            LockElevator();
        }
    }
    private void LockElevator()
    {
        Elevator elevator = FindObjectOfType<Elevator>();
        elevator.LockElevator();
    }
    private void UnlockElevator()
    {
        Elevator elevator = FindObjectOfType<Elevator>();
        elevator.UnlockElevator();
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
        LevelManager.instance.InstanceElevatorShaft();
        if (player == null) { player = FindObjectOfType<PlayerMovement>(); }
        player.backupCameraCanvas.SetActive(false);
    }
    private void Update()
    {
        if (gameRules.waveCount < (int)(gameRules.gameTime / gameRules.wavePeriod) && gameActive)
        {
            try { EnemyManager.instance.UpdateSpawners(); } catch (Exception e) { Debug.LogError(e.Message, this); }
            // spawn enemies
            gameRules.waveCount++;
            try { EnemyManager.instance.SpawnEnemies(gameRules.waveCount * enemyMultiplier, UnityEngine.Random.Range(0, EnemyManager.instance.enemyPrefabs.Count)); } catch (Exception e) { Debug.LogError(e.Message, this); }
            Debug.Log($"GameManager :: {gameRules.waveCount} enemy spawned at {gameRules.gameTime}", this);
        }
        if (gameRules.gameTime > gameRules.elevatorLockTime && gameActive) { UnlockElevator(); }
    }
}