using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScorePopUpScript : MonoBehaviour
{
  #region vars
  public GameObject lineBurst;
  public TextMeshPro scoreTxt;
  #endregion

  #region func

  void selectRandomColor()
  {

  }

  public void setScoreText(int price)
  {
    scoreTxt.text = "+$" + price;
  }

  #endregion
}
