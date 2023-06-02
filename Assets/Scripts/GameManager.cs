using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// this script runs the show

public class GameManager : MonoBehaviour
{
  #region var
  public bool DeleteSaveFilesOnStartUp = false; // always set to false in production!!!!
  public PageNavigationScript nav;
  public Image backgroundColor;
  BackgroundColorScript bgColorScript;
  public GroceriesManagerScript spawnScript;
  public GameObject coinBlastAnim, gameOverBlastAnim;
  ArenaScript arenaScrpt;
  #endregion

  #region func

  void Start()
  {
    if (DeleteSaveFilesOnStartUp)
    {
      cache.deleteGameFiles();
    }

    bgColorScript = backgroundColor.GetComponent<BackgroundColorScript>();
    /* ***********MAIN ENTRY POINT OF THE GAME************** */
    nav.LoadGame(() => nav.showPage("MainMenu", 1)); // the first page to be displayed after loading
    /*-------------------------------------------------------*/
  }

  public void setBackgroundColor()
  {
    // to set the current background color index to stored index
    bgColorScript.setBackgroundColor();
  }
  public void changeBackgroundColor()
  {
    // to change background color
    bgColorScript.changeBackgroundColor();
  }

  void OnApplicationFocus(bool hasFocus)
  {
    // pause game if app loses focus
    if (!hasFocus && cache.isSpawningItems)
    {
      nav.pauseGame();
    }

  }

  public void startGame()
  {
    // to switch to in game page and start spawning items
    arenaScrpt = nav.showPage("Arena", -1, false, pagesToBeRemoved: new string[] { "MainMenu" }).GetComponent<ArenaScript>();
    spawnScript.startSpawning();
  }

  public void startNextLevel()
  {
    // increase difficulty
    nav.removePage("LevelUp");
    spawnScript.startSpawning();
    spawnScript.increaseDifficultyLevel();
  }

  public void addToScore(int price)
  {
    arenaScrpt.addToScore(price);
  }

  public void GameOver(Sprite SpilledItemImg)
  {
    // spilled item img is the sprite of the item the player failed to collect

    bool newHighScore = cache.score > cache.highscore;

    // end game
    if (newHighScore)
    {
      cache.highscore = cache.score;
      cache.SaveGame();
    }

    spawnScript.stopSpawning(true);
    nav.showPage("GameOver", 1.25f, true).GetComponent<GameOverPanelScript>().setGameOverAnim(cache.score, cache.highscore, SpilledItemImg, newHighScore, gameOverBlastAnim);
  }

  public void levelUp()
  {
    spawnScript.stopSpawning(true);
    nav.showPage("LevelUp", 1.25f, true).GetComponent<LevelUpPanelScript>().coinBlastAnim = coinBlastAnim;
  }

  public void backToMainMenu(string callerPageName, string[] pagesToBeRemoved = null)
  {
    nav.removePage(callerPageName);
    nav.showPage("MainMenu", 1f, false, pagesToBeRemoved);
  }

  public void resurrectGame()
  {

  }
  #endregion
}
