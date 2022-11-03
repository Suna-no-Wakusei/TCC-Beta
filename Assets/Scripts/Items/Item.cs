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
            case ItemType.LargeHealthPotion: return "Poção de Cura Grande";
            case ItemType.MediumHealthPotion: return "Poção de Cura Média";
            case ItemType.SmallHealthPotion: return "Poção de Cura Pequena";
            case ItemType.LargeManaPotion: return "Poção de Mana Grande";
            case ItemType.MediumManaPotion: return "Poção de Mana Média";
            case ItemType.SmallManaPotion: return "Poção de Mana Pequena";
            case ItemType.GreyKey: return "Chave Cinza";
            case ItemType.GoldenKey: return "Chave Dourada";
        }
    }

    public string ItemDesc()
    {
        switch (itemType)
        {
            default:
            case ItemType.LargeHealthPotion: return "Essa poção cura 50% do seu HP máximo";
            case ItemType.MediumHealthPotion: return "Essa poção cura 75HP";
            case ItemType.SmallHealthPotion: return "Essa poção cura 50HP";
            case ItemType.LargeManaPotion: return "Essa poção restaura 100% da sua mana";
            case ItemType.MediumManaPotion: return "Essa poção restaura 75% da sua mana";
            case ItemType.SmallManaPotion: return "Essa poção restaura 50% da sua mana";
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
