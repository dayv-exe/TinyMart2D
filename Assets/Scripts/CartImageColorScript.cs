using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

// this script controls dummy carts displayed in the cart customization page

[RequireComponent(typeof(RectTransform))]
public class CartImageColorScript : MonoBehaviour
{
  #region vars
  [HideInInspector] public CartManagerScript cartManagerScrpt; // cart manager script
  public Image body, cover, handle, grip, rim1, rim2; // customizable parts of cart
  public Image lockedCartIndicator, selectedCartIndicator; // images to show if the cart is locked or selected
  public bool isLocked = true; // true if unlocked
  [HideInInspector] public PageNavigationScript nav;
  float[] price; // list of prices
  int cartIndex; // cart index
  #endregion

  #region func

  public void CheckCartState()
  {
    // to check if current cart is unlocked or selected and change the status image accordingly

    isLocked = true;

    for (int i = 0; i < cache.unlockedCarts.Length; i++)
    {
      if (cache.unlockedCarts[i] == cartIndex)
      {
        // the cart is unlocked
        isLocked = false;
        break;
      }
    }
  }

  void paidWithMoney()
  {
    //  google pay verification
    nav.ShowMessage("Purchase failed!");
  }

  void paidWithCoins()
  {
    if (cache.coins < price[0])
    {
      // if not enough coins
      nav.ShowMessage("Not enough coins!", "Purchase failed");
    }
    else
    {
      // if enough coins, unlock and select
      nav.minusCoins(Convert.ToInt32(price[0]));
      isLocked = false;
      cache.addToUnlockedCarts(cartIndex);
      selectThisCart();
    }
  }

  public void selectThisCart()
  {
    // to select this cart
    if (isLocked)
    {
      nav.ShowConfirmPurchaseBox(price[1], paidWithMoney, Convert.ToInt32(price[0]), paidWithCoins, "Buy cart?", false);
      return;
    }

    cartManagerScrpt.ChangeCart(cartIndex);

    CheckCartState();
  }

  public void setColor(int index, int priceMultiplier, Color primary, Color secondary, Sprite defaultSkin, Sprite customSkin = null)
  {

    // to customize individual cart

    price = new[] { 1000 * priceMultiplier, .5f * priceMultiplier }; // price of cart increases in each row

    cartIndex = index; // assign index
    CheckCartState(); // update the status images

    // set parts colors
    cover.color = secondary;
    grip.color = primary;
    rim1.color = secondary;
    rim2.color = secondary;

    if (customSkin != null)
    {
      // use custom skin for body if available
      body.sprite = customSkin;
      body.color = Color.white;
      return;
    }

    // else use default skin
    body.sprite = defaultSkin;
    body.color = primary;
  }

  void FixedUpdate()
  {
    // to enable or disable the indicators (update status images)
    if (isLocked)
    {
      lockedCartIndicator.gameObject.SetActive(true);
      selectedCartIndicator.gameObject.SetActive(false);
    }
    else
    {
      lockedCartIndicator.gameObject.SetActive(false);
    }

    if (cache.chosenCart == cartIndex) // is selected
    {
      isLocked = false;
      selectedCartIndicator.gameObject.SetActive(true);
    }
    else
    {
      selectedCartIndicator.gameObject.SetActive(false);
    }
  }
  #endregion
}
