using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// to spawn clouds

public class CloudManagerScript : MonoBehaviour
{
  #region var
  public PageNavigationScript nav;
  [HideInInspector] public GameObject[] clouds; // different types of cloud
  public Transform cloudParent; // where to spawn clouds
  #endregion

  #region func
  void Start()
  {
    StartCoroutine(cloudProductionFactory());
  }
  void createCloud()
  {
    // to create a new cloud
    if (clouds.Length < 1) return;

    // height, speed
    GameObject c = Instantiate(clouds[Random.Range(0, clouds.Length)], cloudParent) as GameObject;
    c.transform.position = new Vector3(cache.theOTX + .5f, -cache.theOTY - Random.Range(.25f, 3f));
    c.GetComponent<CloudScript>().floatSpeed = Random.Range(.025f, .05f);
    c.name = "Cloud #" + (Random.Range(1, 100) + Random.Range(0, 10));

    float randScale = Random.Range(.25f, .5f);
    c.transform.localScale = new Vector3(randScale, randScale);
  }

  IEnumerator cloudProductionFactory()
  {
    // endlessly create clouds
    while (true)
    {
      createCloud();
      yield return new WaitForSeconds(5f);
    }
  }
  #endregion
}
