using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using audio;

public class BotHover : MonoBehaviour
{
    [SerializeField] Sprite sprite;
    [SerializeField] Sprite highlightSprite;
    bool on;

    [SerializeField] Image img;

    public void Toggle()
    {
        if (on)
        {
            img.sprite = sprite;
            on = false;
        }
        else
        {
            AudioManager.instance.PlaySound2D(6);
            img.sprite = highlightSprite;
            on = true;
        }
    }
}
