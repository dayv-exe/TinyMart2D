using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// controls individual clouds

public class CloudScript : MonoBehaviour
{
  #region var
  [HideInInspector] public float floatSpeed; // how fast cloud moves across screen
  #endregion

  #region func
  void moveCloud()
  {
    // to move cart across screen
    GetComponent<Rigidbody2D>().AddForce(new Vector2(-floatSpeed, 0), ForceMode2D.Force);
    if (transform.position.x < -cache.theOTX - .75f) Destroy(gameObject);
  }

  void FixedUpdate()
  {
    moveCloud();
  }
  #endregion
}
