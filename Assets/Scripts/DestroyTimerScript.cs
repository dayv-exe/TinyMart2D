using System.Collections;
using System.Collections.Generic;
using UnityEngine;

  // destroys the parent gameobject after fixed time has elapsed


public class DestroyTimerScript : MonoBehaviour
{
  #region vars
  public float destroyAfterSeconds = 0f;
  #endregion

  #region func
  void Start()
  {
    if (destroyAfterSeconds > 0) StartCoroutine(destructionTimer());
  }
  IEnumerator destructionTimer()
  {
    yield return new WaitForSeconds(destroyAfterSeconds);
    Destroy(gameObject);
  }
  #endregion
}
