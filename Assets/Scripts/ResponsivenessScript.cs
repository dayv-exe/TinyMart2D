using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// to make sure the game fits well on the screen

public class ResponsivenessScript : MonoBehaviour
{
  #region variables
  public SpriteRenderer rink; // for calc screen bounds
  Vector2 prevScreenSize = Vector2.zero; // to keep track of when screen size changes
  public GameObject theFloor; // the game platform
  public float floorPos_OTY_DivideBy = 1.5f; // the position of the floor determined by dividing the camera's orthographic size by the var (it works for whatever reason)
  public float launch_y_DividedBy = .5f; // the height where items are dropped from determined by dividing the camera's orthographic size by the var (it works for whatever reason)
  #endregion

  #region functions

  void Start()
  {
    // to find variables
    setScreenBounds();
  }

  void FixedUpdate()
  {
    setScreenBounds();
  }

  void setScreenBounds()
  {
    // to fit game to screen every time screen size changes
    if (!rink)
    {
      throw new System.Exception("Rink not found!");
    }

    if (prevScreenSize != new Vector2(Screen.width, Screen.height))
    {

      prevScreenSize = new Vector2(Screen.width, Screen.height);
      float orthoSize = rink.bounds.size.x * Screen.height / Screen.width * 0.5f; //to calc the new orthographic size
      Camera.main.orthographicSize = orthoSize; //to set the new orthographic size
      cache.OrthographicBounds(Camera.main); //to get screen bounds

      theFloor.transform.position = new Vector3(0, -(Camera.main.orthographicSize / floorPos_OTY_DivideBy)); // to set floor pos
      cache.dropHeight = Camera.main.orthographicSize / launch_y_DividedBy;
      Debug.Log(cache.dropHeight);

      // wall pos
      GameObject[] theWall = { cache.findChildInParents(new[] { theFloor.transform.parent }, "Left Wall"), cache.findChildInParents(new[] { theFloor.transform.parent }, "Right Wall") };

      theWall[0].transform.position = new Vector3(-(cache.theOTX + (theWall[0].transform.localScale.x / 2)), 0);

      theWall[1].transform.position = new Vector3(cache.theOTX + (theWall[1].transform.localScale.x / 2), 0);
    }
  }
  #endregion
}
