using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

// this script controls the background and text colors of the game. In the inspector add colors into the color arrays and when player presses the background btn in main menu colors will be cycled through

public class BackgroundColorScript : MonoBehaviour
{
  #region var
  public Color[] backgroundColors, primaryColors, textColors; // list of all game colors
  #endregion

  #region func

  public void setBackgroundColor()
  {
    // to set the current background color index to stored index
    GetComponent<Image>().color = backgroundColors[cache.chosenBackgroundColor];
  }

  public void changeBackgroundColor()
  {
    // to change background color
    cache.chosenBackgroundColor++;
    if (cache.chosenBackgroundColor == backgroundColors.Length) cache.chosenBackgroundColor = 0;
    setBackgroundColor();

    cache.SaveGame(); // saves game after a new color is selected
  }

  #endregion
}
