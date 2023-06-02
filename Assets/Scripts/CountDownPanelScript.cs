using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using TMPro;

// this script controls a count down timer popup, with function to run after countdown is complete

public class CountDownPanelScript : MonoBehaviour
{
  #region vars
  Animator anim;
  public Slider progressBar;
  public Image countdownImg;
  public TextMeshProUGUI countTxt;
  bool usingCounter = false; // if the count down panel is not counting down with time
  int count = 0;
  #endregion

  #region func

  void setAnims()
  {
    // exit animation
    anim = GetComponent<Animator>();
    anim.SetBool("timerRunningOut", false);
    anim.SetBool("complete", false);
  }

  public void setTimer(int timeInSeconds, Action funcInvokeAfter, Sprite displayImage = null)
  {
    setAnims();
    usingCounter = false;
    // to count down by one every second
    // to show and start the timer
    countTxt.gameObject.SetActive(false);
    progressBar.gameObject.SetActive(true);

    progressBar.value = 100;
    // leave img as null to use default clock image
    if (displayImage) countdownImg.sprite = displayImage;
    StartCoroutine(timer(timeInSeconds, funcInvokeAfter));
  }

  public void setCounter(int startNum, Sprite displayImage)
  {
    setAnims();
    usingCounter = true;
    // to count down when count down func called
    progressBar.gameObject.SetActive(false);
    countTxt.gameObject.SetActive(true);

    countTxt.text = startNum + "";
    count = startNum;
    countdownImg.sprite = displayImage;
  }

  public int countDown(Action funcInvokeAfter)
  {
    // minus one from count when called, ends when count hits 0
    if (!usingCounter) throw new Exception("Can not call the 'countDown' function before calling the 'setCounter' function. Call the 'setCounter' function and use this 'countDown' function to minus 1 from the start number until it gets to zero then the counter ends or else Use 'setTimer' to count down by 1 every second");
    if (count > 0) count -= 1;
    if (count == 0)
    {
      // play exit anim
      funcInvokeAfter.Invoke();
      anim.SetBool("timerRunningOut", true);
      anim.SetBool("complete", true);
    }
    return count;
  }

  IEnumerator timer(int t, Action f)
  {
    float time = t;
    while (true)
    {
      if (((time / t) * 100 <= 20) || t < 10) // if 15% or less time is left, or total time < 10
      {
        // play time running out anim
        anim.SetBool("timerRunningOut", true);
      }
      yield return new WaitForSeconds(1f);
      // minus one from time left every second
      time -= 1;
      float timeLeft = (time / t) * 100;
      progressBar.value = timeLeft;
      if (time == 0) // if time is up
      {
        // play exit anim
        f.Invoke();
        anim.SetBool("complete", true);
        StopAllCoroutines();
      }
    }
  }

  void FixedUpdate()
  {
    // to keep count up-to-date
    countTxt.text = count + "";
  }
  #endregion
}
