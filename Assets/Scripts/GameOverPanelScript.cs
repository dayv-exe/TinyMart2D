using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// this script controls the game over panel

public class GameOverPanelScript : MonoBehaviour
{
  #region vars
  public PageNavigationScript nav;
  public Image spilledItemImg; // the sprite renderer for the item that player failed to collect (the gameOver animation)
  public GameObject newBestPnl; // new best gameobject
  public TMPro.TextMeshProUGUI scoreTxt, highScoreTxt;

  #endregion

  #region func
  void Start()
  {
    nav.showPrompt(gameObject.transform, "swipe to continue", false);
  }

  public void setGameOverAnim(int score, int highScore, Sprite spilledItem, bool isNewBestScore, GameObject gameOverAnim)
  {
    // set the details of the last game
    scoreTxt.text = "$" + score;
    highScoreTxt.text = "$" + highScore;
    spilledItemImg.sprite = spilledItem;

    StartCoroutine(showGameOverConfettiBlastAfter(isNewBestScore, 1.025f, gameOverAnim));
  }

  IEnumerator showGameOverConfettiBlastAfter(bool isNewBestScore, float secondsToWait, GameObject gameOverAnim)
  {
    yield return new WaitForSeconds(secondsToWait);

    GameObject g = Instantiate(gameOverAnim) as GameObject;
    g.transform.position = new Vector3(0, 1.5f, 1);

    newBestPnl.SetActive(isNewBestScore);
  }

  void FixedUpdate()
  {
    if (nav.playerSwipedToContinue())
    {
      // go back to main menu after player taps to continue
      backToMainMenu();
    }
  }

  public void backToMainMenu()
  {
    nav.showPage("MainMenu", .75f, true, new[] { "Arena", "GameOver" });
    cache.score = 0;
    cache.displayScore = 0;
  }
  #endregion
}
