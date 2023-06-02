using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// this script controls grocery item

public class GroceryItemScript : MonoBehaviour
{
  #region vars
  public int price; // score when collected
  public Vector2 itemScale = new Vector3(.6f, .6f, 1); // the default size of the item when spawned
  public Vector2 customShadowPosition = new Vector2(-.08f, -.08f); // default shadow position
  [Range(0, 255)] public int shadowAlphaVal = 150; // the transparency of shadow
  [HideInInspector] public GroceriesManagerScript parentScrpt;
  public GameManager gm;
  bool startedGameOverSequence = false; // to stop the game over animations from repeating since they are called in the "fixed update" function
  bool touchedGroundOrIrredeemable = false; // if the item has touched the ground or player can no longer get to it
  public bool destroyWithVFX = true; // to show a visual effect when item is destroyed
  public float moveToPos_x; // position the item should be moved to on the x axis after it is spawned
  bool moved = false; // true if item has been moved to final position on x axis

  Rigidbody2D rb2d;
  BoxCollider2D bxCol2d;
  SpriteRenderer sr;

  #endregion

  #region funcs
  void addShadow()
  {
    // creates shadow and add to item to make it pop
    GameObject shad = new GameObject("Shadow");
    shad.transform.SetParent(gameObject.transform);
    shad.transform.localScale = new Vector3(1, 1, 1);
    shad.AddComponent<SpriteRenderer>();
    shad.GetComponent<SpriteRenderer>().sprite = sr.sprite;
    shad.GetComponent<SpriteRenderer>().sortingLayerName = "GroceriesLayer";
    shad.GetComponent<SpriteRenderer>().sortingOrder = 0;
    shad.GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, shadowAlphaVal / 255f);
    shad.transform.localPosition = customShadowPosition;
  }

  void prepItem()
  {
    // adds rigidbody and colliders to item
    transform.localScale = itemScale;
    sr = GetComponent<SpriteRenderer>();
    sr.sortingLayerName = "GroceriesLayer";
    sr.sortingOrder = 1;
    addShadow();

    tag = "item";

    bxCol2d = gameObject.AddComponent<BoxCollider2D>();
    bxCol2d.size = new Vector2(bxCol2d.size.x - .5f, bxCol2d.size.y - .5f);
    rb2d = gameObject.AddComponent<Rigidbody2D>();
    rb2d.gravityScale = parentScrpt.fallSpeed();
    rb2d.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
    rb2d.drag = 5f;
    rb2d.constraints = RigidbodyConstraints2D.FreezeRotation;

    price = price == 0 ? Random.Range(1, 6) : price;
  }

  void moveTo(float pos_x)
  {
    // to move grocery item to given x position in game play area

    Vector3 touchPos = new Vector3(pos_x, 0);
    touchPos.z = 0;
    Vector3 touchDirection = (touchPos - transform.position);
    rb2d.velocity = new Vector2(touchDirection.x * 10f, -1f);
    if (cache.isInGameArea(transform.position.x) && Mathf.Abs(rb2d.velocity.x) < 1)
    {
      rb2d.velocity = new Vector2(0, rb2d.velocity.y);
      moved = true;
    }
  }

  private void OnDestroy()
  {
    // to spawn "pop" animation after item is destroyed
    if (!destroyWithVFX) return;
    GameObject vfx = Instantiate(parentScrpt.destroyedItemVFX) as GameObject;
    vfx.transform.position = transform.position;
  }

  IEnumerator destroyItemAndEndGame()
  {
    // to freeze item while game over highlighter plays it's animation and end game afterwards
    rb2d.constraints = RigidbodyConstraints2D.FreezeAll;
    gm.spawnScript.stopSpawning(false);

    GameObject f = Instantiate(Resources.Load<GameObject>(parentScrpt.gm.nav.FXPrefabsPath + "gameOverItemHighliter")) as GameObject;

    f.transform.parent = transform;
    f.transform.localScale = new Vector3(transform.localScale.x + .5f, transform.localScale.y + .5f, 1);
    f.transform.localPosition = Vector3.zero;

    yield return new WaitForSeconds(1.25f); // how long game over highlighter should play it's animation for before the game is subsequently ended

    Destroy(f);
    gm.GameOver(sr.sprite);
    Destroy(gameObject);
  }

  void isIrredeemable()
  {
    // if this item is at a height where it can no longer be collected
    touchedGroundOrIrredeemable = true;
  }

  private void OnCollisionEnter2D(Collision2D other)
  {
    if (other.gameObject.name == "Floor")
    {
      // item is irredeemable is it hits the floor
      isIrredeemable();
    }
  }

  private void FixedUpdate()
  {
    if (!cache.isSpawningItems && !touchedGroundOrIrredeemable) Destroy(gameObject); // if items are no longer being spawned, destroy all items in scene
    if (transform.position.y <= cache.irredeemableHeight && !startedGameOverSequence)
    {
      // game over if item is irredeemable
      startedGameOverSequence = true;
      isIrredeemable();
      StartCoroutine(destroyItemAndEndGame());
    }
    if (rb2d && !moved) moveTo(moveToPos_x); // to move item to position in scene once only
  }

  void Start()
  {
    prepItem();
  }
  #endregion
}
