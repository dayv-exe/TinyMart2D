using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

// to set the color of what ever game object script is place on

public class ColorObjectScript : MonoBehaviour
{
  #region var
  public bool hasTMProComponent = false;
  public bool usePrimaryColor = false, useTextColor;
  public bool useColorForTextOutline = false;
  BackgroundColorScript bcs;

  TextMeshProUGUI existingText;
  Image existingImage;

  #endregion

  #region func

  void setColors()
  {
    if (usePrimaryColor)
    {
      if (useColorForTextOutline && hasTMProComponent)
        {
          GetComponent<TextMeshProUGUI>().outlineColor = bcs.primaryColors[cache.chosenBackgroundColor];
          return;
        }
      if (hasTMProComponent) { GetComponent<TextMeshProUGUI>().color = bcs.primaryColors[cache.chosenBackgroundColor]; return; }
      GetComponent<Image>().color = bcs.primaryColors[cache.chosenBackgroundColor];
    }
    else if (useTextColor)
    {
      if (hasTMProComponent)
      {
        if (useColorForTextOutline)
        {
          GetComponent<TextMeshProUGUI>().outlineColor = bcs.textColors[cache.chosenBackgroundColor];
          return;
        }
        GetComponent<TextMeshProUGUI>().color = bcs.textColors[cache.chosenBackgroundColor];
        return;
      }
      GetComponent<Image>().color = bcs.textColors[cache.chosenBackgroundColor];
    }
  }
  void Start()
  {
    bcs = GameObject.Find("GameManager").GetComponent<GameManager>().backgroundColor.GetComponent<BackgroundColorScript>();

    setColors();
  }

  void FixedUpdate()
  {
    setColors();
  }
  #endregion
}
