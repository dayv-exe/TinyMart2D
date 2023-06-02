using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SwipeToPlayScript : MonoBehaviour
{
  // controls the swipe to play prompt gameobject
  #region var

  GameObject finger, bar;
  TextMeshProUGUI promptText;
  CartControlScript cartCtrlScript;
  #endregion

  #region func

  void load()
  {
    // to get the children
    finger = cache.findChildInParents(new[] { transform }, "finger");
    bar = cache.findChildInParents(new[] { transform }, "bar");
    promptText = cache.findChildInParents(new[] { (cache.findChildInParents(new[] { transform }, "textBG")).transform }, "text").GetComponent<TextMeshProUGUI>();
  }

  public void showPrompt(Transform parent, string text = "swipe to play", bool showFinger = true)
  {
    load();
    finger.SetActive(showFinger);
    bar.SetActive(showFinger);
    promptText.text = text;
    transform.SetParent(parent);
  }

  void Start()
  {
    cartCtrlScript = GameObject.Find("CartInScene").GetComponent<CartControlScript>();
  }

  void FixedUpdate()
  {
    cartCtrlScript.customMoveCart(finger.transform.position, 2f);
  }
  #endregion
}
