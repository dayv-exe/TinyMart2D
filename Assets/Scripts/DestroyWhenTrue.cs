using System.Collections;
using System.Collections.Generic;
using UnityEngine;

 //  destroys a this.gameobject.parent when variable is set to true

public class DestroyWhenTrue : MonoBehaviour
{
  public bool destroyParent = false;

  void FixedUpdate()
  {
    if (destroyParent) Destroy(gameObject);
  }

}
