using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// adds box colliders to parent gameobject to allow user click or touch parent gameobject. this is useful to avoid triggering swipe to continue feature.

[RequireComponent(typeof(RectTransform))]
public class ClickableScript : MonoBehaviour
{
  #region var
  public bool DynamicWidth = false; // if width is dynamic, always listen for change in parent size, then recalculate collider size if change detected
  public bool useProvidedColliders = false; // set to true if the item already has colliders and you want to use it and not let script generate new colliders
  RectTransform rTransform;
  #endregion

  #region func

  void setClickBoundaries(GameObject victim, bool setChildToo = false, bool useEdgeRadius = false)
  {
    RectTransform victim_rTransform = victim.GetComponent<RectTransform>();
    // to add a collider to a gameobject and its children if specified

    /* NOTE: manually set any gameobject tag to "nonclickable" to be ***ignored*** by this script */

    victim.tag = "clickable";
    if (victim.tag == "nonclickable" || useProvidedColliders) return;

    BoxCollider2D bx2d = victim.AddComponent<BoxCollider2D>();
    bx2d.isTrigger = true;
    bx2d.size = new Vector2(victim_rTransform.rect.width, victim_rTransform.rect.height);
    bx2d.edgeRadius = useEdgeRadius ? .1f : 0; // padding around the gameobject

    if (setChildToo)
    {
      // add colliders to children if specified
      for (int i = 0; i < victim.transform.childCount; i++)
      {
        setClickBoundaries(victim.transform.GetChild(i).gameObject);
      }
    }
  }

  public void setCustomBoundaries(GameObject victim, Vector2 newSize, bool setChildToo = false, bool useEdgeRadius = false)
  {
    RectTransform victim_rTransform = victim.GetComponent<RectTransform>();
    if (useProvidedColliders)
    {
      // if the collider the gomeobject has is not auto generated
      throw new System.NotSupportedException("Cannot modify the size of a custom collider!");
    }
    // to change the size of the collider on the parent gameobject
    BoxCollider2D bx2d = victim.GetComponent<BoxCollider2D>();
    bx2d.size = newSize == Vector2.zero ? new Vector2(victim_rTransform.rect.width, victim_rTransform.rect.height) : newSize;
    bx2d.edgeRadius = useEdgeRadius ? .1f : 0;

    if (setChildToo)
    {
      // to change the size of the collider on the children gameobject if specified
      for (int i = 0; i < victim.transform.childCount; i++)
      {
        setCustomBoundaries(victim.transform.GetChild(i).gameObject, Vector2.zero);
      }
    }
  }

  void Start()
  {
    setClickBoundaries(gameObject, true, true);
    rTransform = GetComponent<RectTransform>();
  }

  void FixedUpdate()
  {
    if (!DynamicWidth) return;
    // recalculate clickable boundaries after size changed
    Vector2 victimSize = new Vector2(rTransform.rect.width, rTransform.rect.height);
    if (GetComponent<BoxCollider2D>().size != victimSize)
    {
      setCustomBoundaries(gameObject, rTransform.rect.size, true, true);
    }
  }
  #endregion
}
