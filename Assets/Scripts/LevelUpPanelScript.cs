using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

// this script controls the level up panel

public class LevelUpPanelScript : MonoBehaviour
{
  #region vars
  public TextMeshProUGUI nextLevelText;
  public TextMeshProUGUI scoreText;
  public TextMeshProUGUI rewardText;
  public PageNavigationScript nav;
  public GameObject coinBlastAnim;
  #endregion

  #region func
  void Start()
  {
    nav.showPrompt(gameObject.transform, "swipe to continue", false);

    cache.currentLevel++;
    cache.SaveGame();
    cache.currentLevel--;

    nextLevelText.text = "Level:\n" + cache.currentLevel;
    scoreText.text = "Score: $" + cache.score;
    rewardText.text = "Reward: +    " + cache.getDifficultyLevel() * (10 + Random.Range(1, 6));

    StartCoroutine(showCoinAnim());
  }

  IEnumerator showCoinAnim()
  {
    yield return new WaitForSeconds(.25f);
    GameObject cba = Instantiate(coinBlastAnim) as GameObject;
    cba.transform.position = new Vector3(0f, 1f, 1);
    cba.transform.localScale = new Vector3(3, 3, 3);

    StopCoroutine(showCoinAnim());
  }

  void levelUp()
  {

  }

  public void backToMainMenu()
  {
    nav.gm.backToMainMenu("LevelUp", new string[] { "Arena" });
  }

  public void hidePanel()
  {
    // remove panel
    nav.gm.startNextLevel();
    cache.currentLevel++;
  }

  void FixedUpdate()
  {
    if (nav.getTouches() != Vector3.zero && !nav.ClickedSomething(nav.getTouches()))
    {
      // continue game
      hidePanel();
    }
  }
  #endregion
}
