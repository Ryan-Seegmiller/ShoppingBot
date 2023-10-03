using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerTest : MonoBehaviour
{
    public static PlayerControllerTest instance;
    // Start is called before the first frame update
    void Start()
    {
        if(instance == null)
            instance = this;
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
