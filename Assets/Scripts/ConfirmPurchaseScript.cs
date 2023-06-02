using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

// this script controls a payment option popup

public class ConfirmPurchaseScript : MonoBehaviour
{
  #region vars
  public bool doneWithAnim = false; // if exit animation has finished playing
  public TextMeshProUGUI theTitle;
  public Button PayWithCoins, PayWithMoney;
  public PageNavigationScript nav;
  public bool unfreezeWhenDestroyed = true;
  #endregion

  #region func
  void Start()
  {
  }

  public void setOptions(string title, float worthInMoney, int worthInCoins = 0)
  {
    // to set the options for purchase
    theTitle.text = title;
    PayWithMoney.GetComponentInChildren<TextMeshProUGUI>().text = "$" + worthInMoney;

    if (worthInCoins < 1)
    {
      PayWithCoins.gameObject.SetActive(false);
      return;
    }

    PayWithCoins.gameObject.SetActive(true);
    PayWithCoins.GetComponentInChildren<TextMeshProUGUI>().text = worthInCoins + "";
  }

  public void destroyPurchaseBox()
  {
    if (!doneWithAnim) return;

    // removes the message and stops swipe to play listener for 50ms
    if (unfreezeWhenDestroyed) nav.freezeTouches(.5f);

    Destroy(gameObject);
  }

  public void setCoinsAndMoneyResponseFunctions(Action moneyFunc, Action coinFunc = null)
  {
    // to assign the functions to be called based on user decision

    void finalCoinFunc()
    {
      if (coinFunc == null) return;

      coinFunc.Invoke();
      destroyPurchaseBox();
    }

    void finalMoneyFunc()
    {
      moneyFunc.Invoke();
      destroyPurchaseBox();
    }

    PayWithCoins.onClick.AddListener(() => finalCoinFunc());
    PayWithMoney.onClick.AddListener(() => finalMoneyFunc());
  }

  #endregion
}
