using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopScript : MonoBehaviour
{
  // for the shop panel
  #region vars
  public RectTransform store;
  public bool completedAnimation = false; // true if exit animation is done playing
  public PageNavigationScript nav;
  CartManagerScript cartManager;
  Animator anim;
  int prevCartIndex;
  #endregion

  #region func

  public void hidePanel()
  {
    // to trigger panel removal
    anim.SetBool("showPanel", false);
  }

  void Start()
  {
    // load all cart variants when shop is active
    anim = GetComponent<Animator>();
    prevCartIndex = cache.chosenCart;
    GameObject.Find("CartManager").GetComponent<CartManagerScript>().CreateCartInstanceInStore(store);
  }

  void FixedUpdate()
  {
    if (completedAnimation)
    {
      // to properly remove panel
      nav.freezeTouches(.5f);
      completedAnimation = false;
      nav.removePage("Shop");
      anim.SetBool("showPanel", true);
    }

    // if change in cart is detected
    if (prevCartIndex != cache.chosenCart)
    {
      prevCartIndex = cache.chosenCart;
      hidePanel();
    }
  }
  #endregion
}
