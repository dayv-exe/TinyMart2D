using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

// this script controls the ui of the arena panel, which is the main area game will be played in

public class ArenaScript : MonoBehaviour
{
  #region vars
  public Slider levelProgressbar;

  public float fSize = 110f; // the default size of score text font
  public TextMeshProUGUI scoreTxt; // score displayed in game
  public Button pauseBtn; // pause btn
  public PageNavigationScript nav; // games' central page navigation manager
  public TextMeshProUGUI currentLevelTxt;

  #endregion

  #region func
  // Start is called before the first frame update
  void Start()
  {
    // to pause game when clicked
    pauseBtn.onClick.AddListener(nav.pauseGame);
  }

  public void addToScore(int price)
  {
    cache.displayScore = cache.score;
    StartCoroutine(increaseOrDecreaseScore(cache.score, price + cache.score));
  }

  // Update is called once per frame
  void FixedUpdate()
  {
    levelProgressbar.value = cache.percentageOfCurrentLevelCompleted;
    scoreTxt.text = "$" + cache.displayScore;
    currentLevelTxt.text = cache.currentLevel + "";
  }

  IEnumerator increaseOrDecreaseScore(int startVal, int endValue)
  {
    // to add delay to the coin value change
    scoreTxt.fontSize = fSize;
    cache.displayScore = cache.score;
    cache.score = endValue;

    while (endValue != startVal)
    {
      startVal = startVal < endValue ? startVal + 1 : startVal - 1;
      cache.displayScore = startVal;
      scoreTxt.fontSize = fSize * 1.20f;

      yield return new WaitForSeconds(calcWaitTime(startVal, endValue));

      scoreTxt.fontSize = fSize;
      // end coroutine when completed
      if (startVal == endValue)
      {
        StopCoroutine(increaseOrDecreaseScore(0, 0));
      }
    }
  }

  float calcWaitTime(int startVal, int endValue)
  {
    // to calculate the speed of adding or removing coins animation
    return (.5f / Mathf.Abs(endValue - startVal)) < Mathf.Infinity ? .5f / Mathf.Abs(endValue - startVal) : 1f;
  }

  #endregion
}
