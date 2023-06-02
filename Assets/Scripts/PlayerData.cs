using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// part of save system

[System.Serializable]
public class PlayerData
{
  public int highscore = 0;
  public int currentLevel = 1;
  public int chosenBackgroundColor = 0;
  public bool enabledSFX = true;
  public int chosenCart = 0;
  public int coins = 0;
  public int[] unlockedCarts = new[] { 0 };

  public PlayerData()
  {
    highscore = cache.highscore;
    currentLevel = cache.currentLevel;
    chosenBackgroundColor = cache.chosenBackgroundColor;
    enabledSFX = cache.enabledSFX;
    unlockedCarts = cache.unlockedCarts;
    chosenCart = cache.chosenCart;
    coins = cache.coins;

  }
}
