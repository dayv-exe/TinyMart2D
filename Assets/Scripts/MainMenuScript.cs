using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

// MainMenu panel script

public class MainMenuScript : MonoBehaviour
{
  #region var
  public bool EntryAnimComplete = false;

  public Button SoundToggle, BackgroundToggle, CartToggle, NoAds, CurrentLevel, HighScore; // buttons on main menu panel
  public PageNavigationScript nav;
  public GameManager gm;

  public TextMeshProUGUI highScoreTxt;
  #endregion

  #region func

  void Start()
  {
    gm = GameObject.Find("GameManager").GetComponent<GameManager>();
    nav.showPrompt(transform); // to show swipe to play prompt
    setOnClickFunc();
  }

  void FixedUpdate()
  {
    if (nav.playerSwipedToContinue())
    {
      // starts game if user swipes anywhere to play
      gm.startGame();
    }

    highScoreTxt.text = "$" + cache.highscore;

    CurrentLevel.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "" + cache.currentLevel;
  }

  #region btn Funcs
  // the functions to run when any main menu button is clicked

  void setSoundToggleState()
  {
    // to toggle between sound on or off, and change the icon for each state
    string SpritePrefabPath = "Runtime_Sprites/";
    Image SoundBtnImg = cache.findChildInParent(SoundToggle.transform, "Image").gameObject.GetComponent<Image>();

    SoundBtnImg.sprite = (cache.enabledSFX ? Resources.Load<Sprite>(SpritePrefabPath + "unmute") : Resources.Load<Sprite>(SpritePrefabPath + "mute"));
  }

  void soundToggleFunc()
  {
    // to toggle sound
    cache.enabledSFX = !cache.enabledSFX;
    setSoundToggleState();
    cache.SaveGame();
  }

  void backgroundToggleFunc()
  {
    // cycles between background colors
    gm.changeBackgroundColor();
  }

  void cartToggleFunc()
  {
    // to stop swipe to play listener and show shop
    nav.showCoins(nav.showPage("Shop", 0, true)); // to show coin panel permanently until shop is closed
  }

  void noAdsFunc()
  {
    // when player tries to remove ads
    nav.ShowConfirmPurchaseBox(2.99f, () => nav.ShowMessage("Removed ads!"), title: "Remove ads?", UnfreezeWhenDestroyed: true);
  }

  void currentLevelFunc()
  {
    nav.ShowMessage("You're currently on level " + cache.currentLevel);
  }

  void highScoreFunc()
  {
    nav.ShowMessage("Your high score is $" + cache.highscore);
  }

  #endregion

  void setOnClickFunc()
  {
    // assign functions to buttons so they run when clicked

    SoundToggle.onClick.AddListener(soundToggleFunc);
    setSoundToggleState();
    BackgroundToggle.onClick.AddListener(backgroundToggleFunc);
    CartToggle.onClick.AddListener(cartToggleFunc);

    NoAds.onClick.AddListener(noAdsFunc);
    CurrentLevel.onClick.AddListener(currentLevelFunc);
    HighScore.onClick.AddListener(highScoreFunc);
  }

  #endregion
}
