using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// to add different cart customization options to store (dummy carts), and spawn new carts and change the cart colors when necessary

public class CartManagerScript : MonoBehaviour
{
  #region vars
  public GameObject CartPrefab, CartImagePrefab; // real cart, dummy cart

  // NOTE: TO CREATE A NEW CART ADD A PRIMARY COLOR, SECONDARY COLOR, HANDLE COLOR AND A CUSTOM SKIN IF YOU WANT, ADD THESE TO THE COLORS AND SKIN ARRAYS IN "cartManagerScript" ***IN INSPECTOR***. THIS WILL ADD A NEW INSTANCE OF A CART WITH THOSE SPECIFIED COLORS.

  GameObject cartInScene;
  public Transform cartInSceneParent; // where to spawn real cart
  public Color[] primary, secondary; // list of colors
  public Sprite[] customSkin; // list of skins
  public Sprite NormalSkin; // default skin
  string CartImagePrefabPath = "Prefabs/Carts/"; // location of cart prefabs
  public PageNavigationScript nav;
  public GameManager gameManagerScript;

  public GameObject commendationPopUpPrefab;
  #endregion

  #region func
  void spawnCart()
  {
    // to spawn a new cart ***(runs ones when game starts)***
    cartInScene = Instantiate(CartPrefab, cartInSceneParent) as GameObject;
    cartInScene.name = "CartInScene";
    colorCart(cartInScene);
  }

  void colorCart(GameObject theCart)
  {
    // to customize in game cart
    nav.useFX("CartSpawnedFX", cartInScene.transform.position); // shows smoke effect when cart is spawned
    cartInScene.GetComponent<CartControlScript>().setColor(primary[cache.chosenCart], secondary[cache.chosenCart], NormalSkin, (customSkin[cache.chosenCart] == null) ? null : customSkin[cache.chosenCart]); // changes the cart in scene to new colors
    cartInScene.GetComponent<CartControlScript>().gm = gameManagerScript;
    cartInScene.GetComponent<CartControlScript>().commendationPopUp = commendationPopUpPrefab;
  }

  public void CreateCartInstanceInStore(RectTransform storeTransform)
  {
    // to create different dummy carts in shop

    if (primary.Length > secondary.Length)
    {
      // if not enough colors where specified to add new instance of carts
      throw new System.ArgumentNullException("The length of secondary colors array must match the length of primary colors array! Add a new secondary color in 'CartManager'gameobject in the inspector to add a new cart customization option.");
    }
    else if (primary.Length > customSkin.Length)
    {
      // the length of the custom skin array must match the length of the primary array, if you dont wont a particular instance to use custom skins, add a new item to the array but leave the item null (the cart would then use the default body)
      throw new System.ArgumentNullException("The length of custom skin array must match the length of primary colors array! Add a new custom skin in 'CartManager' gameobject in the inspector, leave the new custom skin null to use default skin.");
    }

    int priceMultiplier = 0;
    for (int i = 0; i < primary.Length; i++)
    {
      if (i % 3 == 0) priceMultiplier++; // to set the prices of the carts, all carts on the same row cost the same. the base price for a cart is 1000 ($0.50) the carts on the next row should cost (base price * multiplier) more.

      GameObject c = Instantiate(CartImagePrefab, storeTransform) as GameObject; 

      CartImageColorScript cScript = c.GetComponent<CartImageColorScript>();

      if (primary[i].a < 1) Debug.LogWarning("Alpha value of a primary color is below 255, fix this in 'CartManager' gameobject in the inspector");

      if (secondary[i].a < 1) Debug.LogWarning("Alpha value of a secondary color is below 255, fix this in 'CartManager' gameobject in the inspector");

      cScript.nav = nav;
      cScript.cartManagerScrpt = gameObject.GetComponent<CartManagerScript>();
      cScript.setColor(i, priceMultiplier, primary[i], secondary[i], NormalSkin, (customSkin[i] == null) ? null : customSkin[i]);
    }

  }

  public void ChangeCart(int newIndex)
  {
    // to change the color of cart in scene
    cache.chosenCart = newIndex;
    colorCart(cartInScene);
    cache.SaveGame();
  }

  void Start()
  {
    spawnCart();
  }

  #endregion
}
