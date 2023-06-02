using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

// controls navigation between pages

public class PageNavigationScript : MonoBehaviour
{
  #region var
  bool allowTouch = true;
  public readonly string UIPrefabsPath = "Prefabs/UI/", PanelPrefabsPath = "Prefabs/Panels/", CloudsPrefabsPath = "Prefabs/Clouds/", FXPrefabsPath = "Prefabs/FX/"; // paths to prefabs
  public GameObject TopMostCanvas, UiCanvas, GameArea; // canvases containing ui gameobjects
  public GameObject loadingScreen, coinsPanel;
  public GameManager gm;

  #region page classes

  // list of all pages or panels in game
  // ***NOTE: to add a page to the game***
  static class MainMenu { public static bool spawned = false; public static GameObject page = null; }
  static class Arena { public static bool spawned = false; public static GameObject page = null; }
  static class GameOver { public static bool spawned = false; public static GameObject page = null; }
  static class LevelUp { public static bool spawned = false; public static GameObject page = null; }
  static class Revive { public static bool spawned = false; public static GameObject page = null; }
  static class CartSelect { public static bool spawned = false; public static GameObject page = null; }
  static class Pause { public static bool spawned = false; public static GameObject page = null; }
  static class Shop { public static bool spawned = false; public static GameObject page = null; }
  static class LoadingScreen { public static bool spawned = false; public static GameObject page = null; }


  static class StartPlayPrompt { public static bool spawned = false; public static GameObject box = null; }
  static class CoinsPanel { public static GameObject box = null; }
  static class MessageBox { public static bool spawned = false; public static GameObject box = null; }
  static class DecisiveMessageBox { public static bool spawned = false; public static GameObject box = null; }
  static class CountDownBox { public static bool spawned = false; public static GameObject box = null; }

  #endregion

  #endregion

  #region func

  void Start()
  {
    toggleLoadingScreen(true);
    // get the layers for pages
    TopMostCanvas = GameObject.Find("TopMostCanvas");
    UiCanvas = GameObject.Find("UICanvas");
    GameArea = GameObject.Find("GameObjectParent");
  }

  public bool ClickedClickable(Vector3 clickPos) => cache.ClickedClickable(clickPos); // true if clickable item was clicked

  public bool ClickedSomething(Vector3 clickPos) => cache.ClickedSomething(clickPos); // if an object with a collider was clicked

  public bool ClickedThisGameObject(string gmName, Vector3 clickPos) => cache.ClickedThisGameObject(gmName, clickPos); // if given gameobject was clicked

  public bool playerSwipedToContinue()
  {
    return getTouches() != Vector3.zero && !ClickedClickable(getTouches());
  }

  public void freezeTouches(float time)
  {
    // to not register touches until a fixed time has elapsed (useful for preventing start game until message box is completely gone)
    if (time == 0)
    {
      // freeze indefinitely 
      allowTouch = false;
    }
    else if (time == -1)
    {
      // do not freeze if time is set to -1
      allowTouch = true;
    }
    else
    {
      // unfreeze after parsed time has elapsed
      StartCoroutine(pauseTouch(time));
    }
  }

  IEnumerator pauseTouch(float t)
  {
    allowTouch = false;
    yield return new WaitForSeconds(t);
    allowTouch = true;
    StopCoroutine(pauseTouch(0));
  }

  public Vector3 getTouches(bool overrideFreeze = false)
  {
    // returns the location of a click and zero if there is no click
    Vector3 touchPos;

    if (!overrideFreeze && !allowTouch) return Vector3.zero;

    if (Input.touchCount > 0)
    {
      Touch touch = Input.GetTouch(0);
      touchPos = touch.position;
    }
    else if (Input.GetMouseButtonDown(0))
    {
      touchPos = Input.mousePosition;
    }
    else
    {
      touchPos = Vector3.zero;
    }

    return touchPos;
  }

