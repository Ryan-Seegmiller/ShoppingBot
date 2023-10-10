using UnityEngine;
using Items;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

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
    }

    // Update is called once per frame
    void Update()
    {

    }
}