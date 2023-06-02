using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

// controls message box pop up

public class MessageBoxScript : MonoBehaviour
{
  #region variables
  public bool isDecisiveMessageBox = false; // if message box should have options
  public bool doneWithAnim = false; // if exit animation has finished playing
  Vector2 prevSize = Vector2.zero; // the previous size of the message box (for touch boundaries)
  public TextMeshProUGUI theText, theTitle;
  public Button yes, no;
  public PageNavigationScript nav;
  #endregion

  #region functions
  public void setMessage(string text, string title)
  {
    // set the texts to show
    theTitle.text = title;
    theText.text = text;
  }

  public void setOptions(string negText, string posText)
  {
    if (!isDecisiveMessageBox) throw new System.FormatException("Can not set options on an informative message box!");

    // to set the options for a decisive message box
    no.GetComponentInChildren<TextMeshProUGUI>().text = negText;
    yes.GetComponentInChildren<TextMeshProUGUI>().text = posText;
  }

  public void setBoxSize(Vector2 size)
  {
    // to set custom WIDTH ONLY(width is set to fit content by default)
    if (size == Vector2.zero) return;

    theTitle.GetComponent<RectTransform>().sizeDelta = size;
    theText.GetComponent<RectTransform>().sizeDelta = size;
  }

  public void destroyMessageBox()
  {
    if (!doneWithAnim) return;

    // removes the message and stops swipe to play listener for 50ms
    nav.freezeTouches(.5f);

    Destroy(gameObject);
  }

  public void setPositiveAndNegativeResponseFunctions(Action yFunc, Action nFunc = null)
  {
    // to assign the functions to be called based on user decision

    if (!isDecisiveMessageBox) throw new System.FormatException("Can not set user response functions on an informative message box!");

    void finalPosFunc()
    {
      yFunc.Invoke();
      destroyMessageBox();
    }

    void finalNegFunc()
    {
      if (nFunc == null)
      {
        destroyMessageBox();
        return;
      }

      nFunc.Invoke();
      destroyMessageBox();
    }

    yes.onClick.AddListener(() => finalPosFunc());
    no.onClick.AddListener(() => finalNegFunc());
  }
  #endregion
}