using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// to follow the last item to be spawned in a level

public class FinalItemHighlighterScript : MonoBehaviour
{
  #region vars
  GameObject parent;
  bool lockedOnParent = false;
  #endregion

  #region func

  public void FollowParent(GameObject p)
  {
    parent = p;
    lockedOnParent = true;
  }

  void FixedUpdate()
  {
    if (lockedOnParent)
    {
      if (!parent)
      {
        Destroy(gameObject);
        return;
      }
      transform.position = parent.transform.position;
    }
  }

  #endregion
}
