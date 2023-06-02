using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// this script is adds shadow cast by items as they get closer to the gruond

public class GroundShadowScript : MonoBehaviour
{
  #region vars
  Sprite shadowSprite;
  SpriteRenderer sr;
  Transform floor;
  GameObject theGroundShadow;

  GameObject parentsLife;

  bool createdShadow = false;

  float parentScaledWidth; // the width of parent multiplied by scale
  float shadowFullScaledWidth; // the scale shadow prefab should be at to match parent width
  float trueScale; // the final scale of shadow prefab when parent in closest to ground
  #endregion

  #region func
  public void createShadow(Transform theFloorTransform, Sprite theShadowSprite, GameObject parentsGameObject)
  {
    parentsLife = parentsGameObject;
    floor = theFloorTransform;
    shadowSprite = theShadowSprite;

    // size
    // ***NOTE: the y axis of scale should be 8 times smaller than the x axis of scale to make shadow perfectly flat on the ground***
    theGroundShadow = new GameObject("_" + name + "'s ground shadow");
    theGroundShadow.transform.SetParent(floor);
    theGroundShadow.transform.localScale = Vector3.zero;
    sr = theGroundShadow.AddComponent<SpriteRenderer>();
    sr.sprite = shadowSprite;
    sr.sortingLayerName = "PopUpLayer";
    sr.color = new Color(0, 0, 0, .3f);

    theGroundShadow.transform.localPosition = new Vector3(transform.position.x, .25f, 1);

    parentScaledWidth = GetComponent<SpriteRenderer>().sprite.rect.width * transform.localScale.x; // the width of parent multiplied by scale
    shadowFullScaledWidth = parentScaledWidth * 0.008f; // 0.008 floats represents one unit of parent width, multiply 0.008 to get scale shadow prefab should be at to match parent width
    trueScale = shadowFullScaledWidth + (shadowFullScaledWidth); // the full final scale of the shadow

    createdShadow = true;
  }

  void calculateShadowTransform()
  {
    if (!createdShadow || transform.position.y > 0) return;

    theGroundShadow.transform.position = new Vector3(transform.position.x, theGroundShadow.transform.position.y);

    float distanceFromGroundInFraction = transform.position.y / Mathf.Abs(theGroundShadow.transform.position.y);

    float currentWidth = (distanceFromGroundInFraction) * trueScale;
    float currentHeight = currentWidth / 8;

    theGroundShadow.transform.localScale = new Vector3(currentWidth, currentHeight, 1);
  }

  private void OnDestroy()
  {
    Destroy(theGroundShadow);
  }

  private void FixedUpdate()
  {
    calculateShadowTransform();
  }
  #endregion
}