  GameObject initMB(string aText, string aTitle, bool isDecisiveMessageBox = false)
  {
    freezeTouches(0);
    GameObject mb = Instantiate(isDecisiveMessageBox ? DecisiveMessageBox.box : MessageBox.box, TopMostCanvas.transform) as GameObject;
    MessageBoxScript mbScript = mb.GetComponent<MessageBoxScript>();
    mbScript.nav = GetComponent<PageNavigationScript>();
    mbScript.setMessage(aText, aTitle);
    return mb;
  }

  public void ShowMessage(string aText, string aTitle = "Info")
  {
    // to show message box with given messages
    initMB(aText, aTitle);
  }

  public void ShowMessage(string aText, string aTitle, Vector2 boxSize, Vector2 boxPos)
  {
    // to show message box with given messages
    GameObject mb = initMB(aText, aTitle);

    if (boxSize != Vector2.zero)
      mb.GetComponent<MessageBoxScript>().setBoxSize(boxSize);

    if (boxPos != Vector2.zero)
      mb.transform.GetChild(0).GetComponent<RectTransform>().anchoredPosition = boxPos;
  }

  public void startTimer(int time, Action funcToRunAfter, Sprite TimerImage = null)
  {
    // to show a seconds count down timer
    GameObject t = Instantiate(CountDownBox.box, TopMostCanvas.transform) as GameObject;
    CountDownPanelScript cdpScript = t.GetComponent<CountDownPanelScript>();
    cdpScript.setTimer(time, funcToRunAfter, TimerImage);
  }

  CountDownPanelScript cdpScript;
  public void startCounter(int startNumber, Sprite counterImage)
  {
    // to show count of something left
    GameObject t = Instantiate(CountDownBox.box, TopMostCanvas.transform) as GameObject;
    cdpScript = t.GetComponent<CountDownPanelScript>();
    cdpScript.setCounter(startNumber, counterImage);
  }

  public void countDown(Action funcToRunAtZero)
  {
    // to minus one from count when called
    if (!cdpScript) throw new Exception("Can not call the 'countDown' function before calling the 'setCounter' function. Call the 'setCounter' function and use this 'countDown' function to minus 1 from the start number until it gets to zero then the counter ends or else Use 'setTimer' to count down by 1 every second");
    cdpScript.countDown(funcToRunAtZero);
  }

  void coinPanelActivation()
  {
    CoinsPanel.box.SetActive(false);
    CoinsPanel.box.SetActive(true);
  }

  public void showCoins(float showForSeconds)
  {
    // to show the total user coins collected
    CoinsPanelScript coinsPnlScript = CoinsPanel.box.GetComponent<CoinsPanelScript>();
    // coinsPnlScript.nav = GetComponent<PageNavigationScript>();
    coinPanelActivation();
    coinsPnlScript.showCoins(showForSeconds);
  }

  public void showCoins(GameObject parent)
  {
    // if parent is destroyed the coins panel will be too
    CoinsPanelScript coinsPnlScript = CoinsPanel.box.GetComponent<CoinsPanelScript>();
    // coinsPnlScript.nav = GetComponent<PageNavigationScript>();
    coinPanelActivation();
    coinsPnlScript.showCoinsUntilParentDestroyed(parent);
  }

  public void addCoins(int amount)
  {
    // to show coin increment animation
    CoinsPanelScript coinsPnlScript = CoinsPanel.box.GetComponent<CoinsPanelScript>();
    // coinsPnlScript.nav = GetComponent<PageNavigationScript>();
    showCoins(7.5f);
    coinsPnlScript.addCoins(amount);
  }

  public void minusCoins(int amount)
  {
    // to show coin decrement anim
    CoinsPanelScript coinsPnlScript = CoinsPanel.box.GetComponent<CoinsPanelScript>();
    // coinsPnlScript.nav = GetComponent<PageNavigationScript>();
    showCoins(7.5f);
    coinsPnlScript.minusCoins(amount);
  }

