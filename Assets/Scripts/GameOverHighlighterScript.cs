using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// this script controls a highlighter gameobject that circles an item that player failed to collect which ended the game

public class GameOverHighlighterScript : MonoBehaviour
{
  #region vars
  SpriteRenderer spriteR;
  public float flashSpeed = .3f; // how fast to switch between colors
  #endregion

  #region func
  void Start()
  {
    spriteR = GetComponent<SpriteRenderer>();

    flash(Color.white, Color.red, .3f);
  }

  void flash(Color firstColor, Color secondColor, float speed)
  {
    StartCoroutine(beginFlashing(firstColor, secondColor, speed));
  }

  IEnumerator beginFlashing(Color c1, Color c2, float s)
  {
    // switch between colors
    int prevColorIndex = 1;
    while (true)
    {
      spriteR.color = prevColorIndex == 1 ? c1 : c2;
      prevColorIndex = prevColorIndex == 1 ? 0 : 1;
      yield return new WaitForSeconds(s);
    }
  }
  #endregion
}
