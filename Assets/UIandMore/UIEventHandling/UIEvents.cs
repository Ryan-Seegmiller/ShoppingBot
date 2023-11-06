using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface UIEvents
{
    public static UIEvents instance { get; }
    public void StartGame();
    public void StopGame();
    public void EndGame();
    public void QuitGame();
}
