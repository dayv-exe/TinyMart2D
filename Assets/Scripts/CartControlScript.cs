using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

// this script controls the cart in scene

public class CartControlScript : MonoBehaviour
{
  string[] commendationTexts = { "Nice!", "Great job!", "Excellent!", "Brilliant!", "Awesome!", "On Fire!", "Outstanding!", "Superb!" };

  #region vars
  public GameObject leftCart, rightCart;
  public SpriteRenderer[] body, cover, handle, grip, rim1, rim2; // customizable cart parts
  public BoxCollider2D bag; // box collider trigger when item pass through it and is collected into cart
  public GameObject irredeemableHeightMarker; // the height at which the player will no longer be able to collect an item
  GroceriesManagerScript groceriesManagerScript;
  public GameManager gm;
  Rigidbody2D rb; // rigidbody2d for this gameobject
  bool onGround = false; // if the cart has touched the ground, since it it spawn at location (0, 0)
  float cartGround_yPos;
  float moveSpeed; // how fast the cart should move
  public GameObject commendationPopUp;
  int itemPriceIncreaseInARow = 0;
  #endregion

  #region func
  void Start()
  {
    if (body.Length != 2 || cover.Length != 2 || handle.Length != 2 || grip.Length != 2 || rim1.Length != 2 || rim2.Length != 2)
    {
      // the cart in scene is made up of 2 identical carts facing opposite directions, only the one face the direction player is swiping to is visible, the body, cover, handle, grip, and rims must be arrays of 2's so this script can have access to all parts of both carts. Throw error if this is not the case
      throw new UnassignedReferenceException("Make sure you assign all cart parts in the 'CartControlScript' for 'CartInScene' prefab in the resource folder!");
    }
    rb = GetComponent<Rigidbody2D>();

    turnToLeft(false);
  }

  void rollTires()
  {
    // to roll tires with cart 
    foreach (SpriteRenderer tire in new SpriteRenderer[] { rim1[0], rim2[0], rim1[1], rim2[1] })
    {
      tire.gameObject.transform.Rotate(new Vector3(0, 0, -rb.velocity.x / 3f) * moveSpeed);
    }
  }

  void turnToLeft(bool turn)
  {
    // to activate the cart facing the direction the user is swiping to
    leftCart.SetActive(turn);
    rightCart.SetActive(!turn);
  }

  void moveCart()
  {
    if (!onGround || !cache.isSpawningItems) return;

    moveSpeed = 20f;
    //to move cart to where finger is
    //**copied**//
    if (Input.touchCount > 0)
    {
      Touch touch = Input.GetTouch(0);

      if (!cache.ClickedClickable(touch.position))
      {

        Vector3 dragOrigin = Input.mousePosition;

        Vector3 touchPos = Camera.main.ScreenToWorldPoint(touch.position);
        touchPos.z = 0;
        Vector3 touchDirection = (touchPos - transform.position);
        rb.velocity = new Vector2(touchDirection.x * moveSpeed, 0f);

        if (rb.velocity.x < -2)
        {
          //if the cart is moving faster than 0.5f then turn the direction the cart is facing
          turnToLeft(true);
        }
        else if (rb.velocity.x > 2)
        {
          //if the cart is moving faster than 0.5f then turn the direction the cart is facing
          turnToLeft(false);
        }

      }
    }
  }