  public void ShowDecisiveMessageBox(string aText, string aTitle, string posOption, string negOption, Action posFunction, Action negFunction = null)
  {
    // to show message box with options
    GameObject mb = initMB(aText, aTitle, true);
    MessageBoxScript mbScript = mb.GetComponent<MessageBoxScript>();
    mbScript.setOptions(negOption, posOption);
    mbScript.setPositiveAndNegativeResponseFunctions(posFunction, negFunction);
  }
  public void showPrompt(Transform parent, string text = "swipe to play", bool showFinger = true)
  {
    // to show swipe to play prompt
    GameObject prompt = Instantiate(StartPlayPrompt.box, TopMostCanvas.transform) as GameObject;
    SwipeToPlayScript promptScript = prompt.GetComponent<SwipeToPlayScript>();
    // promptScript.nav = GetComponent<PageNavigationScript>();
    promptScript.showPrompt(parent, text, showFinger);
  }

  public GameObject useFX(string FXname, Vector3 spawnPos)
  {
    // to spawn any effect from resources
    GameObject fx;
    fx = Instantiate(Resources.Load<GameObject>(FXPrefabsPath + FXname), GameArea.transform) as GameObject;
    fx.gameObject.transform.position = spawnPos;

    return fx;
  }

  public GameObject[] LoadAllClouds()
  {
    // returns left right center cloud prefabs

    GameObject cloud_left = Resources.Load<GameObject>(CloudsPrefabsPath + "cloud_left");
    GameObject cloud_centre = Resources.Load<GameObject>(CloudsPrefabsPath + "cloud_centre");
    GameObject cloud_right = Resources.Load<GameObject>(CloudsPrefabsPath + "cloud_right");

    return new GameObject[] { cloud_left, cloud_centre, cloud_right };
  }
  public void ShowConfirmPurchaseBox(float worthInMoney, Action moneyPaymentFunc, int worthInCoins = 0, Action coinsPaymentFunc = null, string title = "Confirm purchase", bool UnfreezeWhenDestroyed = true)
  {
    freezeTouches(0);

    // to show the confirm purchase box
    GameObject cp = Instantiate(Resources.Load<GameObject>(UIPrefabsPath + "ConfirmPurchaseBox"), TopMostCanvas.transform) as GameObject;

    ConfirmPurchaseScript cpScript = cp.GetComponent<ConfirmPurchaseScript>();
    cpScript.nav = GetComponent<PageNavigationScript>();

    cpScript.setOptions(title, worthInMoney, worthInCoins);
    cpScript.setCoinsAndMoneyResponseFunctions(moneyPaymentFunc, coinsPaymentFunc);
    cpScript.unfreezeWhenDestroyed = UnfreezeWhenDestroyed;
  }


