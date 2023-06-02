using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RevivePanelScript : MonoBehaviour
{
  #region vars
  public PageNavigationScript nav;
  public Button watchAdBtn;
  #endregion

  #region func
  void returnToMainMenu()
  {
    watchAdBtn.onClick.RemoveAllListeners();
    nav.showPage("MainMenu", 1f, false, pagesToBeRemoved: new string[] { "Revive", "Arena", "Shop" });
  }
  // Start is called before the first frame update
  void Start()
  {
    // ask user to revive within 5 seconds, go back to main menu after time expired
    nav.showPrompt(parent: transform, text: "swipe to continue", showFinger: false);
    nav.startTimer(5, () => returnToMainMenu());
  }

  // Update is called once per frame
  void FixedUpdate()
  {

  }
  #endregion
}