  public void customMoveCart(Vector2 touchPosition, float speed, bool faceDirection = false)
  {
    if (!onGround || cache.isSpawningItems) return;

    // to move cart to given x position

    Vector3 touchPos = touchPosition;
    touchPos.z = 0;
    Vector3 touchDirection = (touchPos - transform.position);
    rb.velocity = new Vector2(touchDirection.x * speed, 0f);

    if (!faceDirection) return;

    if (rb.velocity.x < -2)
    {
      //if the cart is moving faster than 0.5f then turn the direction the cart is facing
      turnToLeft(true);
    }
    else if (rb.velocity.x > 2)
    {
      //if the cart is moving faster than 0.5f then turn the direction the cart is facing
      turnToLeft(false);
    }

  }
  public void setColor(Color primary, Color secondary, Sprite defaultSkin, Sprite customSkin = null)
  {
    if (onGround) turnToLeft(Convert.ToBoolean(UnityEngine.Random.Range(0, 2))); // faces the cart in a random direction

    // to customize parts of cart
    cover[0].color = secondary;
    grip[0].color = primary;
    rim1[0].color = secondary;
    rim2[0].color = secondary;

    cover[1].color = secondary;
    grip[1].color = primary;
    rim1[1].color = secondary;
    rim2[1].color = secondary;

    if (customSkin != null)
    {
      // to set the body of the cart to a custom skin if it is given in the current cart customization
      body[0].sprite = customSkin;
      body[0].color = Color.white;

      body[1].sprite = customSkin;
      body[1].color = Color.white;
      return;
    }

    // else use default body skin

    body[0].sprite = defaultSkin;
    body[0].color = primary;

    body[1].sprite = defaultSkin;
    body[1].color = primary;
  }
  void OnCollisionEnter2D(Collision2D other)
  {
    // when the cart collides with the ground/floor
    if (other.gameObject.name == "Floor")
    {
      onGround = true;
      turnToLeft(Convert.ToBoolean(UnityEngine.Random.Range(0, 2)));
      cartGround_yPos = transform.position.y;
    }
  }

  private void OnTriggerEnter2D(Collider2D other)
  {
    // if the player collects an item in the cart
    if (other.tag == "item")
    {
      collectItem(other.gameObject);
    }
  }

  int prevItemPrice = 10;
  void collectItem(GameObject item)
  {
    // process the item player collected
    GroceryItemScript itemScript = item.GetComponent<GroceryItemScript>(); // get the script of the item
    if (!groceriesManagerScript) groceriesManagerScript = itemScript.parentScrpt; // get the grocery manager script
    itemScript.destroyWithVFX = false; // to make the item disappear without any animation

    // spawn score pop up
    GameObject scorePopUp = Instantiate(groceriesManagerScript.scorePopUpVFX) as GameObject;
    scorePopUp.GetComponent<ScorePopUpScript>().setScoreText(itemScript.price);
    scorePopUp.transform.position = item.transform.position;

    // animation with random attributes for uniqueness
    LeanTween.moveLocalY(scorePopUp, UnityEngine.Random.Range(0f, 1.5f), .5f).setEaseOutSine();

    groceriesManagerScript.itemsCollected++;
    gm.addToScore(itemScript.price);

    if (prevItemPrice <= itemScript.price)
    {
      itemPriceIncreaseInARow++;
    }
    else
    {
      itemPriceIncreaseInARow = 0;
    }

    prevItemPrice = itemScript.price;

    if (itemPriceIncreaseInARow > 1 && (UnityEngine.Random.Range(0, 2) != 0))
    {
      itemPriceIncreaseInARow = 0;
      // show commendation
      GameObject commendation = Instantiate(commendationPopUp, gm.nav.UiCanvas.transform) as GameObject;
      commendation.transform.position = new Vector3(transform.position.x, 0, 1);
      commendation.GetComponent<RectTransform>().position = new Vector3(commendation.GetComponent<RectTransform>().position.x, 200, 0);
      commendation.GetComponent<CommendationPopUpScript>().setText(commendationTexts[UnityEngine.Random.Range(0, commendationTexts.Length)]);
    }

    if (groceriesManagerScript.getProgressInPercent() == 100) gm.levelUp();

    Destroy(item); // destroy the item
  }

  void FixedUpdate()
  {
    rollTires();
    cache.irredeemableHeight = irredeemableHeightMarker.transform.position.y;
    moveCart();

    if (rb.velocity.y != 0 && onGround)
    {
      transform.position = new Vector3(transform.position.x, cartGround_yPos, 1);
    }
    if ((transform.position.x > 3.5f || transform.position.x < -3.5f) && onGround)
    {
      transform.position = new Vector3(0, cartGround_yPos, 1);
    }
  }

  #endregion
}