  #region PAGE NAVIGATION
  public void toggleLoadingScreen(bool active)
  {
    loadingScreen.SetActive(active);
  }
  public void LoadGame(Action FuncToRunAfterLoading)
  {
    toggleLoadingScreen(true);
    GameManager gm = GameObject.Find("GameManager").GetComponent<GameManager>();
    // to load all objects needed for game

    // load highscore, current level, chosen background color & image, audio state, chosen cart, unlocked carts
    // 1 game sound load on game start
    // load 9 pages and 1 message box on start up
    // 2 background in storage only load one on start up, and the other when user chooses it
    // load first cart on game start, load other carts when they have been selected by user
    // load first five grocery items on game start, then continue to load other 5 on level up

    // loading...

    cache.loadGameData();

    // ...

    MessageBox.box = Resources.Load<GameObject>(UIPrefabsPath + "MessageBox");
    DecisiveMessageBox.box = Resources.Load<GameObject>(UIPrefabsPath + "DecisiveMessageBox");
    CoinsPanel.box = coinsPanel;
    CoinsPanel.box.SetActive(false);
    StartPlayPrompt.box = Resources.Load<GameObject>(UIPrefabsPath + "Prompt");
    CountDownBox.box = Resources.Load<GameObject>(UIPrefabsPath + "Timer");

    // to load clouds

    gm.GetComponent<CloudManagerScript>().clouds = LoadAllClouds();

    // ...

    gm.setBackgroundColor();

    // done loading
    toggleLoadingScreen(false);
    FuncToRunAfterLoading.Invoke();
  }
  System.Object pageOperations(string pageName, int operation, bool value = false)
  {
    PageNavigationScript nav = GetComponent<PageNavigationScript>();
    // FOR PAGES ONLY NOT BOXES
    // to perform operations on given page classes
    // 1 = get page (returns gameobject), 2 = get spawned (returns bool), 3 = set spawned (set bool), 4 = assign navigationManagerScript to selected page (set gm in page script)
    if (pageName == "MainMenu")
    {
      if (operation == 1) return MainMenu.page;
      else if (operation == 2) return MainMenu.spawned;
      else if (operation == 3)
      {
        MainMenu.spawned = value;
        if (!value) MainMenu.page = null; // to unload page from RAM when it is destroyed
        return null;
      }
      else if (operation == 4)
      {
        // to load up the page prefab and assign the pageNavigation variable
        MainMenu.page = Resources.Load<GameObject>(PanelPrefabsPath + "MainMenuPanel");
        MainMenu.page.tag = "page";
        MainMenu.page.GetComponent<MainMenuScript>().nav = nav;
      }
    }
    else if (pageName == "Arena")
    {
      if (operation == 1) return Arena.page;
      else if (operation == 2) return Arena.spawned;
      else if (operation == 3)
      {
        Arena.spawned = value;
        if (!value) Arena.page = null; // to unload page from RAM when it is destroyed
        return null;
      }
      else if (operation == 4)
      {
        // to load up the page prefab and assign the pageNavigation variable
        Arena.page = Resources.Load<GameObject>(PanelPrefabsPath + "ArenaPanel");
        Arena.page.tag = "page";
        Arena.page.GetComponent<ArenaScript>().nav = nav;
      }
    }
    else if (pageName == "GameOver")
    {
      if (operation == 1) return GameOver.page;
      else if (operation == 2) return GameOver.spawned;
      else if (operation == 3)
      {
        GameOver.spawned = value;
        if (!value) GameOver.page = null; // to unload page from RAM when it is destroyed
        return null;
      }
      else if (operation == 4)
      {
        // to load up the page prefab and assign the pageNavigation variable
        GameOver.page = Resources.Load<GameObject>(PanelPrefabsPath + "GameOverPanel") as GameObject;
        GameOver.page.tag = "page";
        GameOver.page.GetComponent<GameOverPanelScript>().nav = nav;
      }
    }
    else if (pageName == "LevelUp")
    {
      if (operation == 1) return LevelUp.page;
      else if (operation == 2) return LevelUp.spawned;
      else if (operation == 3)
      {
        LevelUp.spawned = value;
        if (!value) LevelUp.page = null; // to unload page from RAM when it is destroyed
        return null;
      }
      else if (operation == 4)
      {
        // to load up the page prefab and assign the pageNavigation variable
        LevelUp.page = Resources.Load<GameObject>(PanelPrefabsPath + "LevelUpPanel") as GameObject;
        LevelUp.page.tag = "page";
        LevelUp.page.GetComponent<LevelUpPanelScript>().nav = nav;
      }
    }
    else if (pageName == "Revive")
    {
      if (operation == 1) return Revive.page;
      else if (operation == 2) return Revive.spawned;
      else if (operation == 3)
      {
        Revive.spawned = value;
        if (!value) Revive.page = null; // to unload page from RAM when it is destroyed
        return null;
      }
      else if (operation == 4)
      {
        // to load up the page prefab and assign the pageNavigation variable
        Revive.page = Resources.Load<GameObject>(PanelPrefabsPath + "RevivePanel");
        Revive.page.tag = "page";
        Revive.page.GetComponent<RevivePanelScript>().nav = GetComponent<PageNavigationScript>();
      }
    }
    else if (pageName == "CartSelect")
    {
      if (operation == 1) return CartSelect.page;
      else if (operation == 2) return CartSelect.spawned;
      else if (operation == 3)
      {
        CartSelect.spawned = value;
        if (!value) CartSelect.page = null; // to unload page from RAM when it is destroyed
        return null;
      }
      else if (operation == 4)
      {
        // to load up the page prefab and assign the pageNavigation variable
        Debug.LogWarning("Navigation Manager not assigned for this page");
        CartSelect.page.tag = "page";
      }
    }
    else if (pageName == "Pause")
    {
      if (operation == 1) return Pause.page;
      else if (operation == 2) return Pause.spawned;
      else if (operation == 3)
      {
        Pause.spawned = value;
        if (!value) Pause.page = null; // to unload page from RAM when it is destroyed
        return null;
      }
      else if (operation == 4)
      {
        // to load up the page prefab and assign the pageNavigation variable
        Pause.page = Resources.Load<GameObject>(PanelPrefabsPath + "PausePnl");
        Pause.page.tag = "page";
        Debug.Log("Navigation Manager unassigned for pause panel");
      }
    }
    else if (pageName == "Shop")
    {
      if (operation == 1) return Shop.page;
      else if (operation == 2) return Shop.spawned;
      else if (operation == 3)
      {
        Shop.spawned = value;
        if (!value) Shop.page = null; // to unload page from RAM when it is destroyed
        return null;
      }
      else if (operation == 4)
      {
        // to load up the page prefab and assign the pageNavigation variable
        Shop.page = Resources.Load<GameObject>(PanelPrefabsPath + "Shop");
        Shop.page.tag = "page";
        Shop.page.GetComponent<ShopScript>().nav = nav;
      }
    }
    else
    {
      throw new NullReferenceException(String.Format("'{0}' prefab not found", pageName));
    }

    return null;
  }

