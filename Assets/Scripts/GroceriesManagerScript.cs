using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// this game controls the spawning of items

public class GroceriesManagerScript : MonoBehaviour
{
  #region vars
  GroceriesManagerScript gmScript;
  [HideInInspector] public GroceryItemScript giScript;
  public GameManager gm;
  public GameObject destroyedItemVFX, scorePopUpVFX; // vfx prefabs
  public Sprite groundShadowSprite;
  public GameObject floor;
  string GroceryItemsPath = "Prefabs/Groceries/"; // path of grocery items prefab
  Transform GroceryParentTransform; // transform.parent of grocery items
  GameObject[] availableItems; // list of 5 random items from 5 random aisles
  string[] aisles; // list of all aisle names

  #region difficulty
  int levelDifficulty = 0; // how fast items should spawn and fall
  float itemsSpawnedInCurrentLevel = 0; // total items spawned in current game
  float[] totalItemsToSpawnedPerLevelDifficulty = new float[5] { 20, 30, 60, 80, 100 }; // level length depending on difficulty level
  [HideInInspector] public float itemsCollected = 0; // number of items player has collected
  int prevItemIndex = 99; // stores the index of the last item spawned to not allow the same item spawn twice

  public GameObject finalItemHighlighter;
  #endregion

  #endregion

  #region func

  public float getProgressInPercent()
  {
    // calculate progress in percent
    return (itemsCollected / totalItemsToSpawnedPerLevelDifficulty[levelDifficulty]) * 100f;
  }

  void setAisles()
  {
    // set the path of all grocery aisles in array
    // list of aisles: beverage, fruits, ice cream, meat, pastry, pizza, sea foods, snacks, sweets, vegetables
    aisles = new string[]
    {
      GroceryItemsPath + "Beverages",
      GroceryItemsPath + "Fruits",
      GroceryItemsPath + "IceCream",
      GroceryItemsPath + "Meat",
      GroceryItemsPath + "Pastry",
      GroceryItemsPath + "Pizza",
      GroceryItemsPath + "SeaFoods",
      GroceryItemsPath + "Snacks",
      GroceryItemsPath + "Sweets",
      GroceryItemsPath + "Vegetables",
    };
  }

  int[] RandomNumbersFromList(int[] numbers, int totalNumbersToSelect)
  {

    if (totalNumbersToSelect > numbers.Length) throw new System.IndexOutOfRangeException("The total amount of random numbers to select must not exceed numbers.length!");

    // selects a random number from list provided

    int[] selectedNumbers = new int[totalNumbersToSelect]; // list rand numbers selected are going to be stored
    for (int i = 0; i < totalNumbersToSelect; i++)
    {
      int chosen = Random.Range(0, numbers.Length); // selects rand index
      selectedNumbers[i] = numbers[chosen]; // stores the selected rand number
      int[] temp = new int[numbers.Length - 1]; // create temp list
      int currentLoc = 0; // keep track of the index of temp list as it fills from 0 to numbers.length - 1
      for (int o = 0; o < numbers.Length; o++)
      {
        if (o != chosen)
        {
          // add the element at 'o' position in numbers array to temp list at 'currentLoc' position if it is not the element chosen at the start of loop
          temp[currentLoc] = numbers[o];
          currentLoc++;
        }
      }

      numbers = temp;
    }

    return selectedNumbers; // returns n amount of random numbers
  }

  GameObject loadItemFromAisle(int aisleIndex)
  {
    // loads up random item from an aisle
    GameObject a = Resources.Load<GameObject>(aisles[aisleIndex]);
    int itemIndex = Random.Range(0, a.transform.childCount); // selects a random item index
    GameObject item = a.transform.GetChild(itemIndex).gameObject;
    return item;
  }

  void Load5RandGroceryItems()
  {
    // select random aisle, select random item 5 times with no aisle repeated
    availableItems = new GameObject[5];
    int[] randomlySelectedAisles = RandomNumbersFromList(new[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 }, 5);
    for (int i = 0; i < 5; i++)
    {
      availableItems[i] = loadItemFromAisle(randomlySelectedAisles[i]);
    }

    randomlySelectedAisles = null;
  }

  void Start()
  {
    setAisles();
  }

  void FixedUpdate()
  {
    cache.percentageOfCurrentLevelCompleted = getProgressInPercent();
  }

  public void startSpawning()
  {
    // starts spawning 5 random items
    cache.isSpawningItems = true;
    Load5RandGroceryItems();
    StartCoroutine(spawnItems());
  }
  public void stopSpawning(bool resetSpawnStats)
  {
    // stops spawning items
    cache.isSpawningItems = false;
    StopCoroutine(spawnItems());

    if (!resetSpawnStats) return;
    itemsCollected = 0; // restarts count on fresh spawning
    itemsSpawnedInCurrentLevel = 0;
  }

  float spawnSpeed()
  {
    // returns time to wait in seconds before new item is spawned after the previous item based on current difficulty level

    return 1f;
  }

  public float fallSpeed()
  {
    // returns gravity scale of new grocery items being spawned based on current difficulty level

    return 1f;
  }

  public void increaseDifficultyLevel()
  {
    
  }

  GameObject dropItem(bool spawnFrmLeft)
  {
    // to create and drop item into scene
    // difficulty determines: total items to spawn, item fall speed, item spawn speed
    int selectedItemIndex = Random.Range(0, availableItems.Length);
    while(selectedItemIndex == prevItemIndex)
    {
      selectedItemIndex = Random.Range(0, availableItems.Length);
    }
    prevItemIndex = selectedItemIndex;
    GameObject gi = Instantiate(availableItems[selectedItemIndex], GroceryParentTransform) as GameObject;

    gi.transform.position = new Vector3(spawnFrmLeft ? -cache.theOTX - gi.transform.localScale.x : cache.theOTX + gi.transform.localScale.x, cache.dropHeight);

    GroceryItemScript giScript = gi.GetComponent<GroceryItemScript>();

    gi.AddComponent<GroundShadowScript>().createShadow(floor.transform, groundShadowSprite, gi);

    giScript.parentScrpt = GetComponent<GroceriesManagerScript>();
    giScript.gm = gm;
    giScript.enabled = true;
    giScript.moveToPos_x = spawnFrmLeft ? -Random.Range(0.5f, 2f) : Random.Range(0.5f, 2f);

    return gi;
  }

  IEnumerator spawnItems()
  {
    // spawn items repeatedly
    bool spawnFrmLeft = System.Convert.ToBoolean(Random.Range(0, 2));
    while (cache.isSpawningItems && itemsSpawnedInCurrentLevel < totalItemsToSpawnedPerLevelDifficulty[levelDifficulty])
    {
      GameObject item = dropItem(spawnFrmLeft);
      spawnFrmLeft = !spawnFrmLeft;
      itemsSpawnedInCurrentLevel++;
      if (totalItemsToSpawnedPerLevelDifficulty[levelDifficulty] == itemsSpawnedInCurrentLevel)
      {
        GameObject fih = Instantiate(finalItemHighlighter) as GameObject;
        fih.transform.position = new Vector3(-10, 100, 1);
        fih.transform.localScale = new Vector3(1.5f, 1.5f, 1);
        fih.GetComponent<FinalItemHighlighterScript>().FollowParent(item);
      }
      yield return new WaitForSeconds(spawnSpeed());
    }
  }

  #endregion
}
