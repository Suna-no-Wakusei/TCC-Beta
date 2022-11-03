using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Item
{
    
    public enum ItemType
    {
        LargeHealthPotion,
        MediumHealthPotion,
        SmallHealthPotion,
        LargeManaPotion,
        MediumManaPotion,
        SmallManaPotion,
        GreyKey,
        GoldenKey,
    }


    public ItemType itemType;
    public int amount;


    public Sprite GetSprite()
    {
        switch (itemType)
        {
            default:
            case ItemType.LargeHealthPotion: return ItemAssets.Instance.largeHealthPotionSprite;
            case ItemType.MediumHealthPotion:   return ItemAssets.Instance.mediumHealthPotionSprite;
            case ItemType.SmallHealthPotion: return ItemAssets.Instance.smallHealthPotionSprite;
            case ItemType.LargeManaPotion: return ItemAssets.Instance.largeManaPotionSprite;
            case ItemType.MediumManaPotion: return ItemAssets.Instance.mediumManaPotionSprite;
            case ItemType.SmallManaPotion: return ItemAssets.Instance.smallManaPotionSprite;
            case ItemType.GreyKey: return ItemAssets.Instance.greyKeySprite;
            case ItemType.GoldenKey: return ItemAssets.Instance.goldenKeySprite;
        }
    }

    public string ItemName()
    {
        switch (itemType)
        {
            default:
            case ItemType.LargeHealthPotion: return "Po��o de Cura Grande";
            case ItemType.MediumHealthPotion: return "Po��o de Cura M�dia";
            case ItemType.SmallHealthPotion: return "Po��o de Cura Pequena";
            case ItemType.LargeManaPotion: return "Po��o de Mana Grande";
            case ItemType.MediumManaPotion: return "Po��o de Mana M�dia";
            case ItemType.SmallManaPotion: return "Po��o de Mana Pequena";
            case ItemType.GreyKey: return "Chave Cinza";
            case ItemType.GoldenKey: return "Chave Dourada";
        }
    }

    public string ItemDesc()
    {
        switch (itemType)
        {
            default:
            case ItemType.LargeHealthPotion: return "Essa po��o cura 50% do seu HP m�ximo";
            case ItemType.MediumHealthPotion: return "Essa po��o cura 75HP";
            case ItemType.SmallHealthPotion: return "Essa po��o cura 50HP";
            case ItemType.LargeManaPotion: return "Essa po��o restaura 100% da sua mana";
            case ItemType.MediumManaPotion: return "Essa po��o restaura 75% da sua mana";
            case ItemType.SmallManaPotion: return "Essa po��o restaura 50% da sua mana";
            case ItemType.GreyKey: return "Uma chave cinza";
            case ItemType.GoldenKey: return "Uma chave dourada";
        }
    }

    public bool IsStackable()
    {
        switch (itemType)
        {
            default:
            case ItemType.GoldenKey:
            case ItemType.GreyKey:
                return false;
            case ItemType.LargeHealthPotion:
            case ItemType.MediumHealthPotion:
            case ItemType.SmallHealthPotion:
            case ItemType.LargeManaPotion:   
            case ItemType.MediumManaPotion:
            case ItemType.SmallManaPotion:
                return true;
        }
    }
}
