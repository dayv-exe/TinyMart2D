using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

// general script for storing variables and functions to be accessed from any script in the game

public static class cache
{
  #region variables
  public static int score = 0; // players current score
  public static int displayScore = 0; // the score that should be displayed as text and is to count up incrementally to 'score'
  public static float theOTX, theOTY; // the x and y coordinates of the screen boundaries
  public static float irredeemableHeight; // the height that if an item is below play fails the game, player can no longer collect an item below that height
  public static float dropHeight; // the y pos items should be dropped from
  public static bool isSpawningItems = false; // set to true if the game is currently being played
  public static float percentageOfCurrentLevelCompleted = 0; // stores current level progress
  #endregion

  #region functions

  #region storage

  // set storage default values in "loadGameData" function

  public static bool isLoading = true; // indicates game is loading
  public static int highscore = 0; // the highest score player has achieved
  public static int currentLevel = 1; // current player level
  public static int chosenBackgroundColor = 0; // index of current background color
  public static bool enabledSFX = true; // true is sound is enabled by user
  public static int chosenCart = 0; // the index of chosen cart
  public static int coins = 0; // total coins user has accumulated
  public static int[] unlockedCarts = new[] { 0 }; // index of all unlocked carts

  public static string gameDataDir() => Application.persistentDataPath + "/TinyProgress.small"; // where game data is stored
  public static void loadGameData()
  {

    // to load game

    if (File.Exists(gameDataDir()))
    {
      // if the game data exists, load it
      PlayerData data = SaveSystem.getSavedData();
      highscore = data.highscore;
      currentLevel = data.currentLevel;
      chosenBackgroundColor = data.chosenBackgroundColor;
      enabledSFX = data.enabledSFX;
      chosenCart = data.chosenCart;
      coins = data.coins;
      unlockedCarts = data.unlockedCarts;
    }
    else
    {

      // if the game data does not exist, set their default values

      highscore = 0;
      currentLevel = 1;
      chosenBackgroundColor = 0;
      enabledSFX = true;
      chosenCart = 0;
      coins = 10000;
      unlockedCarts = new[] { 0 };
    }
  }
  public static void SaveGame()
  {
    // saves game
    SaveSystem.storeData();
  }

  public static void deleteGameFiles()
  {
    if (File.Exists(gameDataDir()))
    {
      File.Delete(gameDataDir());
    }
  }

  #endregion

  public static Bounds OrthographicBounds(this Camera camera)
  {

    //  to calc camera orthographic size to fit the game into any screen size
    if (!camera.orthographic)
    {
      Debug.Log(string.Format("The camera {0} is not Orthographic!", camera.name), camera);
      return new Bounds();
    }

    var t = camera.transform;
    var x = t.position.x;
    var y = t.position.y;
    var size = camera.orthographicSize * 2;
    var width = size * (float)Screen.width / Screen.height;
    var height = size;

    // the edge of the screen vertically and horizontally
    theOTX = width / 2;
    theOTY = -height / 2;


    return new Bounds(new Vector3(x, y, 0), new Vector3(width, height, 0));
  }

  public static bool isInGameArea(float x)
  {
    // returns true if the x coordinates parsed is inside of screen bounds
    return Mathf.Abs(x) < theOTX;
  }

  public static GameObject findChildInParents(Transform[] parents, string childName)
  {
    // searches a list of transforms to find given child
    GameObject child = null;

    for (int t = 0; t < parents.Length; t++)
    {
      for (int i = 0; i < parents[t].childCount; i++)
      {
        if (parents[t].GetChild(i).name == childName)
        {
          child = parents[t].GetChild(i).gameObject;
          break;
        }
      }
    }

    return child;
  }

  public static GameObject findChildInParent(Transform parents, string childName)
  {
    // searches a transforms to find given child
    GameObject child = null;

    for (int i = 0; i < parents.childCount; i++)
    {
      if (parents.GetChild(i).name == childName)
      {
        child = parents.GetChild(i).gameObject;
        break;
      }
    }

    return child;
  }

  static string checkClicked(Vector3 pos, bool returnTag = false)
  {
    // returns the name or tag of whatever collider.gameobject the raycast collides with
    string theTouchedItem = "";
    RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(pos), Vector2.zero);

    if (hit.collider != null)
    {

      theTouchedItem = returnTag ? hit.collider.tag : hit.collider.name;

    }

    return theTouchedItem;
  }

  public static bool ClickedClickable(Vector3 clickPos)
  {
    // sends out raycast to touched position, returns true if raycast hits collider with tag "clickable"

    // for a gameobject to be clickable add the clickable script to it's component

    bool verdict = false;

    if (checkClicked(clickPos, true) == "clickable")
    {
      verdict = true;
    }

    return verdict;
  }

  public static bool ClickedSomething(Vector3 clickPos)
  {
    // sends out raycast to touched position, returns true if the raycast hits any collider with a name
    bool verdict = false;

    if (checkClicked(clickPos, true) == "clickable" || checkClicked(clickPos) != "")
    {
      verdict = true;
    }

    return verdict;
  }

  public static bool ClickedThisGameObject(string gmName, Vector3 clickPos)
  {
    // sends out raycast to touched position, returns true if the raycast hits a collider.gameobject with same name as the given name parameter
    bool verdict = false;

    if (checkClicked(clickPos) == gmName)
    {
      verdict = true;
    }

    return verdict;
  }

  public static string ChangeNumber(float amount)
  {
    //to shorten number by replacing most of it  with decimal

    if (amount >= 1000000000000)
    {
      return (amount / 1000000000000).ToString("#.#") + "t";
    }
    else if (amount >= 1000000000)
    {
      return (amount / 1000000000).ToString("#.#") + "b";
    }
    else if (amount >= 1000000)
    {
      return (amount / 1000000).ToString("#.#") + "m";
    }
    else if (amount >= 1000)
    {
      return (amount / 1000).ToString("#.#") + "k";
    }
    else
    {
      return amount.ToString();
    }
  }

  public static void addToUnlockedCarts(int index)
  {
    // to add a newly unlocked cart to the list of unlocked carts
    int[] _temp = unlockedCarts;

    unlockedCarts = new int[_temp.Length + 1];

    for (int i = 0; i < unlockedCarts.Length; i++)
    {
      unlockedCarts[i] = i == _temp.Length ? index : _temp[i]; // add new cart when transfer of temp is complete
    }

    printList(unlockedCarts);

    _temp = null;
  }

  public static void printList(int[] list)
  {
    // to print out the content of an int[] and separate elements with ,
    string print = "--++START++--\n";
    for (int i = 0; i < list.Length; i++)
    {
      print += list[i];

      if (i != list.Length) print += "\n";

    }

    print += "++--END--++";

    Debug.Log(print);
  }

  static int difficultyLevel = 1; // min: 1;

  public static int getDifficultyLevel()
  {
    return difficultyLevel;
  }

  public static void setDifficultyLevelByGrocManagertOnly(int num)
  {
    difficultyLevel = num;
  }

  #endregion
}