using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    GameManager Instance;

    //PLAYER STATS
    public int cash = 100; //Player cash


    void Start()
    {
        //Singleton
        if (Instance == null) {
            Instance = this;
        } else {
            Destroy(this); }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