  public void pauseGame()
  {
    // pause panel contains scrollRect component, resume game when scroll is detected(only possible if user swipes)
    // to show pause panel and set the listener to resume game
    GameObject pausePanel = showPage("Pause", -1, true);
    pausePanel.GetComponentInChildren<ScrollRect>().onValueChanged.AddListener(delegate { resumeGame(pausePanel); });
    Time.timeScale = 0;
  }

  public void resumeGame(GameObject pausePanel)
  {
    // to remove pause panel and resume game
    showPage("", -1, pagesToBeRemoved: new string[] { "Pause" });

    StartCoroutine(slowMoResume());
  }

  IEnumerator slowMoResume()
  {
    while (Time.timeScale < 1)
    {

      Time.timeScale += .1f;
      yield return new WaitForSecondsRealtime(.2f);

    }

    StopAllCoroutines();
  }

  GameObject spawnPage(string pageName, GameObject page, Transform parent)
  {
    // spawns page
    if (Convert.ToBoolean(pageOperations(pageName, 2))) return null; // returns null if page is already visible
    GameObject p = Instantiate(page, parent) as GameObject;
    pageOperations(pageName, 3, true); // set is spawned
    p.name = pageName;
    return p;
  }

  public GameObject showPage(string pageName, float touchWaitTime, bool onTopMostLayer = false, string[] pagesToBeRemoved = null)
  {
    // destroys given page and spawns given page
    // MainMenu Arena GameOver LevelUp Revive CartSelect Pause Shop

    if (pagesToBeRemoved != null && pagesToBeRemoved.Length > 0)
    {
      foreach (string page in pagesToBeRemoved)
      {
        // to remove only a given page
        removePage(page);
      }
    }

    if (pageName == "") return null; // returns null if no page is given

    pageOperations(pageName, 4);
    freezeTouches(touchWaitTime);
    return spawnPage(pageName, pageOperations(pageName, 1) as GameObject /*to get gameobject*/, onTopMostLayer ? TopMostCanvas.transform : UiCanvas.transform);
  }

  public void removePage(string pageName)
  {
    // find the page to be destroyed in top most layer, if not there find in ui layer
    Destroy(cache.findChildInParents(new[] { UiCanvas.transform, TopMostCanvas.transform }, pageName));
    pageOperations(pageName, 3, false); // set is spawned
  }
  #endregion

  #region popups



  #endregion

  #endregion
}
