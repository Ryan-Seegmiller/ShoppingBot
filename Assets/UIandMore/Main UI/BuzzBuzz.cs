using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class BuzzBuzz : MonoBehaviour
{
    [SerializeField] Sprite sp1;
    [SerializeField] Sprite sp2;

    [SerializeField] Image display;

    [SerializeField] float WaitTime;

    bool first = true;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnEnable()
    {
        StartCoroutine(Buzz());
    }

    IEnumerator Buzz()
    {
        while (true)
        {
            yield return new WaitForSeconds(WaitTime);
            if (first)
            {
                display.sprite = sp2;
                first = false;
            }
            else
            {
                display.sprite = sp1;
                first = true;
            }
        }
    }
}
