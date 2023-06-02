using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// this scripts gives a custom shadow to its parent

public class CustomShadowScript : MonoBehaviour
{
  #region vars
  public Vector2 shadowOffset;
  public Color shadowColor = new Color(0, 0, 0, .58f);
  GameObject shadow;
  #endregion

  #region func
  void setShadowSprite()
  {
    shadow.AddComponent<SpriteRenderer>().sprite = GetComponent<SpriteRenderer>().sprite;
  }
  void castShadow()
  {
    GameObject s = new GameObject("_" + name + "'s shadow");
    shadow = Instantiate(s, transform) as GameObject;
    setShadowSprite();
    shadow.GetComponent<SpriteRenderer>().color = shadowColor;
    shadow.transform.localPosition = new Vector3(0 + shadowOffset.x, 0 - shadowOffset.y, 1);
  }
  private void Start()
  {
    castShadow();
  }
  private void FixedUpdate()
  {
    setShadowSprite();
  }
  #endregion
}
