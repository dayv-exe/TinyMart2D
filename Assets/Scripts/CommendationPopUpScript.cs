using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CommendationPopUpScript : MonoBehaviour
{
  #region vars
  public TextMeshProUGUI commendationText;
  #endregion

  #region 
  
  void Start()
  {
  }

  public void setText(string text)
  {
    commendationText.text = text;

    Destroy(gameObject, 3f);
  }
  #endregion
}
