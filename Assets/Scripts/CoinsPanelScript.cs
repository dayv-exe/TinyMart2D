using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

// controls the coins counter

public class CoinsPanelScript : MonoBehaviour
{
  // ***note: only this script should be able to set the coin count***
  #region var
  public float fSize = 52f; // the default size of coin text font
  public TextMeshProUGUI coinText; // coin text
  public bool exitAnimComplete = false; // true when any running animation is complete
  bool hidePnl = false; // when true an animation to hide panel would be triggered
  Animator anim;
  #endregion

  #region func

  void Start()
  {
    anim = gameObject.GetComponent<Animator>();
  }

  float calcWaitTime(int startVal, int endValue)
  {
    // to calculate the speed of adding or removing coins animation
    return (.5f / Mathf.Abs(endValue - startVal)) < Mathf.Infinity ? .5f / Mathf.Abs(endValue - startVal) : 1f;
  }

  IEnumerator incrementCoins(int startVal, int endValue)
  {
    // to add delay to the coin value change
    coinText.fontSize = fSize;
    cache.coins = endValue;

    cache.SaveGame();

    while (endValue != startVal)
    {
      startVal = startVal < endValue ? startVal + 1 : startVal - 1;
      coinText.text = cache.ChangeNumber(startVal);
      coinText.fontSize = fSize + 15f;

      yield return new WaitForSeconds(calcWaitTime(startVal, endValue));

      coinText.fontSize = fSize + 5;
      // end coroutine when completed
      if (startVal == endValue)
      {
        StartCoroutine(panelTimeout(2f));
        StopCoroutine(incrementCoins(0, 0));
      }
    }
  }

  IEnumerator panelTimeout(float t)
  {
    // to hide the panel after some time
    yield return new WaitForSeconds(t);
    destroyCoinsPnl();
  }

  IEnumerator panelWaitOnDestruction(GameObject p)
  {
    while (true)
    {
      // to hide the panel after some time
      yield return new WaitForSeconds(.1f);
      if (p == null)
      {
        destroyCoinsPnl();
      }
    }
  }

  public void showCoins(float timeout = 4f)
  {
    // to show panel
    initCoins();
    StartCoroutine(panelTimeout(timeout));
  }

  void initCoins()
  {
    coinText.text = cache.ChangeNumber(cache.coins); // to set the coin panel text to the total amount of coins available
  }

  public void showCoinsUntilParentDestroyed(GameObject parent)
  {
    // to hide panel when a target gameobject is destroyed
    initCoins();
    StartCoroutine(panelWaitOnDestruction(parent));
  }

  public void addCoins(int amountOfCoinsToAdd)
  {
    // to start add coins animation and also add coins
    initCoins(); // if set in increment coins, increment coroutine would be stop before it starts
    StopAllCoroutines();
    StartCoroutine(incrementCoins(cache.coins, cache.coins + amountOfCoinsToAdd));
  }

  public void minusCoins(int amountOfCoinsToMinus)
  {
    // to start minus coins animation and also minus coins
    initCoins(); // if set in increment coins, increment coroutine would be stop before it starts
    StopAllCoroutines();
    StartCoroutine(incrementCoins(cache.coins, cache.coins - amountOfCoinsToMinus));
  }

  void destroyCoinsPnl()
  {
    // to trigger the exit animation
    StopAllCoroutines();
    hidePnl = true;
    anim.SetBool("hideCoins", true);
  }

  void FixedUpdate()
  {
    if (hidePnl && exitAnimComplete)
    {
      // to reset and hide the panel after exit animation is complete
      exitAnimComplete = false;
      hidePnl = false;
      gameObject.SetActive(false);
      anim.SetBool("hideCoins", true);
    }
  }

  #endregion
}
