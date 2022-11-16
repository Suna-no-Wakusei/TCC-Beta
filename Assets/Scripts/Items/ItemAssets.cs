using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemAssets : MonoBehaviour
{
    public static ItemAssets Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    public Transform pfItemWorld;
    public Transform pfItemUI;
    public Transform pfItemDescUI;

    public Sprite largeHealthPotionSprite;
    public Sprite mediumHealthPotionSprite;
    public Sprite smallHealthPotionSprite;
    public Sprite largeManaPotionSprite;
    public Sprite mediumManaPotionSprite;
    public Sprite smallManaPotionSprite;
    public Sprite greyKeySprite;
    public Sprite goldenKeySprite;
    public Sprite cookie;
}
