using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface UIEvents
{
    public static UIEvents instance { get; set; }
    public void StartGame(); // starts a new game
    public void PauseGame(); // pauses the game
    public void ContinueGame(); // continues game from a paused state
    public void StopGame(); // stops the game without granting the player their points
    public void EndGame(); // ends the game and grants the player their points
    public void QuitGame(); // closes the applicatio
}
